using System;
using System.Collections.Generic;
using System.Net;
using Brewdude.Domain.Api;

namespace Brewdude.Domain
{
    public class BrewdudeApiException : Exception
    {
        public BrewdudeApiException(HttpStatusCode statusCode, BrewdudeResponseMessage responseMessage, string error)
        {
            StatusCode = statusCode;
            ResponseMessage = responseMessage;
            Errors = error;
        }

        public HttpStatusCode StatusCode { get; set; }
        public BrewdudeResponseMessage ResponseMessage { get; set; }
        public string Errors { get; set; }
        public IEnumerable<BrewdudeApiError> ApiErrors { get; set; }
    }
}