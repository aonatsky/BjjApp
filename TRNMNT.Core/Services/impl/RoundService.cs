using System;
using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class RoundService : IRoundService
    {
        private readonly IRepository<Round> _roundRepository;

        public RoundService(IRepository<Round> roundRepository)
        {
            _roundRepository = roundRepository;
        }

        public ICollection<Round> CreateRoundStructure(Participant[] participants, Guid bracketId)
        {
            var is3Participants = participants.Length == 3;
            var rounds = new List<Round>();
            var lastStage = is3Participants ? 1 : (int)Math.Log(participants.Count(), 2) - 1;
            for (int i = 0; i <= lastStage; i++)
            {
                var roundsToAdd = GetStageRounds(rounds.Where(r => r.Stage == i - 1), i, bracketId);
                if (i == lastStage)
                {
                    if (is3Participants)
                    {
                        roundsToAdd.ToArray()[0].FirstParticipant = participants[0];
                        roundsToAdd.ToArray()[0].FirstParticipantId = participants[0].ParticipantId;
                        roundsToAdd.ToArray()[0].SecondParticipant = participants[1];
                        roundsToAdd.ToArray()[0].SecondParticipantId = participants[1].ParticipantId;
                        roundsToAdd.ToArray()[1].FirstParticipant = participants[2];
                        roundsToAdd.ToArray()[1].FirstParticipantId = participants[2].ParticipantId;
                        roundsToAdd.ToArray()[1].HasBooferParticipant = true;

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

        public void UpdateRound(Round round)
        {
            _roundRepository.Update(round);
        }

        private IEnumerable<Round> GetStageRounds(IEnumerable<Round> parentRounds, int stage, Guid bracketId)
        {
            var childRounds = new List<Round>();
            if (!parentRounds.Any())
            {
                childRounds.Add(new Round()
                {
                    RoundId = Guid.NewGuid(),
                    BracketId = bracketId,
                    Stage = stage
                });
            }
            else
            {
                foreach (var parentRound in parentRounds)
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
            return childRounds;

        }


    }
}
