using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Impl;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class BracketService : IBracketService
    {
        #region dependencies
        private readonly IRepository<Bracket> _bracketRepository;
        private readonly IRoundService _roundService;
        private readonly IParticipantService _participantService;
        private readonly BracketsFileService _bracketsFileService;

        #endregion

        #region .ctor
        public BracketService(IRepository<Bracket> bracketRepository, IRoundService roundService, IParticipantService participantService, BracketsFileService bracketsFileService)
        {
            _bracketRepository = bracketRepository;
            _roundService = roundService;
            _participantService = participantService;
            _bracketsFileService = bracketsFileService;
        }
        #endregion

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


        public async Task UpdateBracket(BracketModel model)
        {

            var bracket = await _bracketRepository.GetAll(b => b.BracketId == model.BracketId).Include(b=>b.Rounds).FirstOrDefaultAsync();
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

            var model = new BracketModel();
            model.BracketId = bracket.BracketId;
            model.RoundModels = new List<RoundModel>();
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


        #endregion
    }
}
