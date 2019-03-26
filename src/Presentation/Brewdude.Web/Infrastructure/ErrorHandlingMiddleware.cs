using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Brewdude.Common.Constants;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Brewdude.Web.Infrastructure
{
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
        /// Kicks off he request pipeline while catching any exceptions thrown in the application layer
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <returns></returns>
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
        /// Handles any exception thrown during the pipeline process and in the application layer.
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <param name="exception">Exceptions thrown during pipeline processing</param>
        /// <returns>Writes the API response to the context to be returned in the web layer</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Initialize errors to pass into the API error response
            object errors = null;
            var validationFailures = new List<BrewdudeApiError>();
            
            // Declare our status code and response, update based on message/status code returned from application layer 
            int statusCode;
            string responseMessage;

            switch (exception)
            {
                case BrewdudeApiException brewdudeApiException:
                    errors = brewdudeApiException.Errors;
                    responseMessage = brewdudeApiException.ResponseMessage.GetDescription();
                    statusCode = (int)brewdudeApiException.StatusCode;
                    context.Response.StatusCode = (int)brewdudeApiException.StatusCode;
                    if (brewdudeApiException.ApiErrors.Any())
                    {
                        validationFailures = brewdudeApiException.ApiErrors.ToList();
                    }
                    break;
                case ValidationException validationException:
                    responseMessage = BrewdudeResponseMessage.ErrorValidation.GetDescription();
                    statusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
                    errors = string.IsNullOrWhiteSpace(exception.Message) ? "Error" : exception.Message;
                    responseMessage = BrewdudeResponseMessage.InternalServerError.GetDescription();
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            
            // Instantiate the response
            context.Response.ContentType = "application/json";
            var response = new BrewdudeApiResponse(statusCode, responseMessage);

            // Set the response API error based on validation failures returned
            if (errors != null && !validationFailures.Any())
            {
                var brewdudeApiError = new BrewdudeApiError((string)errors);
                response.Errors.Add(brewdudeApiError);
            }
            else
            {
                response.Errors = validationFailures;
            }

            var result = JsonConvert.SerializeObject(response, BrewdudeConstants.BrewdudeJsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }

        private static bool ContainsDuplicateValidationFailure(IEnumerable<ValidationFailure> apiResponse, ValidationFailure validationFailure)
        {
            // Cast to list to avoid multiple enumeration
            var validationFailures = apiResponse.ToList();
            return validationFailures.Exists(vf => vf.ErrorCode == validationFailure.ErrorCode) &&
                   validationFailures.Exists(vf => vf.PropertyName == validationFailure.PropertyName);
        }
    }
}