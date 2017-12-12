﻿namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantTableModel : ParticipantModelBase
    {
        public string UserId { get; set; }
        public string TeamName { get; set; }
        public string CategoryName { get; set; }
        public string WeightDivisionName { get; set; }
        public bool IsMember { get; set; }
    }
}