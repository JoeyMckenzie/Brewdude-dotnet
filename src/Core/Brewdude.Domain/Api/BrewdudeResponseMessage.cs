namespace Brewdude.Domain.Api
{
    using System.ComponentModel;
    using Common.Constants;

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