namespace Brewdude.Domain.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class BrewdudeApiException : Exception
    {
        public BrewdudeApiException(HttpStatusCode statusCode, BrewdudeResponseMessage responseMessage, string error)
        {
            StatusCode = statusCode;
            ResponseMessage = responseMessage;
            Errors = error;
            ApiErrors = new List<BrewdudeApiError>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public BrewdudeResponseMessage ResponseMessage { get; set; }

        public string Errors { get; set; }

        public IEnumerable<BrewdudeApiError> ApiErrors { get; set; }
    }
}