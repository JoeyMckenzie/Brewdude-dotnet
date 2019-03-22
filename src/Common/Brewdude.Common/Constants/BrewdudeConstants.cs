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
        public static readonly Regex PasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,20}$");
        public static readonly Regex StreetAddressRegex = new Regex("\\d{1,5}\\s(\\b\\w*\\b\\s){1,2}\\w*\\.");
        public static readonly Regex ZipCodeRegex = new Regex("^\\d{5}$");
        public static readonly JsonSerializerSettings BrewdudeJsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        public const string Version = "0.1.0";
        public const string InternalServerErrorMessage = "An unexpected error has occurred, please try making the request again, or at a later time.";
        public const string ErrorValidationMessage = "One or more validation exceptions has occurred during the request.";
    }
}