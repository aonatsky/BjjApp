using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Medalist;
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

        private readonly IRepository<Match> _roundRepository;
        private readonly IParticipantService _participantService;
        private readonly BracketsFileService _bracketsFileService;
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly ICategoryService _categoryService;
        private readonly IMatchService _matchService;

        #endregion

        #region .ctor
        public BracketService(
            IParticipantService participantService,
            BracketsFileService bracketsFileService,
            IWeightDivisionService weightDivisionService,
            ICategoryService categoryService,
            IMatchService matchService,
            IRepository<Match> matchRepository)
        {
            _participantService = participantService;
            _bracketsFileService = bracketsFileService;
            _weightDivisionService = weightDivisionService;
            _categoryService = categoryService;
            _matchService = matchService;
            _roundRepository = matchRepository;
        }
        #endregion

        #region Public Methods
        public async Task<BracketModel> GetBracketModelAsync(Guid categoryId, Guid weightDivisionId)
        {
            var matches = await _matchService.GetMatchesAsync(categoryId, weightDivisionId);
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId, true);
            return GetBracketModel(weightDivision, matches);
        }

        public async Task<BracketModel> RunWeightDivision(Guid weightDivisionId)
        {
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId);
            List<Match> matches;
            if (weightDivision.StartTs == null)
            {
                matches = await _matchService.GetProcessedMatchesAsync(weightDivision.CategoryId, weightDivision.WeightDivisionId);
                await _weightDivisionService.SetWeightDivisionStarted(weightDivisionId);
            }
            else
            {
                matches = await _matchService.GetMatchesAsync(weightDivision.CategoryId, weightDivision.WeightDivisionId);
            }
            return GetBracketModel(weightDivision, matches);
        }

        public async Task<bool> IsCategoryCompletedAsync(Guid categoryId)
        {
            return (await _categoryService.GetCategoryAsync(categoryId)).CompleteTs != null;
        }

        public async Task<List<ParticipantInAbsoluteDivisionModel>> GetParticipantsForAbsoluteDivisionAsync(Guid categoryId)
        {
            if ((await _categoryService.GetCategoryAsync(categoryId)).CompleteTs == null)
            {
                throw new Exception(
                    $"For Weight divisions for category with id {categoryId} not all rounds has winners");
            }

            var braketsForCategory = await _bracketRepository.GetAll(b => b.WeightDivision.CategoryId == categoryId && !b.WeightDivision.IsAbsolute)
                .Include(b => b.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.AParticipant).ThenInclude(p => p.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.BParticipant).ThenInclude(p => p.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant).ThenInclude(p => p.WeightDivision)
                .ToListAsync();

            var winnerParticipants =
                GetWinnersForBrackets(braketsForCategory)
                    .Select(p => new ParticipantInAbsoluteDivisionModel()
                    {
                        ParticipantId = p.ParticipantId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        TeamName = "",
                        WeightDivisionName = p.WeightDivision.Name,
                        IsSelectedIntoDivision = p.AbsoluteWeightDivisionId != null
                    }).ToList();
            return winnerParticipants;
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
                result.Add(weightDivision.WeightDivisionId.ToString(), await GetBracketModelAsync(categoryId, weightDivision.WeightDivisionId));
            }
            return result;
        }

        public async Task ManageAbsoluteWeightDivisionAsync(CreateAbsoluteDivisionModel model)
        {
            var absoluteWeightDivision = await _weightDivisionService.GetAbsoluteWeightDivisionAsync(model.CategoryId);

            await _participantService.AddAbsoluteWeightDivisionForParticipantsAsync(
                model.ParticipantsIds,
                model.CategoryId,
                absoluteWeightDivision.WeightDivisionId);
        }



        public async Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds)
        {
            var brackets = await _bracketRepository.GetAll(b => categoryIds.Contains(b.WeightDivision.CategoryId) && b.CompleteTs != null)
                    .Include(b => b.Rounds).ThenInclude(r => r.AParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.BParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant).ThenInclude(t => t.Team)
                    .ToListAsync()
                ;
            var categoryMedalists = new List<MedalistProcessingModel>();
            foreach (var bracket in brackets)
            {
                var bracketMedalists = GetMedalistsForBracket(bracket);
                var count = bracketMedalists.GroupBy(m => m.Participant.TeamId);
                if (bracketMedalists.Count == 1)
                {
                }
                else if (count.Count() == 1)
                {

                }
                else
                {
                    categoryMedalists.AddRange(bracketMedalists);
                }
            }

            return categoryMedalists.GroupBy(m => m.Participant.TeamId)
                 .Select(g => new TeamResultModel { TeamId = g.Key, Points = g.Sum(gg => gg.Points), TeamName = g.First().Participant.Team.Name });
        }


        public async Task<CustomFile> GetPersonalResultsFileByCategoriesAsync(
            IEnumerable<Guid> categoryIds)
        {
            var brackets = await _bracketRepository
                    .GetAll(b => categoryIds.Contains(b.WeightDivision.CategoryId) && b.CompleteTs != null)
                    .Include(b => b.Rounds).ThenInclude(r => r.AParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.BParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant).ThenInclude(t => t.Team)
                    .Include(b => b.WeightDivision).ThenInclude(w => w.Category)
                    .ToListAsync()
                ;
            var categoryWeightDivisioMedalistGroups = new List<CategoryWeightDivisionMedalistGroup>();
            foreach (var categoryId in categoryIds)
            {
                var categoryBrackets = brackets.Where(b => b.WeightDivision.CategoryId == categoryId);
                var categoryWeightDivisioMedalistGroup = new CategoryWeightDivisionMedalistGroup()
                {
                    CategoryName = categoryBrackets.First().WeightDivision.Category.Name,
                    WeightDivisionMedalistGroups = new List<WeightDivisionMedalistGroup>()
                };
                var weightDivisions = categoryBrackets.Select(b => b.WeightDivision);
                foreach (var weightDivision in weightDivisions)
                {
                    var bracket = categoryBrackets.First(b => b.WeightDivisionId == weightDivision.WeightDivisionId);
                    categoryWeightDivisioMedalistGroup.WeightDivisionMedalistGroups.Add(new WeightDivisionMedalistGroup()
                    {
                        WeightDivisionName = bracket.WeightDivision.Name,
                        Medalists = GetMedalistsForBracket(bracket)

                    });
                }
                categoryWeightDivisioMedalistGroups.Add(categoryWeightDivisioMedalistGroup);
            }
            return _bracketsFileService.GetPersonalResultsFileAsync(categoryWeightDivisioMedalistGroups);
        }

        public async Task SetBracketResultAsync(BracketResultModel bracketResultModel)
        {
            //todo
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
                AParticipant = match.AParticipant == null ? null : GetParticipantModel(match.AParticipant),
                BParticipant = match.BParticipant == null ? null : GetParticipantModel(match.BParticipant),
                MatchType = match.MatchType,
                MatchTime = matchTime,
                Order = match.Order,
            };

            if (match.WinnerParticipantId != null)
            {
                model.WinnerParticipant = GetParticipantModel(match.WinnerParticipant);
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

        private ParticipantSimpleModel GetParticipantModel(Participant participant)
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

        private void UpdateBracketRoundsFromModel(IEnumerable<Match> rounds, IEnumerable<MatchModel> roundModels)
        {

            foreach (var round in rounds)
            {
                var roundModel = roundModels.First(m => m.MatchId == round.RoundId);
                if (roundModel.AParticipant != null)
                {
                    round.AParticipantId = roundModel.AParticipant.ParticipantId;
                }
                else
                {
                    round.AParticipantId = null;
                }

                if (roundModel.BParticipant != null)
                {
                    round.BParticipantId = roundModel.BParticipant.ParticipantId;
                }
                else
                {
                    round.BParticipantId = null;
                }

                _roundRepository.Update(round);
            }
        }

        private List<Participant> GetWinnersForBrackets(IEnumerable<Bracket> brackets)
        {
            return brackets.SelectMany(b =>
                    GetMedalistsForBracket(b))
                .Select(r => r.Participant).ToList();
        }

        private string GetBracketTtitle(Category category, WeightDivision weightDivision) => $"{category.Name} / {weightDivision.Name}";

        private async Task StartWeightDivisionAsync(Guid weightDivisionId)
        {

            if (bracket.StartTs == null)
            {
                bracket.StartTs = DateTime.UtcNow;
                var firstRounds = bracket.Rounds.Where(r =>
                    r.Stage == bracket.Rounds.Max(rr => rr.Stage) && r.MatchType != (int)MatchTypeEnum.Buffer);
                foreach (var round in firstRounds)
                {
                    if (round.AParticipantId == null || round.BParticipantId == null)
                    {
                        if (round.AParticipantId == null)
                        {
                            round.WinnerParticipantId = round.BParticipantId;
                        }

                        if (round.BParticipantId == null)
                        {
                            round.WinnerParticipantId = round.AParticipantId;
                        }

                        if (round.NextRound != null)
                        {
                            if (round.Order % 2 == 0)
                            {
                                round.NextRound.AParticipantId = round.WinnerParticipantId;
                            }
                            else
                            {
                                round.NextRound.BParticipantId = round.WinnerParticipantId;
                            }
                        }

                        if (bracket.Rounds.Count == 1)
                        {
                            bracket.CompleteTs = DateTime.UtcNow;
                            await CheckCategoryCompletionAsync(bracket);
                        }
                    }
                }
            }
        }

        private List<MedalistProcessingModel> GetMedalistsForBracket(Bracket bracket)
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

        public async Task SetRoundResultAsync(MatchResultModel model)
        {
            await _matchService.SetMatchResultAsync(model);
        }



        #endregion
    }
}

