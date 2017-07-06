using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{
    public class Fighter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FighterId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth {get;set;}
        public Guid TeamId { get; set; }
        public Guid CategoryId{get;set;}
        public Guid WeightDivisionId{get;set;}
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public DateTime UpdateTs { get; set; }

        public Team Team { get; set; }
        public virtual Category Category { get; set; }
        public virtual WeightDivision WeightDivision { get; set; }
        

    }
}