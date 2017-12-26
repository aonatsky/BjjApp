using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantFilterModel
    {
        public Guid EventId { get; set; }

        public int PageIndex { get; set; }

        public CategoryDivisionFilter Filter { get; set; }

    }
}
