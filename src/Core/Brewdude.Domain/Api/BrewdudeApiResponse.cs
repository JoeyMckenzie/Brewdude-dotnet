using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Brewdude.Common.Constants;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Brewdude.Domain.Api
{
    public class BrewdudeApiResponse
    {
        public BrewdudeApiResponse(int statusCode, string responseMessage)
        {
            Errors = new List<BrewdudeApiError>();
            Version = BrewdudeConstants.Version;
            StatusCode = statusCode;
            Message = responseMessage;
        }
        
        [JsonProperty(Order = 1)]
        public bool Success => !Errors.Any();
        
        [JsonProperty(Order = 2)]
        public string Message { get; set; }
        
        [JsonProperty(Order = 3)]
        public List<BrewdudeApiError> Errors { get; set; }
        
        [JsonProperty(Order = 4)]
        public int StatusCode { get; set; }
        
        [JsonProperty(Order = 5)]
        public string Version { get; }
    }

    public class BrewdudeApiResponse<T> : BrewdudeApiResponse
    {
        public BrewdudeApiResponse(int statusCode, string responseMessage, T result, int? length)
            : base(statusCode, responseMessage)
        {
            Result = result;
            Length = length ?? 1;
        }
        
        [JsonProperty(Order = 6)]
        public int Length { get; }

        [JsonProperty(Order = 7)]
        public T Result { get; }
        
    }

    public class BrewdudeApiError
    {
        public BrewdudeApiError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        
        public BrewdudeApiError(string errorMessage, string errorCode, string propertyName)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            PropertyName = propertyName;
        }
        
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string PropertyName { get; set; }
    }

    public enum BrewdudeResponseMessage
    {
        [Description(BrewdudeConstants.SuccessfulRequestMessage)]
        Success,
        [Description(BrewdudeConstants.ErrorValidationMessage)]
        ErrorValidation,
        [Description(BrewdudeConstants.BadRequestMessage)]
        BadRequest,
        [Description(BrewdudeConstants.InvalidModelState)]
        InvalidModelState,
        [Description(BrewdudeConstants.BeerNotFoundMessage)]
        BeerNotFound,
        [Description(BrewdudeConstants.BreweryNotFoundMessage)]
        BreweryNotFound,
        [Description(BrewdudeConstants.UserNotFoundMessage)]
        UserNotFound,
        [Description(BrewdudeConstants.InternalServerErrorMessage)]
        InternalServerError,
        [Description(BrewdudeConstants.CreatedMessage)]
        Created,
        [Description(BrewdudeConstants.DeletedMessage)]
        Deleted,
        [Description(BrewdudeConstants.UpdatedMessage)]
        Updated
    }
}