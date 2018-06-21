using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Core.Enum;
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

        public async Task<BracketModel> RunWeightDivision(Guid weightDivisionId)
        {
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId);
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

            //TODO 
            return new CustomFile();
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
                Title = GetBracketTtitle(weightDivision.Category, weightDivision),
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
            };

            if (match.WinnerParticipantId != null)
            {
                model.WinnerParticipant = GetParticipantSimpleModel(match.WinnerParticipant);
                if (match.WinnerParticipantId == match.AParticipantId)
                {
                    if (match.MatchResultType != (int)MatchResultTypeEnum.DQ)
                    {
                        model.AParticipantResult = ((MatchResultTypeEnum)match.MatchResultType).ToString();
                    }
                    else
                    {
                        model.BParticipantResult = ((MatchResultTypeEnum)match.MatchResultType).ToString();
                    }
                }
                else
                {
                    if (match.MatchResultType != (int)MatchResultTypeEnum.DQ)
                    {
                        model.BParticipantResult = ((MatchResultTypeEnum)match.MatchResultType).ToString();
                    }
                    else
                    {
                        model.AParticipantResult = ((MatchResultTypeEnum)match.MatchResultType).ToString();
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


        private string GetBracketTtitle(Category category, WeightDivision weightDivision) => $"{category.Name} / {weightDivision.Name}";

        #endregion
    }
}

