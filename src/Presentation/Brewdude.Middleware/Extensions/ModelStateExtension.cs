using System.Collections.Generic;
using System.Linq;
using Brewdude.Middleware.Wrappers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Brewdude.Middleware.Extensions
{
    public static class ModelStateExtension
    {
        /// <summary>
        /// Extension method to retrieve a list of all validation errors passed in from the model state.
        /// </summary>
        /// <param name="modelStateDictionary">Model state used within the controller</param>
        /// <returns>Enumerable list of validation errors</returns>
        public static IEnumerable<ValidationError> GetAllValidationErrors(this ModelStateDictionary modelStateDictionary)
        {
            var validationErrors = new List<ValidationError>();
            var fieldsContainingValidationErrors = modelStateDictionary.Where(ms => ms.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

            foreach (var field in fieldsContainingValidationErrors)
            {
                var fieldKey = field.Key;
                var fieldErrors = field.Errors
                    .Select(error => new ValidationError(fieldKey, error.ErrorMessage));
                validationErrors.AddRange(fieldErrors);
            }

            return validationErrors;
        }
    }
}