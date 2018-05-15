using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Medalist;
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
            if (bracket == null)
            {
                bracket = await CreateBracketAsync(weightDivisionId);
                _bracketRepository.Add(bracket);
            }

            return GetBracketModel(bracket);
        }

        public async Task<BracketModel> RunBracketAsync(Guid weightDivisionId)
        {
            var bracket = await GetBracketAsync(weightDivisionId);
            if (bracket == null)
            {
                bracket = await CreateBracketAsync(weightDivisionId);
                StartBracket(bracket);
                _bracketRepository.Add(bracket);
            }
            else
            {
                StartBracket(bracket);
                _bracketRepository.Update(bracket);
            }
            return GetBracketModel(bracket);
        }

        public async Task<bool> IsCategoryCompletedAsync(Guid categoryId)
        {
            return (await _categoryService.GetCategoryAsync(categoryId)).CompleteTs != null;
        }

        public async Task<List<ParticipantInAbsoluteDivisionModel>> GetParticipantsForAbsoluteDivisionAsync(Guid categoryId)
        {
            if ((await _categoryService.GetCategoryAsync(categoryId)).CompleteTs == null)
            {
                throw new Exception(
                    $"For Weight divisions for category with id {categoryId} not all rounds has winners");
            }

            var braketsForCategory = await _bracketRepository.GetAll(b => b.WeightDivision.CategoryId == categoryId && !b.WeightDivision.IsAbsolute)
                .Include(b => b.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.FirstParticipant).ThenInclude(p => p.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.SecondParticipant).ThenInclude(p => p.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant).ThenInclude(p => p.WeightDivision)
                .ToListAsync();

            var winnerParticipants =
                GetWinnersForBrackets(braketsForCategory)
                    .Select(p => new ParticipantInAbsoluteDivisionModel()
                    {
                        ParticipantId = p.ParticipantId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        TeamName = "",
                        WeightDivisionName = p.WeightDivision.Name,
                        IsSelectedIntoDivision = p.AbsoluteWeightDivisionId != null
                    }).ToList();
            return winnerParticipants;
        }

        public async Task UpdateBracket(BracketModel model)
        {

            var bracket = await _bracketRepository.GetAll(b => b.BracketId == model.BracketId).Include(b => b.Rounds)
                .FirstOrDefaultAsync();
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
            var weightDivisions = await _weightDivisionService.GetWeightDivisionModelsByCategoryIdAsync(categoryId);
            var result = new Dictionary<string, BracketModel>();
            foreach (var division in weightDivisions)
            {
                result.Add(division.WeightDivisionId.ToString(), await GetBracketModelAsync(division.WeightDivisionId));
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
            var round = await _roundRepository.GetAll(r => r.RoundId == model.RoundId).Include(r => r.NextRound)
                .FirstOrDefaultAsync();
            if (round != null)
            {
                round.WinnerParticipantId = model.WinnerParticipantId;
                round.RoundResultType = (int)model.RoundResultType;
                round.RoundResultDetails = GetRoundResultDetailsJson(model);
                if (round.Stage == 1)
                {
                    var lostParticipantId = round.WinnerParticipantId == round.FirstParticipantId
                        ? round.SecondParticipantId
                        : round.FirstParticipantId;
                    var bufferRound = await _roundRepository.FirstOrDefaultAsync(r =>
                        r.BracketId == round.BracketId && r.RoundType == (int)RoundTypeEnum.Buffer);
                    if (bufferRound != null && bufferRound.SecondParticipantId == null)
                    {
                        bufferRound.SecondParticipantId = lostParticipantId;
                    }
                    else
                    {
                        var thirdPlaceRound = await _roundRepository.FirstOrDefaultAsync(r =>
                            r.BracketId == round.BracketId && r.RoundType == (int)RoundTypeEnum.ThirdPlace);
                        if (thirdPlaceRound != null)
                        {
                            if (round.Order % 2 == 0)
                            {
                                thirdPlaceRound.FirstParticipantId = lostParticipantId;
                            }
                            else
                            {
                                thirdPlaceRound.SecondParticipantId = lostParticipantId;
                            }

                            //if buffer exists = 3 participants
                            if (bufferRound != null)
                            {
                                thirdPlaceRound.WinnerParticipantId = lostParticipantId;
                            }

                            _roundRepository.Update(thirdPlaceRound);
                        }
                    }
                }

                if (round.NextRound != null)
                {
                    if (round.Order % 2 == 0)
                    {
                        round.NextRound.FirstParticipantId = round.WinnerParticipantId;
                    }
                    else
                    {
                        round.NextRound.SecondParticipantId = round.WinnerParticipantId;
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

                        var bracket = await _bracketRepository
                            .GetAll(b => b.BracketId == round.BracketId).Include(b => b.WeightDivision)
                            .FirstOrDefaultAsync();
                        bracket.CompleteTs = DateTime.UtcNow;
                        _bracketRepository.Update(bracket);
                        //find if other brackets are complete
                        if (!(await _weightDivisionService.GetWeightDivisionsByCategoryIdAsync(bracket.WeightDivision
                            .CategoryId)).SelectMany(wd => wd.Brackets).Any(b => b.CompleteTs == null && b.BracketId != bracket.BracketId))
                        {
                            await _categoryService.SetCategoryCompleteAsync(bracket.WeightDivision.CategoryId);
                        }
                    }
                }

                _roundRepository.Update(round);
            }
        }

        public async Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds)
        {
            var brackets = await _bracketRepository.GetAll(b => categoryIds.Contains(b.WeightDivision.CategoryId) && b.CompleteTs != null)
                    .Include(b => b.Rounds).ThenInclude(r => r.FirstParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.SecondParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant).ThenInclude(t => t.Team)
                    .ToListAsync()
                ;
            var categoryMedalists = new List<MedalistProcessingModel>();
            foreach (var bracket in brackets)
            {
                var bracketMedalists = GetMedalistsForBracket(bracket);
                if (bracketMedalists.Count == 1 ||
                         bracketMedalists.Count == 2 &&
                          bracketMedalists.GroupBy(m => m.Participant.TeamId).Any(g => g.Count() == 1))
                {
                }
                else
                {
                    categoryMedalists.AddRange(bracketMedalists);
                }
            }

            return categoryMedalists.GroupBy(m => m.Participant.TeamId)
                 .Select(g => new TeamResultModel { TeamId = g.Key, Points = g.Sum(gg => gg.Points), TeamName = g.First().Participant.Team.Name });
        }


        public async Task<CustomFile> GetPersonalResultsFileByCategoriesAsync(
            IEnumerable<Guid> categoryIds)
        {
            var brackets = await _bracketRepository
                    .GetAll(b => categoryIds.Contains(b.WeightDivision.CategoryId) && b.CompleteTs != null)
                    .Include(b => b.Rounds).ThenInclude(r => r.FirstParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.SecondParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.WeightDivision).ThenInclude(w => w.Category)
                    .ToListAsync()
                ;
            var categoryWeightDivisioMedalistGroups = new List<CategoryWeightDivisionMedalistGroup>();
            foreach (var categoryId in categoryIds)
            {
                var categoryBrackets = brackets.Where(b => b.WeightDivision.CategoryId == categoryId);
                var categoryWeightDivisioMedalistGroup = new CategoryWeightDivisionMedalistGroup()
                {
                    CategoryName = categoryBrackets.First().WeightDivision.Category.Name,
                    WeightDivisionMedalistGroups = new List<WeightDivisionMedalistGroup>()
                };
                var weightDivisions = categoryBrackets.Select(b => b.WeightDivision);
                foreach (var weightDivision in weightDivisions)
                {
                    var bracket = categoryBrackets.First(b => b.WeightDivisionId == weightDivision.WeightDivisionId);
                    categoryWeightDivisioMedalistGroup.WeightDivisionMedalistGroups.Add(new WeightDivisionMedalistGroup()
                    {
                        WeightDivisionName = bracket.WeightDivision.Name,
                        Medalists = GetMedalistsForBracket(bracket)

                    });
                }
                categoryWeightDivisioMedalistGroups.Add(categoryWeightDivisioMedalistGroup);
            }
            return _bracketsFileService.GetPersonalResultsFileAsync(categoryWeightDivisioMedalistGroups);
        }

        #endregion

        #region Private methods

        private async Task<List<Participant>> GetOrderedListForNewBracketAsync(Guid weightDivisionId)
        {
            var participants =
                (await _participantService.GetParticipantsByWeightDivisionAsync(weightDivisionId)).ToList();
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

        private int GetBracketsSize(int participantCount)
        {
            if (participantCount == 3)
            {
                return 3;
            }

            for (var i = 1; i <= Math.Log(FightersMaxCount, 2); i++)
            {
                var size = Math.Pow(2, i);
                if (size >= participantCount)
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
                    var participant = orderedbyTeam.ElementAtOrDefault(i);
                    if (i % 2 == 0)
                    {
                        sideA.Add(participant);
                    }
                    else
                    {
                        sideB.Add(participant);
                    }
                }

                return DistributeParticipants(sideA).Concat(DistributeParticipants(sideB)).ToList();
            }

            return participantList;
        }

        private BracketModel GetBracketModel(Bracket bracket)
        {
            var model = new BracketModel
            {
                BracketId = bracket.BracketId,
                Title = bracket.Title,
                Medalists = GetMedalistsForBracket(bracket).Select(m => new MedalistModel() { Participant = GetParticipantModel(m.Participant), Place = m.Place }).OrderBy(m => m.Place).ToList(),
                RoundModels = new List<RoundModel>()
            };
            foreach (var round in bracket.Rounds)
            {
                model.RoundModels.Add(GetRoundModel(round, bracket.RoundTime));
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
                Order = round.Order,
            };
            if (round.WinnerParticipantId != null)
            {
                model.WinnerParticipant = GetParticipantModel(round.WinnerParticipant);
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

        private List<Participant> GetWinnersForBrackets(IEnumerable<Bracket> brackets)
        {
            return brackets.SelectMany(b =>
                    GetMedalistsForBracket(b))
                .Select(r => r.Participant).ToList();
        }

        private async Task<Bracket> GetBracketAsync(Guid weightDivisionId)
        {
            var bracket = await _bracketRepository.GetAll(b => b.WeightDivisionId == weightDivisionId)
                .Include(b => b.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.FirstParticipant)
                .Include(b => b.Rounds).ThenInclude(r => r.SecondParticipant)
                .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant)
                .FirstOrDefaultAsync();
            return bracket;
        }

        private async Task<Bracket> CreateBracketAsync(Guid weightDivisionId)
        {
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId);
            var category = await _categoryService.GetCategoryAsync(weightDivision.CategoryId);
            var bracket = new Bracket()
            {
                BracketId = Guid.NewGuid(),
                WeightDivisionId = weightDivisionId,
                RoundTime = category.RoundTime,
                Title = GetBracketTtitle(category, weightDivision)
            };
            var participants = await GetOrderedListForNewBracketAsync(weightDivisionId);
            bracket.Rounds = CreateRoundStructure(participants.ToArray(), bracket.BracketId);
            return bracket;
        }

        private string GetBracketTtitle(Category category, WeightDivision weightDivision)
        {
            return $"{category.Name} / {weightDivision.Name}";
        }

        private string GetRoundResultDetailsJson(RoundResultModel model)
        {
            var jObject = new JObject(
                new JProperty(nameof(model.FirstParticipantPoints), model.FirstParticipantPoints),
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

        private List<Round> GetStageRoundsAsync(IEnumerable<Round> stageRounds, int stage, Guid bracketId)
        {
            var childRounds = new List<Round>();
            if (stage == 0)
            {
                childRounds.Add(new Round()
                {
                    RoundId = Guid.NewGuid(),
                    BracketId = bracketId,
                    Stage = stage,
                    RoundType = (int)RoundTypeEnum.Standard,
                    Order = 0
                });
                childRounds.Add(new Round()
                {
                    RoundId = Guid.NewGuid(),
                    BracketId = bracketId,
                    Stage = stage,
                    RoundType = (int)RoundTypeEnum.ThirdPlace,
                    Order = 1
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
                                NextRound = parentRound,
                                Stage = stage,
                                Order = (2 * parentRound.Order) + i
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
                var roundsToAdd = GetStageRoundsAsync(rounds.Where(r => r.Stage == i - 1), i, bracketId);
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
            return rounds;
        }

        private void StartBracket(Bracket bracket)
        {
            if (bracket.StartTs == null)
            {
                bracket.StartTs = DateTime.UtcNow;
                var firstRounds = bracket.Rounds.Where(r =>
                    r.Stage == bracket.Rounds.Max(rr => rr.Stage) && r.RoundType != (int)RoundTypeEnum.Buffer);
                foreach (var round in firstRounds)
                {
                    if (round.FirstParticipantId == null || round.SecondParticipantId == null)
                    {
                        if (round.FirstParticipantId == null)
                        {
                            round.WinnerParticipantId = round.SecondParticipantId;
                        }

                        if (round.SecondParticipantId == null)
                        {
                            round.WinnerParticipantId = round.FirstParticipantId;
                        }

                        if (round.NextRound != null)
                        {
                            if (round.Order % 2 == 0)
                            {
                                round.NextRound.FirstParticipantId = round.WinnerParticipantId;
                            }
                            else
                            {
                                round.NextRound.SecondParticipantId = round.WinnerParticipantId;
                            }
                        }
                    }
                }
            }
        }

        private List<MedalistProcessingModel> GetMedalistsForBracket(Bracket bracket)
        {
            var bracketMedalists = new List<MedalistProcessingModel>();
            if (bracket.CompleteTs != null)
            {
                var finals = bracket.Rounds.Where(r => r.Stage == 0);
                foreach (var round in finals)
                {

                    if (round.WinnerParticipant != null)
                    {
                        if (round.RoundType == (int)RoundTypeEnum.ThirdPlace)
                        {
                            bracketMedalists.Add(new MedalistProcessingModel()
                            {
                                Participant = round.WinnerParticipant,
                                Place = 3,
                                Points = 1
                            });
                        }
                        else
                        {
                            bracketMedalists.Add(new MedalistProcessingModel()
                            {
                                Participant = round.WinnerParticipant,
                                Place = 1,
                                Points = 9
                            });
                            if (round.SecondParticipant != null && round.FirstParticipant != null)
                            {
                                bracketMedalists.Add(
                                    round.FirstParticipantId == round.WinnerParticipantId
                                        ? new MedalistProcessingModel()
                                        {
                                            Participant = round.SecondParticipant,
                                            Place = 2,
                                            Points = 3
                                        }
                                        : new MedalistProcessingModel()
                                        {
                                            Participant = round.FirstParticipant,
                                            Place = 2,
                                            Points = 3
                                        });
                            }
                        }

                    }
                }
            }
            return bracketMedalists;
        }



        #endregion
    }
}

