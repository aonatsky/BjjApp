using System;
using System.Collections.Generic;
using TRNMNT.Core.Model.Round;

namespace TRNMNT.Core.Model.Bracket
{
    public class BracketModel
    {
        public Guid BracketId { get; set; }
        public string Title { get; set; }
        public ICollection<RoundModel> RoundModels { get; set; }
        public ICollection<MedalistModel> Medalists { get; set; }
    }
}
