using System.Collections.Generic;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Model
{
    public class FighterFilterModel 
    {
        public  ICollection<Category> Categories {get;set;}
        public  ICollection<WeightDivision> WeightDivisions {get;set;}
    }
   
}