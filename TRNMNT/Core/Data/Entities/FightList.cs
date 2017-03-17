using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{
    public class FightList
    {
        [Key]
        public Guid FightListID {get;set;}
        public Guid TournamentID {get;set;}
        public Guid WeightDivisionID {get;set;}
        public Guid DivisionID {get;set;}
        public Guid AgeDivisionID {get;set;}

        public Tournament Tournament {get;set;}
        public BeltDivision BeltDivision {get;set;}
        public AgeDivision AgeDivision {get;set;}
    }
}
