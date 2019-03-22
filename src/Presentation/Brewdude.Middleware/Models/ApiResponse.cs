using System.Runtime.Serialization;
using Brewdude.Common.Constants;
using Brewdude.Middleware.Wrappers;

namespace Brewdude.Middleware.Models
{
    [DataContract]
    public class ApiResponse
    {
        [DataMember]
        public int StatusCode { get; set; }
        
        [DataMember]
        public string Message { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int ResultLength { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public object Result { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public ApiError ResponseException { get; set; }
        
        [DataMember]
        public string Version { get; set; }
        
        public ApiResponse(int statusCode, string message = "", object result = null, ApiError resourceException = null, string apiVersion = BrewdudeConstants.Version)
        {
            StatusCode = statusCode;
            Message = message;
            Result = result;
            ResponseException = resourceException;
            Version = apiVersion;
        }
    }
}