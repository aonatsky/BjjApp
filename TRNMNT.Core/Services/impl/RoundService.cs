using System;
using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Enum;
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
            
            
            var rounds = new List<Round>();
            var lastStage = participants.Length == 3 ? 1 : (int)Math.Log(participants.Count(), 2) - 1;
            for (int i = 0; i <= lastStage; i++)
            {
                var roundsToAdd = GetStageRounds(rounds.Where(r => r.Stage == i - 1), i, bracketId);
                if (i == lastStage)
                {
                    if (participants.Length == 2)
                    {
                        roundsToAdd.Remove(roundsToAdd.First(r => r.RoundType == (int) RoundTypeEnum.ThirdPlace));
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

        public void UpdateRound(Round round)
        {
            _roundRepository.Update(round);
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


    }
}
