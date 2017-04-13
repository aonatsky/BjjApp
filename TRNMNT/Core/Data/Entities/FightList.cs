using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{
    public class FightList
    {
        [Key]
        public Guid FightListId {get;set;}
        public Guid TournamentId {get;set;}
        public Guid WeightDivisionId {get;set;}
        public Guid DivisionId {get;set;}
        public Guid AgeDivisionId {get;set;}

        public Tournament Tournament {get;set;}
        public BeltDivision BeltDivision {get;set;}
        public AgeDivision AgeDivision {get;set;}
    }
}
