using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Brewdude.Common.Constants
{
    /// <summary>
    /// Global application constants to be used in any of the various solution projects.
    /// </summary>
    public static class BrewdudeConstants
    {
        // Meta constants
        public const string Version = "0.1.0-beta";
        public static readonly JsonSerializerSettings BrewdudeJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        
        // Response messages
        public const string SuccessfulRequestMessage = "Request was successful";
        public const string InternalServerErrorMessage = "An unexpected error has occurred, please try making the request again, or at a later time.";
        public const string ErrorValidationMessage = "One or more validation exceptions has occurred during the request.";
        public const string BadRequestMessage = "Your request could not be performed at this time, please try again.";
        public const string InvalidModelState = "The request did not meet the validtion requirements, please check the payload and submit again.";
        public const string BeerNotFoundMessage = "Could not find the requested beer in the database.";
        public const string BreweryNotFoundMessage = "Could not find the requested brewery in the database.";
        public const string UserNotFoundMessage = "Could not find the requested user in the database.";
        public const string CreatedMessage = "Entity created successfully.";
        public const string UpdatedMessage = "Entity updated successfully.";
        public const string DeletedMessage = "Entity Deleted successfully.";
        
        // Validation Constants
        public static readonly Regex PasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,20}$");
        public static readonly Regex StreetAddressRegex = new Regex("\\d{1,5}\\s(\\b\\w*\\b\\s){1,2}\\w*\\.");
        public static readonly Regex ZipCodeRegex = new Regex("^\\d{5}$");
        public static readonly Regex ValidNameRegex = new Regex("^[a-zA-Z-' ]+$");
        public const int MaxNameLength = 32;
        public const int MaxEmailLength = 32;
        public const int MaxUsernameLength = 16;
    }
}