using System;
using System.ComponentModel.DataAnnotations;
namespace TRNMNT.Core.Data.Entities
{

    public class Tournament
    {
        [Key]
        public Guid TournamentId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime Date { get; set; }
        public string Descritpion { get; set; }
        
        public Owner Owner { get; set; }
    }
}