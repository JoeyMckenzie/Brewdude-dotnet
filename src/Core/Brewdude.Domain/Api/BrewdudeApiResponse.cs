namespace Brewdude.Domain.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Constants;
    using Newtonsoft.Json;

    /// <summary>
    /// Formatted general API response for every Brewdude request, consisting of
    /// various metadata about the response and request specific results.
    /// </summary>
    public class BrewdudeApiResponse
    {
        public BrewdudeApiResponse(int statusCode, string responseMessage)
        {
            Errors = new List<BrewdudeApiError>();
            Version = BrewdudeConstants.Version;
            StatusCode = statusCode;
            Message = responseMessage;
        }

        /// <summary>
        /// Gets a value indicating whether indicates a successful response, void of any validation errors.
        /// </summary>
        [JsonProperty(Order = 1)]
        public bool Success => !Errors.Any();

        /// <summary>
        /// Gets or sets response message corresponding to the request type.
        /// </summary>
        [JsonProperty(Order = 2)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets any API errors that occur within a specific application layer.
        /// </summary>
        [JsonProperty(Order = 3)]
        public List<BrewdudeApiError> Errors { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code of the response.
        /// </summary>
        [JsonProperty(Order = 4)]
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets the current Brewdude API semantic version.
        /// </summary>
        [JsonProperty(Order = 5)]
        public string Version { get; }
    }

    /// <summary>
    /// Formatted type specific response for particular Brewdude requests, similar to the generic response object.
    /// </summary>
    /// <typeparam name="TResponse">Response type object associated with the request.</typeparam>
    public class BrewdudeApiResponse<TResponse> : BrewdudeApiResponse
    {
        public BrewdudeApiResponse(int statusCode, string responseMessage, TResponse result, int? length)
            : base(statusCode, responseMessage)
        {
            Result = result;
            Length = length ?? 1;
        }

        /// <summary>
        /// Gets the result length of the response object.
        /// </summary>
        [JsonProperty(Order = 6)]
        public int Length { get; }

        /// <summary>
        /// Gets the object results from a valid request.
        /// </summary>
        [JsonProperty(Order = 7)]
        public TResponse Result { get; }
    }
}