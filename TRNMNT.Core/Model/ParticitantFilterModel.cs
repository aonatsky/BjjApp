using System;
using System.Collections.Generic;

namespace TRNMNT.Core.Model
{
    public class ParticitantFilterModel 
    {
        public  ICollection<Guid> CategoryIds {get;set;}
        public  ICollection<Guid> WeightDivisionIds {get;set;}
    }
   
}