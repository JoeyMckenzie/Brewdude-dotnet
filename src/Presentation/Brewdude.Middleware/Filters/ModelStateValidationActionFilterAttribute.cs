namespace Brewdude.Middleware.Filters
{
    using System.Linq;
    using System.Net;
    using Domain.Api;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        /// <summary>
        /// Overrides the default model state validation from ASP.NET Core, passing any validation failures on request
        /// to API exceptions thrown here so they may be handled manually during the request pipeline.
        /// </summary>
        /// <param name="context">HTTP context passed from the web layer</param>
        /// <exception cref="T:Brewdude.Domain.BrewdudeApiException">API exception handled by the pipeline</exception>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                // Retrieve all model state errors
                var modelErrors = modelState.Keys.SelectMany(key => modelState[key].Errors);

                // Build a list of BrewdudeApiErrors to return to the request pipeline
                var brewdudeApiErrors = modelErrors.Select(modelError => new BrewdudeApiError(modelError.ErrorMessage)).ToList();

                // Instantiate the exception
                var brewdudeApiException = new BrewdudeApiException(
                    HttpStatusCode.BadRequest, 
                    BrewdudeResponseMessage.InvalidModelState,
                    $"Invalid model state during request to [{context.Controller.GetType().Name}]. Trace ID: [{context.HttpContext.TraceIdentifier}]")
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