using System;
using System.Collections.Generic;
using System.Text;
using TRNMNT.Core.Model.Round;

namespace TRNMNT.Core.Model.Bracket
{
    public class BracketModel
    {
        public Guid BracketId { get; set; }
        public IEnumerable<RoundModel> RoundModels { get; set; }
    }
}
