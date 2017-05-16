using System.Collections.Generic;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services
{
    public interface IFighterService
    {
      List<FighterModel> GetFighterModelsByFilter(FighterFilterModel filter);
      List<FighterModel> GetOrderedListForBrackets(FighterFilterModel filter);
      string AddFightersByModels(List<FighterModel> fighterModels);
    }
}
