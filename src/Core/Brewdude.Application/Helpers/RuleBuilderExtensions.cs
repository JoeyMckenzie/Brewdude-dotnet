namespace Brewdude.Application.Helpers
{
    using System;
    using Common.Constants;
    using Domain.Entities;
    using FluentValidation;

    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> HasValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((name, context) =>
            {
                if (!BrewdudeConstants.ValidNameRegex.IsMatch(name))
                {
                    context.AddFailure($"{name} is not a valid name");
                }
            }).NotEmpty();
        }

        public static IRuleBuilderOptions<T, string> HasValidStateAbbreviation<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((stateAbbreviation, context) =>
            {
                if (!BrewdudeConstants.ValidNameRegex.IsMatch(stateAbbreviation))
                {
                    context.AddFailure($"{stateAbbreviation} is not a valid name");
                }
            }).NotEmpty();
        }

        public static IRuleBuilderOptions<T, string> HasValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((password, context) =>
            {
                if (!BrewdudeConstants.PasswordRegex.IsMatch(password))
                {
                    context.AddFailure("Password does not meet the password requirements");
                }
            }).NotEmpty();
        }

        public static IRuleBuilderOptions<T, string> HasValidWebsiteUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((website, context) =>
            {
                var isValidUri = Uri.TryCreate(website, UriKind.RelativeOrAbsolute, out _);
                if (!isValidUri)
                {
                    context.AddFailure($"{website} is not a valid brewery website URL");
                }
            }).NotEmpty();
        }

        public static IRuleBuilderOptions<T, string> HasValidStreetAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((streetAddress, context) =>
            {
                var regex = BrewdudeConstants.StreetAddressRegex;
                if (!regex.IsMatch(context.PropertyValue.ToString()))
                {
                    context.AddFailure($"{streetAddress} is not a valid street address");
                }
            }).NotEmpty();
        }

        public static IRuleBuilderOptions<T, BeerStyle> HasValidBeerStyle<T>(this IRuleBuilder<T, BeerStyle> ruleBuilder)
        {
            return ruleBuilder.Custom((beerStyle, context) =>
            {
                if (!Enum.IsDefined(typeof(BeerStyle), context.PropertyValue))
                {
                    context.AddFailure($"{context.PropertyValue} is not a valid beer style");
                }
            }).NotEmpty();
        }

        public static IRuleBuilderOptions<T, int> HasValidZipCode<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.Custom((zipCode, context) =>
            {
                var regex = BrewdudeConstants.ZipCodeRegex;
                if (!regex.IsMatch(context.PropertyValue.ToString()))
                {
                    context.AddFailure($"{zipCode} is not a valid zipcode");
                }
            }).NotEmpty();
        }

        public static IRuleBuilderOptions<T, Role> HasValidUserRole<T>(this IRuleBuilder<T, Role> ruleBuilder)
        {
            return ruleBuilder.Custom((role, context) =>
            {
                if (!Enum.IsDefined(typeof(Role), context.PropertyValue))
                {
                    context.AddFailure($"{role} is not a valid role");
                }
            }).NotEmpty();
        }
    }
}