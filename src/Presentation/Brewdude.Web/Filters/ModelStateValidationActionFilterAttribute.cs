using System.Collections.Generic;
using System.Linq;
using System.Net;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Brewdude.Web.Filters
{
    public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!context.ModelState.IsValid)
            {
                // Retrieve all model state errors
                var modelErrors = modelState.Keys.SelectMany(key => modelState[key].Errors);
                
                // Build a list of BrewdudeApiErrors to return to the request pipeline
                var brewdudeApiErrors = modelErrors.Select(modelError => new BrewdudeApiError(modelError.ErrorMessage)).ToList();

                // Instantiate the exception
                var brewdudeApiException = new BrewdudeApiException(
                    HttpStatusCode.BadRequest, 
                    BrewdudeResponseMessage.InvalidModelState,
                    $"Invalid model state for request [{context.Controller.GetType().Name}]")
                {
                    ApiErrors = brewdudeApiErrors
                };

                // Throw the API exception to be caught during the pipeline
                throw brewdudeApiException;
            }
            
            base.OnActionExecuting(context);
        }
    }
}