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
        private readonly IRepository<Match> _roundRepository;


        public ResultsService(
            IRepository<Team> teamRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<Match> roundRepository)
        {
            _teamRepository = teamRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _roundRepository = roundRepository;
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
                    var finals = await _roundRepository.GetAllIncluding(r => r.BracketId == bracketId && r.Round == 0, r => r.AParticipant, r => r.BParticipant, r => r.WinnerParticipant).ToListAsync();
                    foreach (var round in finals)
                    {
                        if (round.WinnerParticipant != null)
                        {
                            if (round.RoundType != (int)MatchTypeEnum.ThirdPlace)
                            {
                                medalists.Add((round.WinnerParticipant, 1));
                                if (round.SecondParticipant != null && round.AParticipant != null)
                                {
                                    medalists.Add(
                                        round.AParticipantId == round.WinnerParticipantId
                                            ? (round.SecondParticipant, 2)
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
