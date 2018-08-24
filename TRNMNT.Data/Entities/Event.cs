using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{

    public class Event
    {
        [Key]
        public Guid EventId { get; set; }
        public string OwnerId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RegistrationStartTS { get; set; }
        public DateTime EarlyRegistrationEndTS { get; set; }
        public DateTime RegistrationEndTS { get; set; }
        public string ImgPath { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        public DateTime? UpdateTS { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }

        [MaxLength(100)]
        public string UrlPrefix { get; set; }
        public string Description { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
        public string TNCFilePath { get; set; }

        [MaxLength(100)]
        public string CardNumber { get; set; }

        [MaxLength(100)]
        public string ContactEmail { get; set; }

        [MaxLength(100)]
        public string ContactPhone { get; set; }

        [MaxLength(500)]
        public string FBLink { get; set; }

        [MaxLength(500)]
        public string VKLink { get; set; }
        public string AdditionalData { get; set; }
        public Guid FederationId { get; set; }
        public bool PromoCodeEnabled { get; set; }
        public string PromoCodeListPath { get; set; }
        public string PaymentType { get; set; }
        public int EarlyRegistrationPrice { get; set; }
        public int LateRegistrationPrice { get; set; }
        public int EarlyRegistrationPriceForMembers { get; set; }
        public int LateRegistrationPriceForMembers { get; set; }

        public bool CorrectionsEnabled { get; set; }
        public bool ParticipantListsPublished { get; set; }
        public bool BracketsPublished { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
        public virtual Federation Federation { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<PromoCode> PromoCodes { get; set; }
    }
}