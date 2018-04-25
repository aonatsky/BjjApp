using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Core.Services.Impl;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class BracketService : IBracketService
    {
        #region Dependencies

        private readonly IRepository<Bracket> _bracketRepository;
        private readonly IRepository<Round> _roundRepository;
        private readonly IParticipantService _participantService;
        private readonly BracketsFileService _bracketsFileService;
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly ICategoryService _categoryService;

        #endregion

        #region .ctor

        public BracketService(IRepository<Bracket> bracketRepository,
            IParticipantService participantService,
            BracketsFileService bracketsFileService,
            IWeightDivisionService weightDivisionService,
            ICategoryService categoryService,
            IRepository<Round> roundRepository)
        {
            _bracketRepository = bracketRepository;
            _participantService = participantService;
            _bracketsFileService = bracketsFileService;
            _weightDivisionService = weightDivisionService;
            _categoryService = categoryService;
            _roundRepository = roundRepository;
        }

        #endregion

        #region Public Methods

        public async Task<BracketModel> GetBracketModelAsync(Guid weightDivisionId)
        {
            var bracket = await GetBracketAsync(weightDivisionId);
            var roundTime = await _categoryService.GetRoundTimeAsync(bracket.WeightDivision.CategoryId);

            return GetBracketModel(bracket, roundTime);
        }

        public async Task<BracketModel> RunBracketAsync(Guid weightDivisionId)
        {
            var bracket = await GetBracketAsync(weightDivisionId);
            bracket.StartTs = DateTime.UtcNow;
            var firstRounds = bracket.Rounds.Where(r => r.Stage == bracket.Rounds.Max(rr => rr.Stage));
            foreach (var round in firstRounds)
            {
                if (round.FirstParticipantId == null)
                {
                    round.WinnerParticipantId = round.SecondParticipantId;
                }
                if (round.SecondParticipantId == null)
                {
                    round.WinnerParticipantId = round.SecondParticipantId;
                }

                bracket.Rounds.First(r => r.RoundId == round.NextRoundId).FirstParticipantId =
                    round.WinnerParticipantId;
            }

            var roundTime = await _categoryService.GetRoundTimeAsync(bracket.WeightDivision.CategoryId);
            _bracketRepository.Update(bracket);
            return GetBracketModel(bracket, roundTime);
        }

        public async Task<bool> IsWinnersSelectedForAllRoundsAsync(Guid categoryId)
        {
            var braketsForCategory = _bracketRepository.GetAll(b => b.WeightDivision.CategoryId == categoryId);
            if (!await braketsForCategory.AnyAsync())
            {
                return false;
            }
            if (!await braketsForCategory.AnyAsync(b =>
                b.Rounds.Any(r => r.Stage == 0 && r.RoundType == (int)RoundTypeEnum.Standard)))
            {
                return false;
            }
            return !await GetWinnersForBrackets(braketsForCategory).AnyAsync(p => p == null);
        }

        public async Task<List<ParticipantInAbsoluteDivisionMobel>> GetWinnersAsync(Guid categoryId)
        {
            var braketsForCategory = _bracketRepository.GetAll(b => b.WeightDivision.CategoryId == categoryId);
            if (await GetWinnersForBrackets(braketsForCategory).AnyAsync(p => p == null))
            {
                throw new Exception($"For Weight divisions for category with id {categoryId} not all rounds has winners");
            }

            var winnerParticipants =
                await GetWinnersForBrackets(braketsForCategory
                        .Include(p => p.Rounds)
                        .ThenInclude(r => r.WinnerParticipant).ThenInclude(p => p.Team)
                        .Include(p => p.Rounds)
                        .ThenInclude(r => r.WinnerParticipant).ThenInclude(p => p.WeightDivision))
                    .Select(p => new ParticipantInAbsoluteDivisionMobel()
                    {
                        ParticipantId = p.ParticipantId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        TeamName = p.Team.Name,
                        WeightDivisionName = p.WeightDivision.Name,
                        IsSelectedIntoDivision = p.AbsoluteWeightDivisionId != null
                    })
                    .ToListAsync();
            return winnerParticipants;
        }

        public async Task UpdateBracket(BracketModel model)
        {

            var bracket = await _bracketRepository.GetAll(b => b.BracketId == model.BracketId).Include(b => b.Rounds).FirstOrDefaultAsync();
            if (bracket != null)
            {
                UpdateBracketRoundsFromModel(bracket.Rounds, model.RoundModels);
            }
        }

        public async Task<CustomFile> GetBracketFileAsync(Guid weightDivisionId)
        {
            var bracket = await _bracketRepository.GetAll(b => b.WeightDivisionId == weightDivisionId)
                .Include(b => b.Rounds).ThenInclude(r => r.FirstParticipant)
                .Include(b => b.Rounds).ThenInclude(r => r.SecondParticipant)
                .FirstOrDefaultAsync();
            return await _bracketsFileService.GetBracketsFileAsync(GetOrderedParticipantListFromBracket(bracket));
        }

        public async Task<Dictionary<string, BracketModel>> GetBracketsByCategoryAsync(Guid categoryId)
        {
            var weightDivisions = await _weightDivisionService.GetWeightDivisionsByCategoryIdAsync(categoryId);
            var result = new Dictionary<string, BracketModel>();
            foreach (var division in weightDivisions)
            {
                result.Add(division.WeightDivisionId.ToUpperInvariant(), await GetBracketModelAsync(new Guid(division.WeightDivisionId)));
            }
            return result;
        }

        public async Task ManageAbsoluteWeightDivisionAsync(CreateAbsoluteDivisionModel model)
        {
            var absoluteWeightDivision = await _weightDivisionService.GetAbsoluteWeightDivisionAsync(model.CategoryId);

            await _participantService.AddAbsoluteWeightDivisionForParticipantsAsync(
                model.ParticipantsIds,
                model.CategoryId,
                absoluteWeightDivision.WeightDivisionId);
        }

        public async Task SetRoundResultAsync(RoundResultModel model)
        {
            var round = await _roundRepository.GetAllIncluding(r => r.RoundId == model.RoundId, r => r.NextRound).FirstOrDefaultAsync();
            if (round != null)
            {
                round.WinnerParticipantId = model.WinnerParticipantId;
                round.RoundResultType = (int)model.RoundResultTypeType;
                round.RoundResultDetails = GetRoundResultDetailsJson(model);
                if (round.NextRound != null)
                {
                    if (round.NextRound.FirstParticipantId == null)
                    {
                        round.NextRound.FirstParticipantId = round.WinnerParticipantId;
                    }
                    else
                    {
                        round.NextRound.SecondParticipantId = round.WinnerParticipantId;
                    }
                }

                if (round.Stage == 1)
                {
                    var lostParticipantId = round.WinnerParticipantId == round.FirstParticipantId
                        ? round.SecondParticipantId
                        : round.FirstParticipantId;
                    var thirdPlaceRound = await _roundRepository.FirstOrDefaultAsync(r =>
                        r.BracketId == round.BracketId || r.RoundType == (int)RoundTypeEnum.ThirdPlace);
                    if (thirdPlaceRound != null)
                    {
                        if (thirdPlaceRound.FirstParticipantId == null)
                        {
                            thirdPlaceRound.FirstParticipantId = lostParticipantId;
                        }
                        else
                        {
                            thirdPlaceRound.SecondParticipantId = lostParticipantId;
                        }
                        _roundRepository.Update(thirdPlaceRound);
                    }
                }

                if (round.Stage == 0)
                {
                    if (!await _roundRepository.GetAll(r => r.BracketId == round.BracketId
                                                           && r.Stage == 0
                                                           && r.RoundId != round.RoundId
                                                           && r.WinnerParticipantId == null
                                                           ).AnyAsync())
                    {

                        var bracket = await _bracketRepository.GetAllIncluding(b => b.BracketId == round.BracketId, b => b.WeightDivision).FirstOrDefaultAsync();
                        bracket.CompleteTs = DateTime.UtcNow;
                        _bracketRepository.Update(bracket);
                        await _categoryService.SetCategoryCompleteAsync(bracket.WeightDivision.CategoryId);

                    }
                }

                _roundRepository.Update(round);
            }
        }

        #endregion

        #region Private methods

        private async Task<List<Participant>> GetOrderedListForNewBracketAsync(Guid weightDivisionId)
        {
            var participants = (await _participantService.GetParticipantsByWeightDivisionAsync(weightDivisionId)).ToList();
            var count = participants.Count();
            var bracketSize = GetBracketsSize(count);
            for (var i = 0; i < bracketSize - count; i++)
            {
                participants.Add(_participantService.GetEmptyParticipant());
            }

            return DistributeParticipants(participants);
        }

        private List<Participant> GetOrderedParticipantListFromBracket(Bracket bracket)
        {
            var participantList = new List<Participant>();
            var maxStage = bracket.Rounds.Max(r => r.Stage);
            var firstRounds = bracket.Rounds.Where(r => r.Stage == maxStage);
            foreach (var firstRound in firstRounds)
            {
                participantList.Add(firstRound.FirstParticipant);
                participantList.Add(firstRound.SecondParticipant);
            }
            return participantList;

        }

        private const int FightersMaxCount = 64;

        private int GetBracketsSize(int fightersCount)
        {
            if (fightersCount == 3)
            {
                return 3;
            }
            for (var i = 1; i <= Math.Log(FightersMaxCount, 2); i++)
            {
                var size = Math.Pow(2, i);
                if (size >= fightersCount)
                {
                    return (int)size;
                }
            }
            return 2;
        }

        private List<Participant> DistributeParticipants(List<Participant> participantList)
        {

            var orderedbyTeam = participantList.ToList().GroupBy(f => f.Team).OrderByDescending(g => g.Count())
                .SelectMany(f => f).ToList();
            if (participantList.Count > 2)
            {
                var sideA = new List<Participant>();
                var sideB = new List<Participant>();
                for (var i = 0; i < orderedbyTeam.Count; i++)
                {
                    var fighter = orderedbyTeam.ElementAtOrDefault(i);
                    if (i % 2 == 0)
                    {
                        sideA.Add(fighter);
                    }
                    else
                    {
                        sideB.Add(fighter);
                    }
                }
                return DistributeParticipants(sideA).Concat(DistributeParticipants(sideB)).ToList();
            }
            return participantList;
        }

        private BracketModel GetBracketModel(Bracket bracket, int roundTime)
        {

            var model = new BracketModel
            {
                BracketId = bracket.BracketId,
                RoundModels = new List<RoundModel>()
            };
            foreach (var round in bracket.Rounds)
            {
                model.RoundModels.Add(GetRoundModel(round, roundTime));
            }
            return model;
        }

        private RoundModel GetRoundModel(Round round, int roundTime)
        {
            var model = new RoundModel()
            {
                RoundId = round.RoundId,
                NextRoundId = round.NextRoundId,
                Stage = round.Stage,
                FirstParticipant = round.FirstParticipant == null ? null : GetParticipantModel(round.FirstParticipant),
                SecondParticipant = round.SecondParticipant == null ? null : GetParticipantModel(round.SecondParticipant),
                RoundType = round.RoundType,
                RoundTime = roundTime,
            };
            if (round.WinnerParticipantId != null)
            {

                if (round.WinnerParticipantId == round.FirstParticipantId)
                {
                    if (round.RoundResultType != (int)RoundResultTypeEnum.DQ)
                    {
                        model.FirstParticipantResult = ((RoundResultTypeEnum)round.RoundResultType).ToString();
                    }
                    else
                    {
                        model.SecondParticipantResult = ((RoundResultTypeEnum)round.RoundResultType).ToString();
                    }
                }
                else
                {
                    if (round.RoundResultType != (int)RoundResultTypeEnum.DQ)
                    {
                        model.SecondParticipantResult = ((RoundResultTypeEnum)round.RoundResultType).ToString();
                    }
                    else
                    {
                        model.FirstParticipantResult = ((RoundResultTypeEnum)round.RoundResultType).ToString();
                    }
                }
            }

            return model;

        }

        private ParticipantSimpleModel GetParticipantModel(Participant participant)
        {
            return new ParticipantSimpleModel
            {
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                ParticipantId = participant.ParticipantId,
                DateOfBirth = participant.DateOfBirth
            };
        }

        private void UpdateBracketRoundsFromModel(IEnumerable<Round> rounds, IEnumerable<RoundModel> roundModels)
        {

            foreach (var round in rounds)
            {
                var roundModel = roundModels.First(m => m.RoundId == round.RoundId);
                if (roundModel.FirstParticipant != null)
                {
                    round.FirstParticipantId = roundModel.FirstParticipant.ParticipantId;
                }
                else
                {
                    round.FirstParticipantId = null;
                }
                if (roundModel.SecondParticipant != null)
                {
                    round.SecondParticipantId = roundModel.SecondParticipant.ParticipantId;
                }
                else
                {
                    round.SecondParticipantId = null;
                }
                _roundRepository.Update(round);
            }
        }

        private IQueryable<Participant> GetWinnersForBrackets(IQueryable<Bracket> brackets)
        {
            return brackets.SelectMany(b => b.Rounds.Where(r => r.Stage == 0 && r.RoundType == (int)RoundTypeEnum.Standard))
                .Select(r => r.WinnerParticipant);
        }

        private async Task<Bracket> GetBracketAsync(Guid weightDivisionId)
        {
            var bracket = await _bracketRepository.GetAll(b => b.WeightDivisionId == weightDivisionId)
                .Include(b => b.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.FirstParticipant)
                .Include(b => b.Rounds).ThenInclude(r => r.SecondParticipant)
                .FirstOrDefaultAsync();
            if (bracket == null)
            {
                bracket = new Bracket()
                {
                    BracketId = Guid.NewGuid(),
                    WeightDivisionId = weightDivisionId
                };
                _bracketRepository.Add(bracket);

                var participants = await GetOrderedListForNewBracketAsync(weightDivisionId);
                bracket.Rounds = CreateRoundStructure(participants.ToArray(), bracket.BracketId);
            }
            return bracket;

        }

        private string GetRoundResultDetailsJson(RoundResultModel model)
        {
            var jObject = new JObject(
                new JProperty(nameof(model.FirstParticipantAdvantages), model.FirstParticipantPoints),
                new JProperty(nameof(model.FirstParticipantAdvantages), model.FirstParticipantAdvantages),
                new JProperty(nameof(model.FirstParticipantPenalties), model.FirstParticipantPenalties),
                new JProperty(nameof(model.SecondParticipantPoints), model.SecondParticipantPoints),
                new JProperty(nameof(model.SecondParticipantAdvantages), model.SecondParticipantAdvantages),
                new JProperty(nameof(model.SecondParticipantPenalties), model.SecondParticipantPenalties),
                new JProperty(nameof(model.CompleteTime), model.CompleteTime),
                new JProperty(nameof(model.SubmissionType), model.SubmissionType)
            );
            return jObject.ToString();

        }

        private List<Round> GetStageRounds(IEnumerable<Round> stageRounds, int stage, Guid bracketId)
        {
            var childRounds = new List<Round>();
            if (stage == 0)
            {
                childRounds.Add(new Round()
                {
                    RoundId = Guid.NewGuid(),
                    BracketId = bracketId,
                    Stage = stage,
                    RoundType = (int)RoundTypeEnum.Standard
                });
                childRounds.Add(new Round()
                {
                    RoundId = Guid.NewGuid(),
                    BracketId = bracketId,
                    Stage = stage,
                    RoundType = (int)RoundTypeEnum.ThirdPlace
                });
            }
            else
            {
                foreach (var parentRound in stageRounds)
                {
                    if (parentRound.RoundType == (int)RoundTypeEnum.Standard)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            childRounds.Add(new Round()
                            {
                                RoundId = Guid.NewGuid(),
                                BracketId = bracketId,
                                NextRoundId = parentRound.RoundId,
                                Stage = stage
                            });
                        }
                    }
                }
            }
            return childRounds;
        }

        private ICollection<Round> CreateRoundStructure(Participant[] participants, Guid bracketId)
        {


            var rounds = new List<Round>();
            var lastStage = participants.Length == 3 ? 1 : (int)Math.Log(participants.Count(), 2) - 1;
            for (int i = 0; i <= lastStage; i++)
            {
                var roundsToAdd = GetStageRounds(rounds.Where(r => r.Stage == i - 1), i, bracketId);
                if (i == lastStage)
                {
                    if (participants.Length == 2)
                    {
                        roundsToAdd.Remove(roundsToAdd.First(r => r.RoundType == (int)RoundTypeEnum.ThirdPlace));
                    }
                    if (participants.Length == 3)
                    {
                        roundsToAdd[0].FirstParticipant = participants[0];
                        roundsToAdd[0].FirstParticipantId = participants[0].ParticipantId;
                        roundsToAdd[0].SecondParticipant = participants[1];
                        roundsToAdd[0].SecondParticipantId = participants[1].ParticipantId;
                        roundsToAdd[1].FirstParticipant = participants[2];
                        roundsToAdd[1].FirstParticipantId = participants[2].ParticipantId;
                        roundsToAdd[1].RoundType = (int)RoundTypeEnum.Buffer;

                    }
                    else
                    {
                        var j = 0;
                        foreach (var round in roundsToAdd)
                        {
                            if (participants[j].ParticipantId != Guid.Empty)
                            {
                                round.FirstParticipant = participants[j];
                                round.FirstParticipantId = participants[j].ParticipantId;
                            }

                            if (participants[j + 1].ParticipantId != Guid.Empty)
                            {
                                round.SecondParticipant = participants[j + 1];
                                round.SecondParticipantId = participants[j + 1].ParticipantId;
                            }

                            j = j + 2;
                        }
                    }
                }
                rounds.AddRange(roundsToAdd);
            }
            _roundRepository.AddRange(rounds);
            return rounds;
        }

        private Round AddParticipantToRound(Round round, Guid participantId)
        {
            if (round.FirstParticipantId == null)
            {
                round.FirstParticipantId = participantId;
            }
            else
            {
                round.SecondParticipantId = participantId;
            }
            return round;
        }

        #endregion
    }
}
