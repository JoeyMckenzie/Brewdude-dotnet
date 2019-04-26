namespace Brewdude.Application.Helpers
{
    using System;
    using Common.Constants;
    using Domain.Entities;
    using FluentValidation.Validators;

    /// <summary>
    /// Helper class that houses all custom fluent validations for validation contexts.
    /// </summary>
    public static class CustomValidationHandlers
    {
        public static readonly Action<string, CustomContext> ValidPasswordHandler = (password, context) =>
        {
            if (!BrewdudeConstants.PasswordRegex.IsMatch(password))
            {
                context.AddFailure("Password does not meet the password requirements");
            }
        };

        public static readonly Action<Role, CustomContext> ValidRoleHandler = (role, context) =>
        {
            if (!Enum.IsDefined(typeof(Role), context.PropertyValue))
            {
                context.AddFailure($"{role} is not a valid role");
            }
        };

        public static readonly Action<int, CustomContext> ValidZipCode = (zipCode, context) =>
        {
            var regex = BrewdudeConstants.ZipCodeRegex;
            if (!regex.IsMatch(context.PropertyValue.ToString()))
            {
                context.AddFailure($"{zipCode} is not a valid zipcode");
            }
        };

        public static readonly Action<BeerStyle, CustomContext> ValidBeerStyleHandler = (beerStyle, context) =>
        {
            if (!Enum.IsDefined(typeof(BeerStyle), context.PropertyValue))
            {
                context.AddFailure($"{context.PropertyValue} is not a valid beer style");
            }
        };

        public static readonly Action<string, CustomContext> ValidStreetAddress = (streetAddress, context) =>
        {
            var regex = BrewdudeConstants.StreetAddressRegex;
            if (!regex.IsMatch(context.PropertyValue.ToString()))
            {
                context.AddFailure($"{streetAddress} is not a valid street address");
            }
        };

        public static readonly Action<string, CustomContext> ValidWebsite = (website, context) =>
        {
            var isValidUri = Uri.TryCreate(website, UriKind.RelativeOrAbsolute, out _);
            if (!isValidUri)
            {
                context.AddFailure($"{website} is not a valid brewery website URL");
            }
        };
    }
}