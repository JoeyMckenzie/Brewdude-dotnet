using System;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    public class CreateBreweryCommandValidator : AbstractValidator<CreateBreweryCommand>
    {
        public CreateBreweryCommandValidator()
        {
            RuleFor(b => b.Name).NotEmpty().MaximumLength(32);
            RuleFor(b => b.Description).NotEmpty().MaximumLength(128);
            RuleFor(b => b.City).NotEmpty().MaximumLength(32);
            RuleFor(b => b.State).NotEmpty().Length(2);
            RuleFor(b => b.ZipCode).Custom((zipCode, context) =>
            {
                var regex = new Regex("^\\d{5}$");
                if (!regex.IsMatch(context.PropertyValue.ToString()))
                    context.AddFailure("Zip code is not valid, must be 5 digits");
            }).NotEmpty();
            RuleFor(b => b.StreetAddress).Custom((streetAddress, context) =>
            {
                // TODO: Swap out validation for an API
                var regex = new Regex("\\d{1,5}\\s(\\b\\w*\\b\\s){1,2}\\w*\\.");
                if (!regex.IsMatch(context.PropertyValue.ToString()))
                    context.AddFailure("Invalid street address");                    
            }).NotEmpty();
            RuleFor(b => b.Website).Custom((website, context) =>
            {
                Uri uri;
                var isValidUri = Uri.TryCreate(website, UriKind.RelativeOrAbsolute, out uri);
                if (!isValidUri)
                    context.AddFailure("Invalid brewery website URL");
            });
        }
    }
}