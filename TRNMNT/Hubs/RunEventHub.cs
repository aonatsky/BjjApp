﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Web.Hubs
{
    public class RunEventHub : BaseHub<IClientProxy>
    {
        private readonly IBracketService _bracketService;

        public RunEventHub(IBracketService bracketService)
        {
            _bracketService = bracketService;
        }

        //public async Task UpdateBracketRounds(Guid weightDivisionId)
        //{
        //    var bracketModel = await _bracketService.GetBracketAsync(weightDivisionId);
        //    if (bracketModel != null)
        //    {
        //        await ToGroup(weightDivisionId).RefreshRound(bracketModel);
        //    }
        //}
    }

    public interface IRunEventHubContract
    {
        Task RefreshRound(BracketModel bracketModel);
    }
}