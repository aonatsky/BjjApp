using System.Collections.Generic;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services
{
    public interface IFighterService
    {
      List<FighterModel> GetFighterModels();
   }
}
