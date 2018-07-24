using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public int OrderTypeId { get; set; }
        public string Status { get; set; }
        public int Amount { get; set; }
        public string PaymentProviderReference { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTS { get; set; }
        public DateTime UpdateTS { get; set; }

        public virtual User User { get; set; }

    }
}