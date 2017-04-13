using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{
    public class TournamentType
    {
        [Key]
        public Guid TournamentTypeId { get; set; }
        public String Name { get; set; }
        public string Descritpion { get; set; }

    }
}