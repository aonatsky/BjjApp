
using System;

namespace TRNMNT.Core.Model
{
    public class FighterModel
    {
        public Guid FighterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string WeightDivision { get; set; }
        public string Team { get; set; }
        public string Category { get; set; }
        public string Region { get; set; }
    }

}