using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
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
        private readonly IRoundService _roundService;
        private readonly IParticipantService _participantService;
        private readonly BracketsFileService _bracketsFileService;
        private readonly IWeightDivisionService _weightDivisionService;

        #endregion

        #region .ctor

        public BracketService(IRepository<Bracket> bracketRepository, IRoundService roundService, 
            IParticipantService participantService, 
            BracketsFileService bracketsFileService,
            IWeightDivisionService weightDivisionService)
        {
            _bracketRepository = bracketRepository;
            _roundService = roundService;
            _participantService = participantService;
            _bracketsFileService = bracketsFileService;
            _weightDivisionService = weightDivisionService;
        }

        #endregion

        #region Public Methods

        public async Task<BracketModel> GetBracketAsync(Guid weightDivisionId)
        {
            var bracket = await _bracketRepository.GetAll(b => b.WeightDivisionId == weightDivisionId)
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
                bracket.Rounds = _roundService.CreateRoundStructure(participants.ToArray(), bracket.BracketId);
            }
            return GetBracketModel(bracket);
        }

        public async Task<bool> IsWinnersSelectedForAllRoundsAsync(Guid categoryId)
        {
            var braketsForCategory = _bracketRepository.GetAll(b => b.WeightDivision.CategoryId == categoryId);
            if (!await braketsForCategory.AnyAsync())
            {
                return false;
            }
            if (!await braketsForCategory.AnyAsync(b =>
                b.Rounds.Any(r => r.Stage == 0 && r.RoundType == (int) RoundTypeEnum.Standard)))
            {
                return false;
            }
            return !await GetWinnersForBrackets(braketsForCategory).AnyAsync(p => p == null);
        }

        public async Task<List<ParticipantSmallTableMobel>> GetWinnersAsync(Guid categoryId)
        {
            if (!await IsWinnersSelectedForAllRoundsAsync(categoryId))
            {
                throw new Exception($"For Weight divisions for category with id {categoryId} not all rounds has winners");
            }

            var winnerParticipants =
                await GetWinnersForBrackets(_bracketRepository.GetAll(b => b.WeightDivision.CategoryId == categoryId)
                        .Include(p => p.Rounds)
                        .ThenInclude(r => r.WinnerParticipant).ThenInclude(p => p.Team)
                        .Include(p => p.Rounds)
                        .ThenInclude(r => r.WinnerParticipant).ThenInclude(p => p.WeightDivision))
                    .Select(p => new ParticipantSmallTableMobel()
                    {
                        ParticipantId = p.ParticipantId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        TeamName = p.Team.Name,
                        WeightDivisionName = p.WeightDivision.Name
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
                result.Add(division.WeightDivisionId.ToUpperInvariant(), await GetBracketAsync(new Guid(division.WeightDivisionId)));
            }
            return result;
        }

        #region private methods
        private async Task<List<Participant>> GetOrderedListForNewBracketAsync(Guid weightDivisionId)
        {
            var participants = (await _participantService.GetParticipantsByWeightDivisionAsync(weightDivisionId)).ToList();
            var count = participants.Count();
            var bracketSize = GetBracketsSize(count);
            for (var i = 0; i < bracketSize - count; i++)
            {
                participants.Add(_participantService.GetEmptyParticipant());
            }

            return Distribute(participants);
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

        private List<Participant> Distribute(List<Participant> participantList)
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
                return Distribute(sideA).Concat(Distribute(sideB)).ToList();
            }
            return participantList;
        }

        private BracketModel GetBracketModel(Bracket bracket)
        {

            var model = new BracketModel
            {
                BracketId = bracket.BracketId,
                RoundModels = new List<RoundModel>()
            };
            foreach (var round in bracket.Rounds)
            {
                model.RoundModels.Add(GetRoundModel(round));
            }
            return model;
        }

        private RoundModel GetRoundModel(Round round)
        {
            return new RoundModel()
            {
                RoundId = round.RoundId,
                NextRoundId = round.NextRoundId,
                Stage = round.Stage,
                FirstParticipant = round.FirstParticipant == null ? null : GetParticipantModel(round.FirstParticipant),
                SecondParticipant = round.SecondParticipant == null ? null : GetParticipantModel(round.SecondParticipant),
                RoundType = round.RoundType
            };
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
                _roundService.UpdateRound(round);
            }
        }

        private IQueryable<Participant> GetWinnersForBrackets(IQueryable<Bracket> brackets)
        {
            return brackets.SelectMany(b => b.Rounds.Where(r => r.Stage == 0 && r.RoundType == (int) RoundTypeEnum.Standard))
                .Select(r => r.WinnerParticipant);
        }

        #endregion
    }
}
