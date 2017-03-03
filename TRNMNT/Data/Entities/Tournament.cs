using System;
using System.ComponentModel.DataAnnotations;
namespace TRNMNT.Data.Entities
{

    public class Tournament
    {
        [Key]
        public Guid TournamentID { get; set; }
        public Guid OwnerID { get; set; }
        public DateTime Date { get; set; }
        public string Descritpion { get; set; }
        
        public Owner Owner { get; set; }
    }
}