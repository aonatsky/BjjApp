using System;
using System.Collections.Generic;
using TRNMNT.Core.Model.Round;

namespace TRNMNT.Core.Model.Bracket
{
    public class BracketModel
    {
        public Guid WeightDivisionId { get; set; }
        public string Title { get; set; }
        public List<MatchModel> MatchModels { get; set; }
        public List<MedalistModel> Medalists { get; set; }
    }
}
