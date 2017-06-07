using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{
    public class Fight
    {
        public Fight(int level, Guid? nextFightID){
            FightId = Guid.NewGuid();
            Level = level;
            IsCompleted = false;
            NextFightId = nextFightID;
        }
        
        [Key]
        public Guid FightId { get; set; }
        public int Level { get; set; }
        public Guid? NextFightId {get;set;}
        public Guid? WhiteGIFighterId {get;set;}
        public Guid? BlueGIFighterId {get;set;}
        public Guid FightListId {get;set;}
        public bool IsCompleted {get;set;}
        public Guid? WinnerId {get;set;}
        public String Result {get;set;} 
              
        
        public Fighter WhiteGIFighter { get; set; }
        public Fighter BlueGIFighter { get; set; }
        public ICollection<Fight> PreviousFights {get;set;}
        
        [ForeignKey("NextFightID")]
        public Fight NextFight {get;set;}
    }
}