using System.ComponentModel;

namespace Brewdude.Middleware.Wrappers
{
    public enum ResourceMessage
    {
        [Description("Request was successful")]
        Success,
        [Description("Bad request during pipepline")]
        BadRequest,
        [Description("Request responded with exceptions")]
        Exception,
        [Description("User is unauthorized to make the request")]
        Unauthorized,
        [Description("User does not have permission to make the request")]
        Forbidden,
        [Description("Request responded with validation error(s)")]
        ValidationError,
        [Description("Unable to process the request")]
        Failure
    }
}