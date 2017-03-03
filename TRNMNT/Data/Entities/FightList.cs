using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TRNMNT.Data.Entities
{
    public class FightList
    {
        [Key]
        public Guid FightListID {get;set;}
        public Guid TournamentID {get;set;}
        public Guid WeightClassID {get;set;}
        public Guid DivisionID {get;set;}
        public Guid AgeClassID {get;set;}

        public Tournament Tournament {get;set;}
        public BeltClass BeltClass {get;set;}
        public AgeClass AgeClass {get;set;}
    }
}
