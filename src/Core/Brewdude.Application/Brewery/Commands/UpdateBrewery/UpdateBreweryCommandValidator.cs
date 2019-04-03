using System;
using Brewdude.Common.Constants;
using FluentValidation;

namespace Brewdude.Application.Brewery.Commands.UpdateBrewery
{
    public class UpdateBreweryCommandValidator : AbstractValidator<UpdateBreweryCommand>
    {
        public UpdateBreweryCommandValidator()
        {
            RuleFor(b => b.Name).NotEmpty().MaximumLength(32);
            RuleFor(b => b.Description).NotEmpty().MaximumLength(128);
            RuleFor(b => b.AddressDto.City).NotEmpty().MaximumLength(32);
            RuleFor(b => b.AddressDto.State).NotEmpty().Length(2);
            RuleFor(b => b.AddressDto.ZipCode).Custom((zipCode, context) =>
            {
                var regex = BrewdudeConstants.ZipCodeRegex;
                if (!regex.IsMatch(context.PropertyValue.ToString()))
                    context.AddFailure("Zip code is not valid, must be 5 digits");
            }).NotEmpty();
            RuleFor(b => b.AddressDto.StreetAddress).Custom((streetAddress, context) =>
            {
                // TODO: Swap out validation for an API
                var regex = BrewdudeConstants.StreetAddressRegex;
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