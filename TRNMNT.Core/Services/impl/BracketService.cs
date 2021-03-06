﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Core.Services.Impl;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class BracketService : IBracketService
    {
        #region Dependencies

        private readonly IParticipantService _participantService;
        private readonly BracketsFileService _fileService;
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly IMatchService _matchService;
        private readonly IResultsService _resultsService;

        #endregion

        #region .ctor
        public BracketService(
            IParticipantService participantService,
            BracketsFileService fileService,
            IWeightDivisionService weightDivisionService,
            ICategoryService categoryService,
            IMatchService matchService,
            IRepository<Match> matchRepository,
            IResultsService resultsService)
        {
            _participantService = participantService;
            _fileService = fileService;
            _weightDivisionService = weightDivisionService;
            _matchService = matchService;
            _resultsService = resultsService;
        }
        #endregion

        #region Public Methods
        public async Task<BracketModel> GetBracketModelAsync(Guid weightDivisionId)
        {
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId, true);
            var matches = await _matchService.GetMatchesAsync(weightDivision.CategoryId, weightDivisionId);
            return GetBracketModel(weightDivision, matches);
        }

        public async Task CreateBracketsForEventAsync(Guid eventId)
        {
            var weightDivisions = await _weightDivisionService.GetWeightDivisionsByEventIdAsync(eventId, false);
            foreach (var weightDivision in weightDivisions)
            {
                var participants =
                    await _participantService.GetParticipantsByWeightDivisionAsync(weightDivision.WeightDivisionId, true, false, true);
                var orderedParticapants = OrderParticipantsForBracket(participants.ToList());
                _matchService.CreateMatchesAsync(weightDivision.CategoryId, weightDivision.WeightDivisionId, orderedParticapants);
            }
        }

        public async Task DeleteBracketsForEventAsync(Guid eventId)
        {
            var weightDivisions = await _weightDivisionService.GetWeightDivisionsByEventIdAsync(eventId, false);
            if (weightDivisions.Any(wd => wd.StartTs != null))
            {
                throw new BusinessException("ERROR.EVENT_ALREADY_STARTED");
            }
            await _matchService.DeleteMatchesForEventAsync(eventId);
        }

        public async Task<BracketModel> RunWeightDivisionAsync(Guid weightDivisionId)
        {
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId, true);
            List<Match> matches;
            if (weightDivision.StartTs == null)
            {
                matches = await _matchService.GetProcessedMatchesAsync(weightDivision.CategoryId, weightDivision.WeightDivisionId);
                await _weightDivisionService.SetWeightDivisionStartedAsync(weightDivisionId);
            }
            else
            {
                matches = await _matchService.GetMatchesAsync(weightDivision.CategoryId, weightDivision.WeightDivisionId);
            }
            return GetBracketModel(weightDivision, matches);
        }

        public async Task<IEnumerable<ParticipantInAbsoluteDivisionModel>> GetParticipantsForAbsoluteDivisionAsync(Guid categoryId)
        {
            return await _resultsService.GetParticipantsForAbsoluteAsync(categoryId);
        }

        public async Task UpdateBracket(BracketModel model)
        {
            await _matchService.UpdateMatchesParticipantsAsync(model.MatchModels);
        }

        public async Task<CustomFile> GetBracketFileAsync(Guid weightDivisionId)
        {
            var participants =
                await _participantService.GetParticipantsByWeightDivisionAsync(weightDivisionId, true, false, true);
            var orderedParticapants = OrderParticipantsForBracket(participants.ToList());
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId, true);
            var matches = await  _matchService.GetFirstMatchesAsync(weightDivision.CategoryId, weightDivision.WeightDivisionId);
            return _fileService.GetBracketsFileAsync(matches, GetBracketTitle(weightDivision.Category, weightDivision));
        }

        public async Task<Dictionary<string, BracketModel>> GetBracketsByCategoryAsync(Guid categoryId)
        {
            var weightDivisions = await _weightDivisionService.GetWeightDivisionModelsByCategoryIdAsync(categoryId);
            var result = new Dictionary<string, BracketModel>();
            foreach (var weightDivision in weightDivisions)
            {
                result.Add(weightDivision.WeightDivisionId.ToString(), await GetBracketModelAsync(weightDivision.WeightDivisionId));
            }
            return result;
        }

        public async Task EditAbsoluteWeightDivisionAsync(CreateAbsoluteDivisionModel model)
        {
            var absoluteWeightDivision = await _weightDivisionService.GetAbsoluteWeightDivisionAsync(model.CategoryId);

            await _participantService.AddParticipantsToAbsoluteWeightDivisionAsync(
                model.ParticipantsIds,
                model.CategoryId,
                absoluteWeightDivision.WeightDivisionId);
        }

        public async Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds)
        {
            return await _resultsService.GetTeamResultsByCategoriesAsync(categoryIds);
        }

        public async Task<CustomFile> GetPersonalResultsFileByCategoriesAsync(
            IEnumerable<Guid> categoryIds)
        {

            return _fileService.GetPersonalResultsFileAsync((await _resultsService.GetGrouppedPersonalResultsAsync(categoryIds)).ToList());
        }

        public async Task SetBracketResultAsync(BracketResultModel bracketResultModel)
        {
            //todo
        }

        public async Task SetRoundResultAsync(MatchResultModel model)
        {
            await _matchService.SetMatchResultAsync(model);
        }

        #endregion

        #region Private methods

        private BracketModel GetBracketModel(WeightDivision weightDivision, List<Match> matches)
        {
            var model = new BracketModel
            {
                WeightDivisionId = weightDivision.WeightDivisionId,
                Title = GetBracketTitle(weightDivision.Category, weightDivision),
                Medalists = new List<MedalistModel>(),
                MatchModels = matches.Select(m => GetMatchModel(m, weightDivision.Category.MatchTime)).ToList()
            };
            return model;
        }

        private MatchModel GetMatchModel(Match match, int matchTime)
        {
            var model = new MatchModel()
            {
                MatchId = match.MatchId,
                NextMatchId = match.NextMatchId,
                Round = match.Round,
                AParticipant = match.AParticipant == null ? null : GetParticipantSimpleModel(match.AParticipant),
                BParticipant = match.BParticipant == null ? null : GetParticipantSimpleModel(match.BParticipant),
                MatchType = match.MatchType,
                MatchTime = matchTime,
                Order = match.Order,
                CategoryId = match.CategoryId,
                WeightDivisionId = match.WeightDivisionId
            };

            if (match.WinnerParticipantId != null)
            {
                model.WinnerParticipant = GetParticipantSimpleModel(match.WinnerParticipant);
                if (match.WinnerParticipantId == match.AParticipantId)
                {
                    if (match.MatchResultType != (int) MatchResultTypeEnum.DQ)
                    {
                        model.AParticipantResult = ((MatchResultTypeEnum) match.MatchResultType).ToString();
                    }
                    else
                    {
                        model.BParticipantResult = ((MatchResultTypeEnum) match.MatchResultType).ToString();
                    }
                }
                else
                {
                    if (match.MatchResultType != (int) MatchResultTypeEnum.DQ)
                    {
                        model.BParticipantResult = ((MatchResultTypeEnum) match.MatchResultType).ToString();
                    }
                    else
                    {
                        model.AParticipantResult = ((MatchResultTypeEnum) match.MatchResultType).ToString();
                    }
                }
            }

            return model;

        }

        private ParticipantSimpleModel GetParticipantSimpleModel(Participant participant)
        {
            return new ParticipantSimpleModel
            {
                FirstName = participant.FirstName,
                    LastName = participant.LastName,
                    ParticipantId = participant.ParticipantId,
                    DateOfBirth = participant.DateOfBirth,
                    TeamName = participant.Team.Name
            };
        }

        private string GetBracketTitle(Category category, WeightDivision weightDivision) => $"{category.Name} / {weightDivision.Name}";

        private const int ParticipantsMaxCount = 64;

        private int GetBracketSize(int participantCount)
        {
            if (participantCount == 0)
            {
                return 0;
            }

            if (participantCount == 3)
            {
                return 3;
            }

            for (var i = 1; i <= Math.Log(ParticipantsMaxCount, 2); i++)
            {
                var size = Math.Pow(2, i);
                if (size >= participantCount)
                {
                    return (int) size;
                }
            }

            return 2;

        }

        private List<Participant> OrderParticipantsForBracket(List<Participant> participants)
        {
            if (!participants.Any())
            {
                return participants;
            }
            var bracketSize = GetBracketSize(participants.Count);
            int participantsToAddCount = bracketSize - participants.Count;
            for (var i = 0; i < participantsToAddCount; i++)
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

        #endregion
    }
}