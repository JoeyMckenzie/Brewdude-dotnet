namespace Brewdude.Domain.Entities
{
    using System;
    using System.Net;

    public class BrewdudeResponse
    {
        protected BrewdudeResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            RequestId = Guid.NewGuid().ToString();
            StatusCode = (int)statusCode;
            Result = result;
            ErrorMessage = errorMessage;
        }

        public int StatusCode { get; }

        public string RequestId { get; }

        public string ErrorMessage { get; }

        public object Result { get; }
    }
}