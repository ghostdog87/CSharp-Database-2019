using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BillsPaymentSystem.Models
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }

        [Range(typeof(decimal), "0", "50000")]
        public decimal Balance { get; set; }

        public string BankName { get; set; }

        public string SWIFTCode { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
