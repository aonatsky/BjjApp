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
            //var bracket = await _bracketRepository.GetAll(b => b.WeightDivisionId == weightDivisionId)
            //    .Include(b => b.Rounds).ThenInclude(r => r.AParticipant).ThenInclude(p => p.Team)
            //    .Include(b => b.Rounds).ThenInclude(r => r.BParticipant).ThenInclude(p => p.Team)
            //    .Include(b => b.WeightDivision).ThenInclude(w => w.Category)
            //    .FirstOrDefaultAsync();
            //return await _bracketsFileService.GetBracketsFileAsync(GetOrderedParticipantListFromBracket(bracket), GetBracketTtitle(bracket.WeightDivision.Category, bracket.WeightDivision));
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

        public async Task SetRoundResultAsync(RoundResultModel model)
        {
            var round = await _roundRepository.GetAll(r => r.RoundId == model.RoundId).Include(r => r.NextRound)
                .FirstOrDefaultAsync();
            if (round != null)
            {
                round.WinnerParticipantId = model.WinnerParticipantId;
                round.RoundResultType = (int)model.RoundResultType;
                round.RoundResultDetails = GetRoundResultDetailsJson(model);
                if (round.Stage == 1)
                {
                    var lostParticipantId = round.WinnerParticipantId == round.AParticipantId
                        ? round.SecondParticipantId
                        : round.AParticipantId;
                    var bufferRound = await _roundRepository.FirstOrDefaultAsync(r =>
                        r.BracketId == round.BracketId && r.MatchType == (int)MatchTypeEnum.Buffer);
                    if (bufferRound != null && bufferRound.SecondParticipantId == null)
                    {
                        bufferRound.SecondParticipantId = lostParticipantId;
                    }
                    else
                    {
                        var thirdPlaceRound = await _roundRepository.FirstOrDefaultAsync(r =>
                            r.BracketId == round.BracketId && r.MatchType == (int)MatchTypeEnum.ThirdPlace);
                        if (thirdPlaceRound != null)
                        {
                            if (round.Order % 2 == 0)
                            {
                                thirdPlaceRound.AParticipantId = lostParticipantId;
                            }
                            else
                            {
                                thirdPlaceRound.SecondParticipantId = lostParticipantId;
                            }

                            //if buffer exists = 3 participants
                            if (bufferRound != null)
                            {
                                thirdPlaceRound.WinnerParticipantId = lostParticipantId;
                            }

                            _roundRepository.Update(thirdPlaceRound);
                        }
                    }
                }

                if (round.NextRound != null)
                {
                    if (round.Order % 2 == 0)
                    {
                        round.NextRound.AParticipantId = round.WinnerParticipantId;
                    }
                    else
                    {
                        round.NextRound.SecondParticipantId = round.WinnerParticipantId;
                    }
                }

                if (round.Stage == 0)
                {
                    if (!await _roundRepository.GetAll(r => r.BracketId == round.BracketId
                                                            && r.Round == 0
                                                            && r.RoundId != round.RoundId
                                                            && r.WinnerParticipantId == null
                    ).AnyAsync())
                    {
                        var bracket = await _bracketRepository
                            .GetAll(b => b.BracketId == round.BracketId).Include(b => b.WeightDivision)
                            .FirstOrDefaultAsync();
                        bracket.CompleteTs = DateTime.UtcNow;
                        _bracketRepository.Update(bracket);
                        await CheckCategoryCompletionAsync(bracket);
                    }
                }

                _roundRepository.Update(round);
            }
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
            var bracket = await _bracketRepository.GetAll(b => b.BracketId == bracketResultModel.BracketId).Include(b => b.WeightDivision).FirstOrDefaultAsync();
            var rounds = await _roundRepository.GetAll(r =>
                r.BracketId == bracketResultModel.BracketId && r.Round == 0).ToListAsync();
            var thirdPlaceRound = rounds.FirstOrDefault(r => r.RoundType == (int)MatchTypeEnum.ThirdPlace);
            var finalRound = rounds.FirstOrDefault(r => r.RoundType != (int)MatchTypeEnum.ThirdPlace);
            if (finalRound != null)
            {
                finalRound.AParticipantId = bracketResultModel.FirstPlaceParticipantId;
                finalRound.SecondParticipantId = bracketResultModel.SecondPlaceParticipantId;
                finalRound.WinnerParticipantId = bracketResultModel.FirstPlaceParticipantId;
                _roundRepository.Update(finalRound);
            }
            if (thirdPlaceRound != null)
            {
                thirdPlaceRound.WinnerParticipantId = bracketResultModel.ThirdPlaceParticipantId;
                thirdPlaceRound.AParticipantId = bracketResultModel.ThirdPlaceParticipantId;
                _roundRepository.Update(thirdPlaceRound);
            }
            bracket.CompleteTs = DateTime.UtcNow;
            _bracketRepository.Update(bracket);
            await CheckCategoryCompletionAsync(bracket);
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

                if (roundModel.SecondParticipant != null)
                {
                    round.BParticipantId = roundModel.SecondParticipant.ParticipantId;
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

        private async Task<Bracket> GetBracketAsync(Guid weightDivisionId)
        {
            var bracket = await _bracketRepository.GetAll(b => b.WeightDivisionId == weightDivisionId)
                .Include(b => b.WeightDivision)
                .Include(b => b.Rounds).ThenInclude(r => r.AParticipant).ThenInclude(p => p.Team)
                .Include(b => b.Rounds).ThenInclude(r => r.BParticipant).ThenInclude(p => p.Team)
                .Include(b => b.Rounds).ThenInclude(r => r.WinnerParticipant).ThenInclude(p => p.Team)
                .FirstOrDefaultAsync();
            return bracket;
        }

        private async Task<Bracket> CreateBracketAsync(Guid weightDivisionId)
        {
            var weightDivision = await _weightDivisionService.GetWeightDivisionAsync(weightDivisionId);
            var category = await _categoryService.GetCategoryAsync(weightDivision.CategoryId);
            var participants = await GetOrderedListForNewBracketAsync(weightDivisionId);
            var bracket = new Bracket()
            {
                BracketId = Guid.NewGuid(),
                WeightDivisionId = weightDivisionId,
                RoundTime = category.MatchTime,
                Title = GetBracketTtitle(category, weightDivision)
            };

            bracket.Rounds = CreateRoundStructure(participants.ToArray(), bracket.BracketId);
            return bracket;
        }

        private string GetBracketTtitle(Category category, WeightDivision weightDivision) => $"{category.Name} / {weightDivision.Name}";

        private string GetRoundResultDetailsJson(RoundResultModel model)
        {
            var jObject = new JObject(
                new JProperty(nameof(model.AParticipantPoints), model.AParticipantPoints),
                new JProperty(nameof(model.AParticipantAdvantages), model.AParticipantAdvantages),
                new JProperty(nameof(model.AParticipantPenalties), model.AParticipantPenalties),
                new JProperty(nameof(model.SecondParticipantPoints), model.SecondParticipantPoints),
                new JProperty(nameof(model.SecondParticipantAdvantages), model.SecondParticipantAdvantages),
                new JProperty(nameof(model.SecondParticipantPenalties), model.SecondParticipantPenalties),
                new JProperty(nameof(model.CompleteTime), model.CompleteTime),
                new JProperty(nameof(model.SubmissionType), model.SubmissionType)
            );
            return jObject.ToString();

        }

        private List<Match> GetStageRoundsAsync(IEnumerable<Match> stageRounds, int stage, Guid bracketId)
        {
            var childRounds = new List<Match>();
            if (stage == 0)
            {
                childRounds.Add(new Match()
                {
                    RoundId = Guid.NewGuid(),
                    BracketId = bracketId,
                    Round = stage,
                    MatchType = (int)MatchTypeEnum.Standard,
                    Order = 0
                });
                childRounds.Add(new Match()
                {
                    RoundId = Guid.NewGuid(),
                    BracketId = bracketId,
                    Round = stage,
                    MatchType = (int)MatchTypeEnum.ThirdPlace,
                    Order = 1
                });
            }
            else
            {
                foreach (var parentRound in stageRounds)
                {
                    if (parentRound.MatchType == (int)MatchTypeEnum.Standard)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            childRounds.Add(new Match()
                            {
                                RoundId = Guid.NewGuid(),
                                BracketId = bracketId,
                                NextMatchId = parentRound.RoundId,
                                NextRound = parentRound,
                                Round = stage,
                                Order = (2 * parentRound.Order) + i
                            });
                        }
                    }
                }
            }

            return childRounds;
        }



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

        private async Task CheckCategoryCompletionAsync(Bracket bracket)
        {
            //find if other brackets are complete
            if (!(await _weightDivisionService.GetWeightDivisionsByCategoryIdAsync(bracket.WeightDivision
                .CategoryId)).SelectMany(wd => wd.Brackets).Any(b => b.CompleteTs == null && b.BracketId != bracket.BracketId))
            {
                await _categoryService.SetCategoryCompleteAsync(bracket.WeightDivision.CategoryId);
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



        #endregion
    }
}

