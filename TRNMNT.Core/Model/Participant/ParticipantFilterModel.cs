using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantFilterModel
    {
        public Guid EventId { get; set; }

        public int PageIndex { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? WeightDivisionId { get; set; }

    }
}
