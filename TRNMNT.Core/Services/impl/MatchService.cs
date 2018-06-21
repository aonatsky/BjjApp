using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> _matchRepository;
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly ICategoryService _categoryService;
        private readonly IRepository<WeightDivision> _weightDivisionRepository;
        private readonly IRepository<Participant> _participantRepository;

        public MatchService(
            IRepository<Match> matchRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<Participant> participantRepository,
            IWeightDivisionService weightDivisionService,
            ICategoryService categoryService)
        {
            _matchRepository = matchRepository;
            _weightDivisionService = weightDivisionService;
            _categoryService= categoryService;
            _participantRepository = participantRepository;
        }

        public async Task<List<Match>> GetMatchesAsync(Guid categoryId, Guid weightDivisionId)
        {
            var matches = await _matchRepository.GetAll(m => m.CategoryId == categoryId && m.WeightDivisionId == weightDivisionId).ToListAsync();
            if (!matches.Any())
            {
                var participants = await _participantRepository.GetAll(p => p.WeightDivisionId == weightDivisionId && p.CategoryId == categoryId && p.IsActive).ToListAsync();
                var orderedParticapants = GetParticipantsForBracket(participants);
                matches = CreateMatches(orderedParticapants, weightDivisionId, categoryId);
                _matchRepository.AddRange(matches);
                return matches;
            }
            return matches;
        }

        public async Task UpdateMatchesParticipantsAsync(List<MatchModel> matchModels)
        {
            var matchIds = matchModels.Select(mm => mm.MatchId);
            var matchesToUpdate = await _matchRepository.GetAll(m => matchIds.Contains(m.MatchId)).ToListAsync();
            foreach (var match in matchesToUpdate)
            {
                var model = matchModels.First(mm => mm.MatchId == match.MatchId);
                match.AParticipantId = model.AParticipant?.ParticipantId;
                match.BParticipantId = model.BParticipant?.ParticipantId;
                _matchRepository.Update(match);
            }
        }

        public async Task<List<Match>> GetProcessedMatchesAsync(Guid categoryId, Guid weightDivisionId)
        {
            var matches = await GetMatchesAsync(categoryId, weightDivisionId);
            var maxRound = matches.Max(m => m.Round);
            foreach (var matchToProcess in matches.Where(m => m.Round == maxRound))
            {
                if (matchToProcess.AParticipantId == null)
                {
                    SetMatchNoContest(matchToProcess, matchToProcess.BParticipantId.Value, matches);
                }
                else if (matchToProcess.BParticipantId == null)
                {
                    SetMatchNoContest(matchToProcess, matchToProcess.AParticipantId.Value, matches);
                }
                _matchRepository.Update(matchToProcess);
            }

            void SetMatchNoContest(Match matchToProcess, Guid winnerId, List<Match> allMatches)
            {
                matchToProcess.WinnerParticipantId = winnerId;
                matchToProcess.MatchResultType = (int)MatchResultTypeEnum.NC;
                if (matchToProcess.NextMatchId != null)
                {
                    var nextMatch = matches.First(m => m.MatchId == matchToProcess.NextMatchId);
                    if (matchToProcess.Order % 2 == 0)
                    {
                        nextMatch.AParticipantId = winnerId;
                    }
                    else
                    {
                        nextMatch.BParticipantId = winnerId;
                    };
                    _matchRepository.Update(nextMatch);
                }
            }
            return matches;
        }


        public async Task SetMatchResultAsync(MatchResultModel model)
        {
            var match = await _matchRepository.GetAll(m => m.MatchId == model.MatchId).Include(m => m.NextMatch)
                .FirstOrDefaultAsync();
            if (match != null)
            {
                match.WinnerParticipantId = model.WinnerParticipantId;
                match.MatchResultType = (int)model.RoundResultType;
                match.MatchResultDetails = GetRoundResultDetailsJson(model);
                if (match.Round == 1)
                {
                    var lostParticipantId = match.WinnerParticipantId == match.AParticipantId
                        ? match.BParticipantId
                        : match.AParticipantId;
                    var bufferMatch = await _matchRepository.FirstOrDefaultAsync(m =>
                        m.WeightDivisionId == match.WeightDivisionId
                        && m.CategoryId == match.CategoryId
                        && m.MatchType == (int)MatchTypeEnum.Buffer);
                    if (bufferMatch != null && bufferMatch.BParticipantId == null)
                    {
                        bufferMatch.BParticipantId = lostParticipantId;
                        _matchRepository.Update(bufferMatch);
                    }
                    else
                    {
                        var thirdPlaceMatch = await _matchRepository.FirstOrDefaultAsync(m =>
                            m.WeightDivisionId == match.WeightDivisionId
                            && m.CategoryId == match.CategoryId
                            && m.MatchType == (int)MatchTypeEnum.ThirdPlace);
                        if (thirdPlaceMatch != null)
                        {
                            if (match.Order % 2 == 0)
                            {
                                thirdPlaceMatch.AParticipantId = lostParticipantId;
                            }
                            else
                            {
                                thirdPlaceMatch.BParticipantId = lostParticipantId;
                            }

                            //if buffer exists = 3 participants
                            if (bufferMatch != null)
                            {
                                thirdPlaceMatch.WinnerParticipantId = lostParticipantId;
                            }

                            _matchRepository.Update(thirdPlaceMatch);
                        }
                    }
                }

                if (match.NextMatch != null)
                {
                    if (match.Order % 2 == 0)
                    {
                        match.NextMatch.AParticipantId = match.WinnerParticipantId;
                    }
                    else
                    {
                        match.NextMatch.BParticipantId = match.WinnerParticipantId;
                    }
                }

                if (match.Round == 0)
                {
                    if (!await _matchRepository.GetAll(m => m.WeightDivisionId == match.WeightDivisionId
                                                            && m.CategoryId == match.CategoryId
                                                            && m.Round == 0
                                                            && m.MatchId != match.MatchId
                                                            && m.WinnerParticipantId == null
                    ).AnyAsync())
                    {
                        await _weightDivisionService.SetWeightDivisionCompletedAsync(match.WeightDivisionId);
                    }
                }
                _matchRepository.Update(match);
            }
        }

        private string GetRoundResultDetailsJson(MatchResultModel model)
        {
            var jObject = new JObject(
                new JProperty(nameof(model.AParticipantPoints), model.AParticipantPoints),
                new JProperty(nameof(model.AParticipantAdvantages), model.AParticipantAdvantages),
                new JProperty(nameof(model.AParticipantPenalties), model.AParticipantPenalties),
                new JProperty(nameof(model.BParticipantPoints), model.BParticipantPoints),
                new JProperty(nameof(model.BParticipantAdvantages), model.BParticipantAdvantages),
                new JProperty(nameof(model.BParticipantPenalties), model.BParticipantPenalties),
                new JProperty(nameof(model.CompleteTime), model.CompleteTime),
                new JProperty(nameof(model.SubmissionType), model.SubmissionType)
            );
            return jObject.ToString();

        }



        #region private methods


        private List<Match> CreateMatches(List<Participant> orderedParticipants, Guid weightDivisionId, Guid categoryId)
        {
            var maxRound = orderedParticipants.Count == 3
                ? 1
                : (int)Math.Log(orderedParticipants.Count(), 2) - 1;

            var matches = new List<Match>
            {
                GetMatch(weightDivisionId, categoryId, 0, 0, null)
            };

            if (orderedParticipants.Count() > 2)
            {
                matches.Add(GetMatch(weightDivisionId, categoryId, 0, 1, null, (int)MatchTypeEnum.ThirdPlace));
            }

            for (int i = 1; i <= maxRound; i++)
            {
                var roundMatches = GetRoundMatches(matches.Where(r => r.Round == i - 1 && r.MatchType == (int)MatchTypeEnum.Standard), i);
                if (i == maxRound)
                {
                    if (orderedParticipants.Count() == 3)
                    {
                        roundMatches[0].AParticipant = orderedParticipants[0];
                        roundMatches[0].AParticipantId = orderedParticipants[0].ParticipantId;
                        roundMatches[0].BParticipant = orderedParticipants[1];
                        roundMatches[0].BParticipantId = orderedParticipants[1].ParticipantId;
                        roundMatches[1].AParticipant = orderedParticipants[2];
                        roundMatches[1].AParticipantId = orderedParticipants[2].ParticipantId;
                        roundMatches[1].MatchType = (int)MatchTypeEnum.Buffer;
                    }
                    else
                    {
                        var j = 0;
                        foreach (var match in roundMatches)
                        {
                            if (orderedParticipants[j] != null)
                            {
                                match.AParticipant = orderedParticipants[j];
                                match.AParticipantId = orderedParticipants[j].ParticipantId;
                            }

                            if (orderedParticipants[j + 1] != null)
                            {
                                match.BParticipant = orderedParticipants[j + 1];
                                match.BParticipantId = orderedParticipants[j + 1].ParticipantId;
                            }

                            j = j + 2;
                        }
                    }
                }

                matches.AddRange(roundMatches);
            }
            return matches;

        }

        private List<Match> GetRoundMatches(IEnumerable<Match> parentMatches, int round)
        {
            var matchesForRound = new List<Match>();
            foreach (var parentMatch in parentMatches)
            {
                for (int i = 0; i < 2; i++)
                {
                    matchesForRound.Add(GetMatch(parentMatch.WeightDivisionId, parentMatch.CategoryId, round, 2 * parentMatch.Order + i, parentMatch.MatchId));
                }
            }
            return matchesForRound;
        }

        private const int ParticipantsMaxCount = 64;

        private int GetBracketSize(int participantCount)
        {
            if (participantCount == 3)
            {
                return 3;
            }

            for (var i = 1; i <= Math.Log(ParticipantsMaxCount, 2); i++)
            {
                var size = Math.Pow(2, i);
                if (size >= participantCount)
                {
                    return (int)size;
                }
            }

            return 2;

        }

        private List<Participant> GetParticipantsForBracket(List<Participant> participants)
        {
            var bracketSize = GetBracketSize(participants.Count);
            for (var i = 0; i < bracketSize - participants.Count; i++)
            {
                participants.Add(null);
            }
            return DistributeParticipants(participants);
        }

        private List<Participant> DistributeParticipants(List<Participant> participantList)
        {

            var orderedbyTeam = participantList.GroupBy(f => f?.TeamId).OrderByDescending(g => g.Count()).SelectMany(f => f).ToList();
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

        private Match GetMatch(Guid weightDivisionId, Guid categoryId, int round, int order, Guid? nextMatchId, int matchType = (int)MatchTypeEnum.Standard)
        {
            return new Match
            {
                MatchId = Guid.NewGuid(),
                Round = round,
                CategoryId = categoryId,
                WeightDivisionId = weightDivisionId,
                MatchType = matchType,
                Order = order,
                NextMatchId = nextMatchId
            };
        }

        #endregion
    }
}
