using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Brewdude.Common.Constants;
using Brewdude.Common.Extensions;
using Brewdude.Middleware.Models;
using Brewdude.Middleware.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Brewdude.Middleware
{
    /// <summary>
    /// A web middleware that prepends all Brewdude API responses with meta data. This is a modified version
    /// of the RESTApiResponseWrapper.Core library by proudmonkey. Repository can be found the GitHub page <see href="https://github.com/proudmonkey/RESTApiResponseWrapper.Core">here.</see>
    /// </summary>
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ApiResponseMiddleware> _logger;
        private readonly IDictionary<int, string> _responseMessageFromStatusCode;

        public ApiResponseMiddleware(RequestDelegate requestDelegate, ILogger<ApiResponseMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _responseMessageFromStatusCode = new Dictionary<int, string>
            {
                { (int)HttpStatusCode.OK, ResourceMessage.Success.GetDescription() },
                { (int)HttpStatusCode.InternalServerError, ResourceMessage.Failure.GetDescription() },
                { (int)HttpStatusCode.Unauthorized, ResourceMessage.Unauthorized.GetDescription() },
                { (int)HttpStatusCode.Forbidden, ResourceMessage.Forbidden.GetDescription() }
            };
        }

#region ApiResponseMiddleware::Invoke
        
        /// <summary>
        /// Serves as the API middleware entry point, coordinating with the request pipeline to handle invoked responses.
        /// </summary>
        /// <param name="context">Request context passed in from the pipeline hierarchy</param>
        /// <returns>Response wrapper passed into the request pipeline</returns>
        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    context.Response.Body = memoryStream;
                    await _requestDelegate.Invoke(context);

                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        var body = await FormatResponse(context.Response);
                        await HandleSuccessfulRequestAsync(context, body, context.Response.StatusCode);
                    }
                    else
                    {
                        await HandleFailedRequestAsync(context, context.Response.StatusCode);
                    }
                }
                catch (Exception exception)
                {
                    await HandleExceptionAsync(context, exception);
                }
                finally
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(currentBody);
                }
            }
        }

#endregion

#region ApiResponseMiddleWare::HandlerMethods
        
        /// <summary>
        /// Handles a successful request passed in from the request pipeline and coordinates the construction of the response wrapper.
        /// </summary>
        /// <param name="context">Request context passed in from the pipeline hierarchy</param>
        /// <param name="body">Response body from the application layer</param>
        /// <param name="statusCode">HTTP status code of the response</param>
        /// <returns>Successfully wrapped response to proceed in the pipeline</returns>
        private static Task HandleSuccessfulRequestAsync(HttpContext context, object body, int statusCode)
        {
            context.Response.ContentType = "application/json";
            string responseJson;
            ApiResponse apiResponse;

            var bodyText = body.ToString().IsValidJson() ? body.ToString() : JsonConvert.SerializeObject(body);

            var bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            var type = bodyContent?.GetType();

            if (type != null && type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(bodyText);
                
                if (apiResponse.StatusCode != statusCode)
                {
                    responseJson = JsonConvert.SerializeObject(apiResponse);
                }
                else if (apiResponse.Result != null)
                {
                    responseJson = JsonConvert.SerializeObject(apiResponse);
                }
                else
                {
                    apiResponse = new ApiResponse(statusCode, ResourceMessage.Success.GetDescription(), bodyContent, null);
                    responseJson = JsonConvert.SerializeObject(apiResponse);
                }
            }
            else
            {
                apiResponse = new ApiResponse(statusCode, ResourceMessage.Success.GetDescription(), bodyContent, null);
                responseJson = JsonConvert.SerializeObject(apiResponse);
            }

            return context.Response.WriteAsync(responseJson);
        }

        /// <summary>
        /// Handles failed request passed in from the pipeline and coordinates the construction of the failed response wrapper.
        /// </summary>
        /// <param name="context">Response context passed in from the pipeline hierarchy</param>
        /// <param name="statusCode">HTTP status of the failed response</param>
        /// <returns>Wrapped response to proceed in the pipeline</returns>
        private Task HandleFailedRequestAsync(HttpContext context, int statusCode)
        {
            context.Response.ContentType = "application/json";

            ApiError apiError;

            switch (statusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    apiError = new ApiError("The specified URI was not found based on the request context. Please verify and try again.");
                    break;
                case (int)HttpStatusCode.NoContent:
                    apiError = new ApiError("The specified URI does not contain any content.");
                    break;
                case (int)HttpStatusCode.Unauthorized:
                    apiError = new ApiError($"The request from user ID is unauthorized to access {context.Request.Path}");
                    break;
                case (int)HttpStatusCode.Forbidden:
                    apiError = new ApiError($"The request from user ID {context.User.Identity.Name} does not have access for path {context.Request.Path}");
                    break;
                default:
                    apiError = new ApiError(BrewdudeConstants.InternalServerErrorMessage);
                    break;
            }

            var responseMessage = _responseMessageFromStatusCode.ContainsKey(statusCode) ? _responseMessageFromStatusCode[statusCode] : ResourceMessage.Failure.GetDescription();
            var apiResponse = new ApiResponse(statusCode, responseMessage, null, apiError);
            context.Response.StatusCode = statusCode;
            var json = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(json);
        }
        
        /// <summary>
        /// Handles any exceptions incurred during the pipeline invocation process. 
        /// </summary>
        /// <param name="context">Response context from the failed pipeline hierarchy</param>
        /// <param name="exception">Exception produced during the pipeline process</param>
        /// <returns>Wrapped response to proceed in the pipeline</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiError apiError;
            int statusCode;

            if (exception is ApiException apiException)
            {
                apiError = new ApiError(apiException.Message)
                {
                    ValidationErrors = apiException.Errors,
                    ReferenceErrorCode = apiException.ReferenceErrorCode,
                    ReferenceDocumentLink = apiException.ReferenceDocumentLink
                };
                statusCode = apiException.StatusCode;
                context.Response.StatusCode = statusCode;
            }
            else if (exception is UnauthorizedAccessException unauthorizedAccessException)
            {
                apiError = new ApiError(unauthorizedAccessException.Message);
                statusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.StatusCode = statusCode;
            }
            else
            {
#if !DEBUG
                var message = "An unhandled error occurred.";
                var stackTrace = null;
#else
                var message = exception.GetBaseException().Message;
                var stackTrace = exception.StackTrace;
#endif

                apiError = new ApiError(message)
                {
                    Details = stackTrace
                };
                statusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusCode = statusCode;
            }

            context.Response.ContentType = "application/json";
            var apiResponse = new ApiResponse(statusCode, ResourceMessage.Exception.GetDescription(), null, apiError);
            var json = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(json);
        }
    
#endregion
        
        /// <summary>
        /// Formats a successful (HTTP status code = 200) response from the response context to be passed to the response handling method.
        /// </summary>
        /// <param name="response">Successful context response</param>
        /// <returns>String converted stream body</returns>
        private async Task<string> FormatResponse(HttpResponse response)
        {
            var plainBodyText = string.Empty;

            try
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not format response during API middleware wrapping. Reason: {exception.Message}");
            }
            
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }
    }
}