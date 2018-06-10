using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Web.Hubs
{
    public class RunEventHub : BaseHub<IRunEventHubContract>
    {
        private readonly IBracketService _bracketService;
        private readonly ILogger<RunEventHub> _logger;

        public RunEventHub(IBracketService bracketService, ILogger<RunEventHub> logger)
        {
            _bracketService = bracketService;
            _logger = logger;
        }

        #region Public Methods

        public async Task RoundStart(MatchModel roundModel)
        {
            await ToGroup(roundModel.WeightDivisionId).RoundStart(roundModel);
        }

        public async Task RoundComplete(Guid weightDivisionId)
        {
            var refreshModel = await GetRefreshRoundModel(weightDivisionId);
            if (refreshModel == null) return;
            await ToGroup(weightDivisionId).RoundComplete(refreshModel);
        }

        public async Task WeightDivisionChanged(ChangeWeightDivisionModel changeModel)
        {
            var refreshModel = await GetRefreshRoundModel(changeModel.WeightDivisionId);
            if (refreshModel == null) return;
            await ToGroup(changeModel.SynchronizationId).WeightDivisionChanged(refreshModel);
        }

        #endregion

        #region Private Methods

        private async Task<RefreshBracketModel> GetRefreshRoundModel(Guid weightDivisionId)
        {
            var errorMessage = $"Bracket model for weightDivision with id {weightDivisionId} could not be found or created!";
            try
            {
                var bracketModel = await _bracketService.RunWeightDivision(weightDivisionId);
                if (bracketModel == null)
                {
                    _logger.LogError(errorMessage);
                    return null;
                }
                return new RefreshBracketModel
                {
                    WeightDivisionId = weightDivisionId,
                    Bracket = bracketModel
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{errorMessage}");
            }
            return null;
        }

        #endregion
    }


    public interface IRunEventHubContract
    {
        Task RoundStart(MatchModel roundModel);
        Task RoundComplete(RefreshBracketModel refreshModel);
        Task WeightDivisionChanged(RefreshBracketModel refreshModel);

    }
}