namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    using FluentValidation;
    using Helpers;

    public class CreateBreweryCommandValidator : AbstractValidator<CreateBreweryCommand>
    {
        public CreateBreweryCommandValidator()
        {
            RuleFor(b => b.Name)
                .MaximumLength(32)
                .HasValidName();

            RuleFor(b => b.Description)
                .MaximumLength(128)
                .NotEmpty();

            RuleFor(b => b.AddressDto.City)
                .HasValidStateAbbreviation()
                .MaximumLength(32);

            RuleFor(b => b.AddressDto.State)
                .HasValidStateAbbreviation()
                .Length(2);

            RuleFor(b => b.AddressDto.ZipCode)
                .HasValidZipCode();

            RuleFor(b => b.AddressDto.StreetAddress)
                .HasValidStreetAddress()
                .MaximumLength(32);

            RuleFor(b => b.Website)
                .HasValidWebsiteUrl()
                .MaximumLength(64);
        }
    }
}