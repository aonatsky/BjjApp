using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantEventModel : ParticipantTableModel
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Result { get; set; }
        public bool CorrectionsAllowed { get; set; }
    }
}