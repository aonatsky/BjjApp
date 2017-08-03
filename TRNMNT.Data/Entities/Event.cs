using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{

    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EventId { get; set; }

        public string OwnerId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RegistrationStartTS { get; set; }
        public DateTime RegistrationEndTS { get; set; }
        public string ImgPath { get; set; }
        public string Title { get; set; }
        public DateTime UpdateTS { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public string UrlPrefix { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string TNCFilePath { get; set; }
        public string CardNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string FBLink { get; set; }
        public string VKLink { get; set; }
        public string AdditionalData { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}