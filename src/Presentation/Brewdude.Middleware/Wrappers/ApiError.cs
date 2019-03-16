using System.Collections.Generic;
using System.Linq;
using Brewdude.Common.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Brewdude.Middleware.Wrappers
{
    public class ApiError
    {
        public ApiError(string message)
        {
            ExceptionMessage = message;
            IsError = true;
        }
        
        public bool IsError { get; set; }
        public string ExceptionMessage { get; set; }
        public string Details { get; set; }
        public string ReferenceErrorCode { get; set; }
        public string ReferenceDocumentLink { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }

        public ApiError(ModelStateDictionary modelState)
        {
            IsError = true;
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                ExceptionMessage = BrewdudeConstants.ErrorValidationMessage;
                ValidationErrors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(modelError => new ValidationError(key, modelError.ErrorMessage)))
                    .ToList();

            }
        }
    }
}