using System;
using System.Net;

namespace Brewdude.Domain.Entities
{
    public class BrewdudeResponse
    {
        public static BrewdudeResponse Create(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            return new BrewdudeResponse(statusCode, result, errorMessage);
        }
        
        protected BrewdudeResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            RequestId = Guid.NewGuid().ToString();
            StatusCode = (int)statusCode;
            Result = result;
            ErrorMessage = errorMessage;
        }

        public int StatusCode { get; set; }
        public string RequestId { get; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}