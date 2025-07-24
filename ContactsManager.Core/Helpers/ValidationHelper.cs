using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        public static void ModelValidtion(object obj)
        {
            //Model validation 
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj,validationContext
                ,validationResults,true);
            if (!isValid) 
            {
                throw new ArgumentException
                    (validationResults.FirstOrDefault()?.ErrorMessage);
            }

        }
    }
}
