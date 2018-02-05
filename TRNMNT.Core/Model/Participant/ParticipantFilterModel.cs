using System;
using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantFilterModel
    {
        public Guid EventId { get; set; }

        public int PageIndex { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? WeightDivisionId { get; set; }

        public bool IsMembersOnly { get; set; }

        public ParticpantSortField SortField { get; set; }

        public SortDirectionEnum SortDirection { get; set; }

    }
}
