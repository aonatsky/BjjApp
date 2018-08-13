using System;

namespace TRNMNT.Core.Model.Federation
{
    public class FederationModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MembershipPrice { get; set; }
        public int TeamRegistrationPrice { get; set; }
        public int CommissionPercentage { get; set; }
        public int MinCommission { get; set; }
        public string ContactInformation { get; set; }
        public string ImgPath { get; set; }
    }
}