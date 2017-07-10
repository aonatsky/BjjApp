using System.Collections.Generic;
using TRNMNT.Web.Core.Model;

namespace TRNMNT.Web.Core.Services
{
    public interface IFighterService
    {
      List<FighterModel> GetFighterModelsByFilter(FighterFilterModel filter);
      List<FighterModel> GetOrderedListForBrackets(FighterFilterModel filter);
      string AddFightersByModels(List<FighterModel> fighterModels);
    }
}
