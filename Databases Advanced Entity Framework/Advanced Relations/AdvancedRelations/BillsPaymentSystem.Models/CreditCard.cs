using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BillsPaymentSystem.Models
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }

        [Range(typeof(decimal),"0","50000")]
        public decimal Limit { get; set; }

        [Range(typeof(decimal), "0", "50000")]
        public decimal MoneyOwed { get; set; }

        [Range(typeof(decimal), "0", "50000")]
        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        [Required]
        [ExpirationDate]
        public DateTime ExpirationDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }   
}
