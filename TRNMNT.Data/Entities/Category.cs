using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }
        public String Name { get; set; }
        public Guid EventID { get; set; }

        [JsonIgnore]
        public virtual Event Event {get;set;}
        public virtual ICollection<WeightDivision> WeightDivisions { get; set; }

    }
}