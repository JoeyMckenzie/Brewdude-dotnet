namespace Brewdude.Middleware.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Extensions;
    using Domain.Api;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        /// <summary>
        /// Kicks off he request pipeline while catching any exceptions thrown in the application layer.
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <returns>Handoff to next request delegate in teh pipeline</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles any exception thrown during the pipeline process and in the application layer. Note that model state
        /// validation failures made in the web layer are handled by the ASP.NET Core model state validation failure filter.
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <param name="exception">Exceptions thrown during pipeline processing</param>
        /// <returns>Writes the API response to the context to be returned in the web layer</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Declare status code, response, and errors updated based on message/status code returned from the application
            int statusCode;
            string responseMessage;
            object errors;
            var validationFailures = new List<BrewdudeApiError>();

            /*
             * Handle exceptions based on type, while defaulting to generic internal server error for unexpected exceptions.
             * Each case handles binding the API response message, API response status code, the HTTP response status code,
             * and any errors incurred in the application layer. Validation failures returned from Fluent Validation will
             * be added to the API response if there are any instances.
             */
            switch (exception)
            {
                case BrewdudeApiException brewdudeApiException:
                    responseMessage = brewdudeApiException.ResponseMessage.GetDescription();
                    statusCode = (int)brewdudeApiException.StatusCode;
                    context.Response.StatusCode = (int)brewdudeApiException.StatusCode;
                    errors = brewdudeApiException.Errors;
                    if (brewdudeApiException.ApiErrors.Any())
                    {
                        validationFailures = brewdudeApiException.ApiErrors.ToList();
                    }
                    break;
                case ValidationException validationException:
                    responseMessage = BrewdudeResponseMessage.ErrorValidation.GetDescription();
                    statusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errors = validationException.Message;
                    foreach (var validationFailure in validationException.Errors)
                    {
                        var brewdudeValidationError = new BrewdudeApiError(validationFailure.ErrorMessage)
                        {
                            ErrorCode = validationFailure.ErrorCode,
                            PropertyName = validationFailure.PropertyName
                        };
                        _logger.LogInformation("Validation failure to request @{validationFailure}", validationFailure);
                        validationFailures.Add(brewdudeValidationError);
                    }
                    break;
                default:
                    responseMessage = BrewdudeResponseMessage.InternalServerError.GetDescription();
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errors = exception.Message;
                    break;
            }

            // Instantiate the response
            context.Response.ContentType = "application/json";
            var response = new BrewdudeApiResponse(statusCode, responseMessage);

            // Set the response API error based on model state validation failures returned
            if (errors != null)
            {
                var brewdudeApiError = new BrewdudeApiError((string)errors);
                response.Errors.Add(brewdudeApiError);
            }

            // Add any Fluent Validation errors returned in the application layer to the API response
            if (validationFailures.Any())
            {
                response.Errors = response.Errors.Concat(validationFailures).ToList();
            }

            // Serialize the response and write out to the context buffer to return
            var result = JsonConvert.SerializeObject(response, BrewdudeConstants.BrewdudeJsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }
    }
}