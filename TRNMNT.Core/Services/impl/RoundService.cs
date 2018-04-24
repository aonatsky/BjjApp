using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class RoundService : IRoundService
    {
        #region Dependencies
        private readonly IRepository<Round> _roundRepository;
        #endregion

        #region .ctor
        public RoundService(IRepository<Round> roundRepository)
        {
            _roundRepository = roundRepository;
        }

        #endregion
        #region Public Methods
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

        public void UpdateRound(Round round)
        {
            _roundRepository.Update(round);
        }

        public async Task SetRoundResultAsync(RoundResultModel model)
        {
            var round = await _roundRepository.GetAllIncluding(r => r.RoundId == model.RoundId, r=> r.NextRound).FirstOrDefaultAsync();
            if (round != null)
            {
                round.WinnerParticipantId = model.WinnerParticipantId;
                round.RoundResultType = model.RundResultType;
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
                        r.BracketId == round.BracketId || r.RoundType == (int) RoundTypeEnum.ThirdPlace);
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
                _roundRepository.Update(round);
            }
        }



        #endregion

        #region PrivateMethods
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
        #endregion


    }
}
