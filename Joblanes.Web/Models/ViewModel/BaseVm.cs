using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class BaseVm
    {
        public class ValidateDateRange : ValidationAttribute
        {
            public ValidationResult IsValid(DateTime value, ValidationContext validationContext)
            {
                // your validation logic
                if (value >= Convert.ToDateTime("01/10/1920") && value <= DateTime.MaxValue)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Date is not in given range.");
                }
            }
        }
    }
}