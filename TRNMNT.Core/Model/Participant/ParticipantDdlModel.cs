using System.Collections.Generic;
using TRNMNT.Core.Model.Category;
using TRNMNT.Core.Model.Team;
using TRNMNT.Core.Model.WeightDivision;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantDdlModel
    {
        public IEnumerable<TeamModelBase> Teams { get; set; }
        public IEnumerable<CategoryModelBase> Categories { get; set; }
        public IEnumerable<WeightDivisionModel> WeightDivisions { get; set; }
    }
}