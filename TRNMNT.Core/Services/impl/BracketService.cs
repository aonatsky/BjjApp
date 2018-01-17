using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Round;
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

        #endregion

        #region .ctor
        public BracketService(IRepository<Bracket> bracketRepository, IRoundService roundService, IParticipantService participantService)
        {
            _bracketRepository = bracketRepository;
            _roundService = roundService;
            _participantService = participantService;
        }
        #endregion

        public async Task<BracketModel> GetBracketAsync(Guid weightDivisionId)
        {
            var  bracket = await _bracketRepository.GetAll(b => b.WeightDivisionId == weightDivisionId)
                .Include(b=>b.Rounds).ThenInclude(r=>r.FirstParticipant)
                .Include(b=>b.Rounds).ThenInclude(r=>r.SecondParticipant)
                .FirstOrDefaultAsync();
            if (bracket == null)
            {
                bracket = new Bracket()
                {
                    BracketId = Guid.NewGuid(),
                    WeightDivisionId = weightDivisionId
                };
                _bracketRepository.Add(bracket);

                var participants = await GetOrderedListForBracketsAsync(weightDivisionId);
                bracket.Rounds = _roundService.CreateRoundStructure(participants.ToArray(), bracket.BracketId);  
            }
            return GetBracketModel(bracket);
        }

       
        public Task UpdateBracket(BracketModel model)
        {
            throw new NotImplementedException();
        }



        #region private methods
        private async Task<List<Participant>> GetOrderedListForBracketsAsync(Guid weightDivisionId)
        {
            var participants = (await _participantService.GetParticipantsByWeightDivisionAsync(weightDivisionId)).ToList();
            var count = participants.Count();
            var bracketSize = GetBracketsSize(participants.Count());
            for (var i = 0; i < bracketSize - count; i++)
            {
                participants.Add(_participantService.GetEmptyParticipant());
            }

            return Distribute(participants);
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
                SecondParticipant = round.SecondParticipant == null ? null :  GetParticipantModel(round.SecondParticipant)
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

        #endregion
    }
}
