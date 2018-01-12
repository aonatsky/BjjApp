using System;

namespace TRNMNT.Core.Model.FileProcessingOptions
{
    public struct ParticipantListProcessingOptions
    {
        public Guid EventId { get; set; }
        public Guid FederationId { get; set; }
    }
}