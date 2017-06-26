using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{

    public class Event
    {
        [Key]
        public Guid EventId { get; set; }

        public Guid OwnerId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RegistrationStartTS { get; set; }
        public DateTime RegistrationEndTS { get; set; }
        public string ImgPath { get; set; }
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public string Address { get; set; }
        public DateTime UpdateTS { get; set; }


        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
    }
}