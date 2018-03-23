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
    class ResultsService : IResultsService
    {
        private readonly IRepository<BracketService> _bracketRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<WeightDivision> _weightDivisionRepository;
        private readonly IRepository<Round> _roundRepository;


        public ResultsService(IRepository<BracketService> bracketRepository,
            IRepository<Category> categoryRepository,
            IRepository<Team> teamRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<Round> roundRepository)
        {
            _bracketRepository = bracketRepository;
            _categoryRepository = categoryRepository;
            _teamRepository = teamRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _roundRepository = roundRepository;
        }

        public async Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds)
        {
            var teams = await _teamRepository.GetAll().Take(3).ToListAsync();
            var results = new List<TeamResultModel>();
            for (int i = 0; i < 3; i++)
            {
                results.Add(new TeamResultModel()
                {
                    Points = (i + 3) * 10,
                    TeamId = teams[i].TeamId,
                    TeamName = teams[i].Name
                });
            }

            //            return results;

            var firstPlaceParticipantIds = new List<Guid>();
            var secondPlaceParticipantIds = new List<Guid>();
            var thirdPlaceParticipantIds = new List<Guid>();

            foreach (var categoryId in categoryIds)
            {
                var bracketIds = await _weightDivisionRepository.GetAllIncluding(wd => wd.CategoryId == categoryId, wd => wd.Brackets)
                    .Select(wd => wd.Brackets.First().BracketId).ToListAsync();

                foreach (var bracketId in bracketIds)
                {
                    var rounds =  await _roundRepository.GetAll(r => r.BracketId == bracketId && r.Stage == 0).ToListAsync();
                    foreach (var round in rounds)
                    {
                        if (round.WinnerParticipantId.HasValue)
                        {
                            if (round.RoundType == (int) RoundTypeEnum.Standard)
                            {
                                firstPlaceParticipantIds.Add(round.WinnerParticipantId.Value);
                                secondPlaceParticipantIds.Add(
                                    round.FirstParticipantId.Value == round.WinnerParticipantId.Value
                                        ? round.SecondParticipantId.Value
                                        : round.FirstParticipantId.Value);
                            }
                            else
                            {
                                thirdPlaceParticipantIds.Add(round.WinnerParticipantId.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}
