using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Medalist;
using TRNMNT.Core.Model.Participant;
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
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly IParticipantService _participantService;

        public ResultsService(
            IRepository<Team> teamRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<Match> roundRepository,
            IWeightDivisionService weightDivisionService,
            IParticipantService participantService)
        {
            _teamRepository = teamRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _matchRepository = roundRepository;
            _weightDivisionService = weightDivisionService;
            _participantService = participantService;
        }



        public async Task<IEnumerable<ParticipantInAbsoluteDivisionModel>> GetParticipantsForAbsoluteAsync(Guid categoryId)
        {
            var weightDivisions = await _weightDivisionService.GetWeightDivisionsByCategoryIdAsync(categoryId);

            if (weightDivisions.Any(wd => wd.CompleteTs == null))
            {
                throw new Exception(
                    $"For Weight divisions for category with id {categoryId} not all rounds has winners");
            }

            var absoluteDivisionPaticipants = new List<ParticipantInAbsoluteDivisionModel>();
            var selectedParticipantIds = (await _participantService.GetParticipantsInAbsoluteDivisionByCategoryAsync(categoryId)).Select(p => p.ParticipantId);
            foreach (var weightDivision in weightDivisions)
            {
                absoluteDivisionPaticipants.AddRange(
                    (await GetWeightDivisionMedalistsAsync(weightDivision.WeightDivisionId))
                    .Select(p => new ParticipantInAbsoluteDivisionModel()
                    {
                        ParticipantId = p.Participant.ParticipantId,
                        FirstName = p.Participant.FirstName,
                        LastName = p.Participant.LastName,
                        TeamName = p.Participant.TeamName,
                        WeightDivisionName = weightDivision.Name,
                        IsSelectedIntoDivision = selectedParticipantIds.Contains(p.Participant.ParticipantId)
                    }).ToList());
            }
            return absoluteDivisionPaticipants;
        }


        public async Task<List<MedalistModel>> GetWeightDivisionMedalistsAsync(Guid weightDivisionId)
        {
            var participantResults = new List<MedalistModel>();
            if (await _weightDivisionService.IsWeightDivisionCompletedAsync(weightDivisionId))
            {
                var finals = _matchRepository.GetAll(m => m.WeightDivisionID == weightDivisionId && m.Round == 0)
                    .Include(m => m.AParticipant).ThenInclude(p => p.Team)
                    .Include(m => m.BParticipant).ThenInclude(p => p.Team)
                    .Include(m => m.WinnerParticipant).ThenInclude(p => p.Team)
                    ;
                foreach (var match in finals)
                {
                    if (match.WinnerParticipant != null)
                    {
                        if (match.MatchType == (int)MatchTypeEnum.ThirdPlace)
                        {
                            participantResults.Add(new MedalistModel()
                            {
                                Participant = ModelMapper.GetParticipantSimpleModel(match.WinnerParticipant),
                                Place = 3,
                                Points = 1
                            });
                        }
                        else
                        {
                            participantResults.Add(new MedalistModel()
                            {
                                Participant = ModelMapper.GetParticipantSimpleModel(match.WinnerParticipant),
                                Place = 1,
                                Points = 9
                            });
                            if (match.BParticipant != null && match.AParticipant != null)
                            {
                                participantResults.Add(
                                    match.AParticipantId == match.WinnerParticipantId
                                        ? new MedalistModel()
                                        {
                                            Participant = ModelMapper.GetParticipantSimpleModel(match.BParticipant),
                                            Place = 2,
                                            Points = 3
                                        }
                                        : new MedalistModel()
                                        {
                                            Participant = ModelMapper.GetParticipantSimpleModel(match.AParticipant),
                                            Place = 2,
                                            Points = 3
                                        });
                            }
                        }

                    }
                }
            }
            return participantResults;
        }

        public async Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds)
        {
            var results = new List<TeamResultModel>();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryWeightDivisionMedalistGroup>> GetGrouppedPersonalResultsAsync(IEnumerable<Guid> categoryIds)
        {
            var categoryWeightDivisioMedalistGroups = new List<CategoryWeightDivisionMedalistGroup>();
            foreach (var categoryId in categoryIds)
            {
                var weightDivisions = await _weightDivisionService.GetWeightDivisionsByCategoryIdAsync(categoryId, true);
                var categoryWeightDivisioMedalistGroup = new CategoryWeightDivisionMedalistGroup()
                {
                    CategoryName = weightDivisions.FirstOrDefault()?.Category.Name,
                    WeightDivisionMedalistGroups = new List<WeightDivisionMedalistGroup>()
                };

                foreach (var weightDivision in weightDivisions)
                {
                    categoryWeightDivisioMedalistGroup.WeightDivisionMedalistGroups.Add(new WeightDivisionMedalistGroup()
                    {
                        WeightDivisionName = weightDivision.Name,
                        Medalists = await GetWeightDivisionMedalistsAsync(weightDivision.WeightDivisionId)

                    });
                }
                categoryWeightDivisioMedalistGroups.Add(categoryWeightDivisioMedalistGroup);
            }
            return categoryWeightDivisioMedalistGroups;
        }
    }
}
