using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Medalist;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IResultsService
    {
        /// <summary>
        /// Returns results by selected categories;
        /// </summary>
        /// <param name="categoryIds">Category Id</param>
        /// <returns></returns>
        Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds);
        /// <summary>
        /// Returns candidates for absolute category;
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<IEnumerable<ParticipantInAbsoluteDivisionModel>> GetParticipantsForAbsoluteAsync(Guid categoryId);
        /// <summary>
        /// Returns medalists for weightdivision
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <returns></returns>
        Task<List<MedalistModel>> GetWeightDivisionMedalistsAsync(Guid weightDivisionId);
        /// <summary>
        /// Get personal results groupped by category and weightdivision
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        Task<IEnumerable<CategoryWeightDivisionMedalistGroup>> GetGrouppedPersonalResultsAsync(IEnumerable<Guid> categoryIds);

    }
}
