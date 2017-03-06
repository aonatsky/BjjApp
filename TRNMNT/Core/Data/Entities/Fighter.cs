using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{
    public class Fighter        
    {
        [Key]
        public Guid FighterID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Guid TeamID { get; set; }
        
        

        public Team Team { get; set; }

    }
}