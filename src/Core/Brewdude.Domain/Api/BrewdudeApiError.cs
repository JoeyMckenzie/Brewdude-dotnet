namespace Brewdude.Domain.Api
{
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
}