using System;
using System.Collections.Generic;
using System.Net;

namespace Brewdude.Middleware.Wrappers
{
    public class ApiException : Exception
    {
        public ApiException(
            string message,
            int statusCode = (int)HttpStatusCode.InternalServerError,
            IEnumerable<ValidationError> errors = null,
            string errorCode = "",
            string referenceDocumentLink = "") :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
            ReferenceErrorCode = errorCode;
            ReferenceDocumentLink = referenceDocumentLink;
        }
        
        public ApiException(Exception exception, int statusCode = (int)HttpStatusCode.InternalServerError) 
            : base(exception.Message)
        {
            StatusCode = statusCode;
        }
        
        public int StatusCode { get; set; }
        public IEnumerable<ValidationError> Errors { get; set; }
        public string ReferenceErrorCode { get; set; }
        public string ReferenceDocumentLink { get; set; }
    }
}