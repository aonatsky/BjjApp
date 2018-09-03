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
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> _matchRepository;
        private readonly IWeightDivisionService _weightDivisionService;

        public MatchService(
            IRepository<Match> matchRepository,
            IWeightDivisionService weightDivisionService
        )
        {
            _matchRepository = matchRepository;
            _weightDivisionService = weightDivisionService;
        }

        public async Task<List<Match>> GetMatchesAsync(Guid categoryId, Guid weightDivisionId)
        {
            var matches = await _matchRepository.GetAll(
                    m => m.CategoryId == categoryId && m.WeightDivisionId == weightDivisionId)
                .Include(m => m.AParticipant).ThenInclude(p => p.Team)
                .Include(m => m.BParticipant).ThenInclude(p => p.Team)
                .Include(m => m.WinnerParticipant).ThenInclude(p => p.Team)
                .ToListAsync();
            return matches;
        }

        public List<Match> CreateMatchesAsync(Guid categoryId, Guid weightDivisionId, List<Participant> orderedParticapants)
        {
            var matches = CreateMatches(orderedParticapants, weightDivisionId, categoryId);
            _matchRepository.AddRange(matches);
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
            var matches = await _matchRepository.GetAll(
                    m => m.CategoryId == categoryId && m.WeightDivisionId == weightDivisionId)
                .Include(m => m.AParticipant).ThenInclude(p => p.Team)
                .Include(m => m.BParticipant).ThenInclude(p => p.Team)
                .Include(m => m.WinnerParticipant).ThenInclude(p => p.Team)
                .ToListAsync();
            if (matches.Any())
            {
                matches = ProcessMatches(matches);
                foreach (var match in matches)
                {
                    _matchRepository.Update(match);
                }
            }

            // CANNOT RUN BRACKET WITHOUT MATCHES CREATED
            // else
            // {
            //     matches = await CreateMatchesAsync(categoryId, weightDivisionId);
            //     matches = ProcessMatches(matches);
            //     _matchRepository.AddRange(matches);
            // }
            return matches;
        }

        public async Task SetMatchResultAsync(MatchResultModel model)
        {
            var match = await _matchRepository.GetAll(m => m.MatchId == model.MatchId).Include(m => m.NextMatch)
                .FirstOrDefaultAsync();
            if (match != null)
            {
                match.WinnerParticipantId = model.WinnerParticipantId;
                match.MatchResultType = (int) model.RoundResultType;
                match.MatchResultDetails = GetRoundResultDetailsJson(model);
                if (match.Round == 1)
                {
                    var lostParticipantId = match.WinnerParticipantId == match.AParticipantId ?
                        match.BParticipantId :
                        match.AParticipantId;
                    var bufferMatch = await _matchRepository.FirstOrDefaultAsync(m =>
                        m.WeightDivisionId == match.WeightDivisionId &&
                        m.CategoryId == match.CategoryId &&
                        m.MatchType == (int) MatchTypeEnum.Buffer);
                    if (bufferMatch != null && bufferMatch.BParticipantId == null)
                    {
                        bufferMatch.BParticipantId = lostParticipantId;
                        _matchRepository.Update(bufferMatch);
                    }
                    else
                    {
                        var thirdPlaceMatch = await _matchRepository.FirstOrDefaultAsync(m =>
                            m.WeightDivisionId == match.WeightDivisionId &&
                            m.CategoryId == match.CategoryId &&
                            m.MatchType == (int) MatchTypeEnum.ThirdPlace);
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
                    if (!await _matchRepository.GetAll(m => m.WeightDivisionId == match.WeightDivisionId &&
                            m.CategoryId == match.CategoryId &&
                            m.Round == 0 &&
                            m.MatchId != match.MatchId &&
                            m.WinnerParticipantId == null
                        ).AnyAsync())
                    {
                        await _weightDivisionService.SetWeightDivisionCompletedAsync(match.WeightDivisionId);
                    }
                }
                _matchRepository.Update(match);
            }
        }

        public async Task<bool> AreMatchesCreatedForEvent(Guid eventId)
        {
            return await _matchRepository.GetAll(m => m.Category.EventId == eventId).AnyAsync();
        }

        #region private methods
        private List<Match> ProcessMatches(List<Match> matchesToProcess)
        {
            var maxRound = matchesToProcess.Max(m => m.Round);
            foreach (var matchToProcess in matchesToProcess.Where(m => m.Round == maxRound))
            {
                if (matchToProcess.AParticipantId == null)
                {
                    SetMatchNoContest(matchToProcess, matchToProcess.BParticipant);
                }
                else if (matchToProcess.BParticipantId == null)
                {
                    SetMatchNoContest(matchToProcess, matchToProcess.AParticipant);
                }
            }

            void SetMatchNoContest(Match matchToProcess, Participant winner)
            {
                matchToProcess.WinnerParticipantId = winner.ParticipantId;
                matchToProcess.WinnerParticipant = winner;
                matchToProcess.MatchResultType = (int) MatchResultTypeEnum.NC;
                if (matchToProcess.NextMatchId != null)
                {
                    var nextMatch = matchesToProcess.First(m => m.MatchId == matchToProcess.NextMatchId);
                    if (matchToProcess.Order % 2 == 0)
                    {
                        nextMatch.AParticipantId = winner.ParticipantId;
                        nextMatch.AParticipant = winner;
                    }
                    else
                    {
                        nextMatch.BParticipantId = winner.ParticipantId;
                        nextMatch.BParticipant = winner;
                    };
                }
            }
            return matchesToProcess;
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

        private List<Match> CreateMatches(List<Participant> orderedParticipants, Guid weightDivisionId, Guid categoryId)
        {
            var matches = new List<Match>();
            if (orderedParticipants.Any())
            {
                var maxRound = orderedParticipants.Count == 3 ?
                    1 :
                    (int) Math.Log(orderedParticipants.Count(), 2) - 1;

                if (orderedParticipants.Count <= 2)
                {
                    var finalMatch = GetMatch(weightDivisionId, categoryId, 0, 0, null);
                    finalMatch.AParticipantId = orderedParticipants.FirstOrDefault().ParticipantId;
                    finalMatch.AParticipant = orderedParticipants.FirstOrDefault();
                    var bParticipant = orderedParticipants.ElementAtOrDefault(1);
                    if (bParticipant != null)
                    {
                        finalMatch.BParticipantId = bParticipant.ParticipantId;
                        finalMatch.BParticipant = bParticipant;
                    }
                    matches.Add(finalMatch);
                    return matches;
                }

                matches.Add(GetMatch(weightDivisionId, categoryId, 0, 0, null));
                if (orderedParticipants.Count() > 2)
                {
                    matches.Add(GetMatch(weightDivisionId, categoryId, 0, 1, null, (int) MatchTypeEnum.ThirdPlace));
                }

                for (var i = 1; i <= maxRound; i++)
                {
                    var roundMatches = GetRoundMatches(matches.Where(r => r.Round == i - 1 && r.MatchType == (int) MatchTypeEnum.Standard), i);
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
                            roundMatches[1].MatchType = (int) MatchTypeEnum.Buffer;
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
            }
            return matches;

        }

        private List<Match> GetRoundMatches(IEnumerable<Match> parentMatches, int round)
        {
            var matchesForRound = new List<Match>();
            foreach (var parentMatch in parentMatches)
            {
                for (var i = 0; i < 2; i++)
                {
                    matchesForRound.Add(GetMatch(parentMatch.WeightDivisionId, parentMatch.CategoryId, round, 2 * parentMatch.Order + i, parentMatch.MatchId));
                }
            }
            return matchesForRound;
        }

        private Match GetMatch(Guid weightDivisionId, Guid categoryId, int round, int order, Guid? nextMatchId, int matchType = (int) MatchTypeEnum.Standard)
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

        public async Task<bool> AreMatchesCreatedForEventAsync(Guid eventId)
        {
            return await _matchRepository.GetAll(m => m.Category.EventId == eventId).AnyAsync();
        }

        public async Task DeleteMatchesForEventAsync(Guid eventId)
        {
            var matches = await _matchRepository.GetAll(m => m.Category.EventId == eventId).ToListAsync();
            _matchRepository.DeleteRange(matches);
        }

        #endregion
    }
}