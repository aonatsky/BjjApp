using TRNMNT.Core.Model.Participant;

namespace TRNMNT.Core.Model
{
    public class MedalistModel
    {
        public ParticipantSimpleModel Participant { get; set; }
        public int Place { get; set; }
        public int Points { get; set; }
    }
}
