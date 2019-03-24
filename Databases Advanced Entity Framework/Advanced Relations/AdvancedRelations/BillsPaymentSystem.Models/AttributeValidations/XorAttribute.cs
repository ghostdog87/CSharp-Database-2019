using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BillsPaymentSystem.Models.AttributeValidations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XorAttribute : ValidationAttribute
    {
        private string TargetAttribute { get; set; }

        public XorAttribute(string targetAttribute)
        {
            this.TargetAttribute = targetAttribute;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var targetValue = validationContext
                .ObjectType
                .GetProperty(this.TargetAttribute)
                .GetValue(validationContext.ObjectInstance);

            if ((value == null && targetValue == null)
                || (value != null && targetValue != null))
            {
                string errorMessage = "The two properties must have opposite values!";

                return new ValidationResult(errorMessage);
            }
            return ValidationResult.Success;
            
        }
    }
}
