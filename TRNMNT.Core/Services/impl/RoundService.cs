using System;
using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class RoundService: IRoundService
    {
        private readonly IRepository<Round> _roundRepository;

        public RoundService(IRepository<Round> roundRepository)
        {
            _roundRepository = roundRepository;
        }
        
        public ICollection<Round> CreateRoundStructure(Participant[] participants, Guid bracketId)
        {
            var rounds = new List<Round>();
            var lastStage = (int)Math.Log(participants.Count(), 2)-1;

            rounds.Add(new Round()
            {
                RoundId = Guid.NewGuid(),
                Stage = 0,
                BracketId =  bracketId
            });
            for (int i = 1; i <= lastStage; i++)
            {
                var roundsToAdd = GetStageRounds(rounds.Where(r => r.Stage == i-1),i);
                if (i == lastStage)
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
                rounds.AddRange(roundsToAdd);
            }
            _roundRepository.AddRange(rounds);
            return rounds;
        }

        public void UpdateRound(Round round)
        {
            _roundRepository.Update(round);
        }

        private IEnumerable<Round> GetStageRounds(IEnumerable<Round> parentRounds, int stage)
        {
            var childRounds = new List<Round>();
            foreach (var parentRound in parentRounds)
            {
                for (int i = 0; i < 2; i++)
                {
                    childRounds.Add(new Round()
                    {
                        RoundId = Guid.NewGuid(),
                        BracketId = parentRound.BracketId,
                        NextRoundId = parentRound.RoundId,
                        Stage = stage
                    });
                }
            }
            return childRounds;

        }

       
    }
}
