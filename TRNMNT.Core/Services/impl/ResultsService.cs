using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class ResultsService : IResultsService
    {
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<WeightDivision> _weightDivisionRepository;
        private readonly IRepository<Match> _matchRepository;


        public ResultsService(
            IRepository<Team> teamRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<Match> roundRepository)
        {
            _teamRepository = teamRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _matchRepository = roundRepository;
        }

        public Task<IEnumerable<Participant>> GetMedalistsForAbsolute(Guid categoryId, bool includeAbsolute)
        {
            var medalists = new List<Participant>();
            var finalMatches = _matchRepository.GetAll(m => m.CategoryId == categoryId && m.Round == 0 && !m.WeightDivision.IsAbsolute);
            foreach (var match in finalMatches)
            {
                medalists.Add(match.AParticipant);
                medalists.Add(match.BParticipant);
            }
        }

        private List<> GetMedalistsForBracket(Bracket bracket)
        {
            var bracketMedalists = new List<MedalistProcessingModel>();
            if (bracket.CompleteTs != null)
            {
                var finals = bracket.Rounds.Where(r => r.Stage == 0);
                foreach (var round in finals)
                {

                    if (round.WinnerParticipant != null)
                    {
                        if (round.MatchType == (int)MatchTypeEnum.ThirdPlace)
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
                            if (round.BParticipant != null && round.AParticipant != null)
                            {
                                bracketMedalists.Add(
                                    round.AParticipantId == round.WinnerParticipantId
                                        ? new MedalistProcessingModel()
                                        {
                                            Participant = round.BParticipant,
                                            Place = 2,
                                            Points = 3
                                        }
                                        : new MedalistProcessingModel()
                                        {
                                            Participant = round.AParticipant,
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

        public async Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds)
        {
            var results = new List<TeamResultModel>();
            var medalists = new List<(Participant, int)>();

            foreach (var categoryId in categoryIds)
            {
                var bracketIds = await _weightDivisionRepository.GetAllIncluding(wd => wd.CategoryId == categoryId, wd => wd.Brackets)
                    .Select(wd => wd.Brackets.First().BracketId).ToListAsync();

                foreach (var bracketId in bracketIds)
                {
                    var finals = await _matchRepository.GetAllIncluding(r => r.BracketId == bracketId && r.Round == 0, r => r.AParticipant, r => r.BParticipant, r => r.WinnerParticipant).ToListAsync();
                    foreach (var round in finals)
                    {
                        if (round.WinnerParticipant != null)
                        {
                            if (round.RoundType != (int)MatchTypeEnum.ThirdPlace)
                            {
                                medalists.Add((round.WinnerParticipant, 1));
                                if (round.BParticipant != null && round.AParticipant != null)
                                {
                                    medalists.Add(
                                        round.AParticipantId == round.WinnerParticipantId
                                            ? (round.BParticipant, 2)
                                            : (round.AParticipant, 2));
                                }

                            }
                            else
                            {
                                medalists.Add((round.WinnerParticipant, 3));
                            }
                        }
                    }
                }
            }
            return results;
        }

    }
}
