using System;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ExpirationDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateValue = (DateTime)value;

            if (dateValue > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}