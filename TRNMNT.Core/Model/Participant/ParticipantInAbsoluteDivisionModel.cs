using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantInAbsoluteDivisionModel : ParticipantModelBase
    {
        public string TeamName { get; set; }
        public string WeightDivisionName { get; set; }
        public bool IsSelectedIntoDivision { get; set; }
    }
}