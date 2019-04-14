using System;
using Brewdude.Common.Constants;
using Brewdude.Domain.Entities;
using FluentValidation.Validators;

namespace Brewdude.Application.Helpers
{
    /// <summary>
    /// Static helper class that houses all custom fluent validations for validation contexts. 
    /// </summary>
    public static class CustomValidationHandlers
    {
        public static void ValidNameHandler(string name, CustomContext context)
        {
            if (!BrewdudeConstants.ValidNameRegex.IsMatch(name))
                context.AddFailure($"{name} is not a valid name");
        }

        public static void ValidPasswordHandler(string password, CustomContext context)
        {
            if(!BrewdudeConstants.PasswordRegex.IsMatch(password))
                context.AddFailure("Password does not meet the password requirements");
        }

        public static void ValidRoleHandler(Role role, CustomContext context)
        {
            if (!Enum.IsDefined(typeof(Role), context.PropertyValue))
                context.AddFailure("User role is not valid");
        }

        public static void ValidBeerStyleHandler(BeerStyle beerStyle, CustomContext context)
        {
            if (!Enum.IsDefined(typeof(BeerStyle), context.PropertyValue))
                context.AddFailure($"{context.PropertyValue} is not a valid beer style");
        }
    }
}