using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Core.Data.Entities
{
    public class Fight
    {
        public Fight(int level, Guid? nextFightID){
            FightID = Guid.NewGuid();
            Level = level;
            IsCompleted = false;
            NextFightID = nextFightID;
        }
        
        [Key]
        public Guid FightID { get; set; }
        public int Level { get; set; }
        public Guid? NextFightID {get;set;}
        public Guid? WhiteGIFighterID {get;set;}
        public Guid? BlueGIFighterID {get;set;}
        public Guid FightListID {get;set;}
        public bool IsCompleted {get;set;}
        public Guid? WinnerID {get;set;}
        public String Result {get;set;} 
              
        
        public Fighter WhiteGIFighter { get; set; }
        public Fighter BlueGIFighter { get; set; }
        public ICollection<Fight> PreviousFights {get;set;}
        
        [ForeignKey("NextFightID")]
        public Fight NextFight {get;set;}
    }
}