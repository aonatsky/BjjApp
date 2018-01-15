using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IBracketService
    {
        Task<BracketModel> CreateBracketAsync(Guid weightDivisionId);
        Task<BracketModel> GetBracket(Guid bracketId);
        Task UpdateBracket(BracketModel model);
    }
}
