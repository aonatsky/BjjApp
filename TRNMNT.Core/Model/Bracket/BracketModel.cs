using System;
using System.Collections.Generic;
using TRNMNT.Core.Model.Round;

namespace TRNMNT.Core.Model.Bracket
{
    public class BracketModel
    {
        public Guid BracketId { get; set; }
        public List<RoundModel> RoundModels { get; set; }
    }
}
