using System;
using System.Collections.Generic;

namespace TRNMNT.Web.Core.Model
{
    public class FighterFilterModel 
    {
        public  ICollection<Guid> CategoryIds {get;set;}
        public  ICollection<Guid> WeightDivisionIds {get;set;}
    }
   
}