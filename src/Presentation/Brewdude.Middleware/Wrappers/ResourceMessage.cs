using System.ComponentModel;

namespace Brewdude.Middleware.Wrappers
{
    public enum ResourceMessage
    {
        [Description("Request successful")]
        Success,
        [Description("Request responded with exceptions")]
        Exception,
        [Description("Request denied")]
        Unauthorized,
        [Description("Access denied")]
        Forbidden,
        [Description("Request responded with validation error(s)")]
        ValidationError,
        [Description("Unable to process the request")]
        Failure
    }
}