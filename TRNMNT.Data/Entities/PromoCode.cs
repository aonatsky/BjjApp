using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{
    public class PromoCode
    {
        [Key]
        public Guid PromoCodeId { get; set; }
        public Guid EventId { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdateTs { get; set; }
        public string BurntBy { get; set; }

        public Event Event { get; set; }
    }
}
