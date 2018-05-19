using System;

namespace TRNMNT.Core.Model.Bracket
{
    public class BracketResultModel
    {
        public Guid BracketId { get; set; }
        public Guid FirstPlaceParticipantId { get; set; }
        public Guid SecondPlaceParticipantId{ get; set; }
        public Guid ThirdPlaceParticipantId { get; set; }
    }
}
