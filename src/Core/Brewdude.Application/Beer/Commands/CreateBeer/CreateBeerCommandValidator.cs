namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    using FluentValidation;
    using Helpers;

    public class CreateBeerCommandValidator : AbstractValidator<CreateBeerCommand>
    {
        public CreateBeerCommandValidator()
        {
            RuleFor(b => b.Name)
                .MaximumLength(32)
                .HasValidName();

            RuleFor(b => b.Description)
                .MaximumLength(128)
                .NotEmpty();

            RuleFor(b => b.BeerStyle)
                .HasValidBeerStyle();

            RuleFor(b => b.Ibu)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(b => b.Abv).NotEmpty()
                .GreaterThanOrEqualTo(0.0)
                .LessThan(100.0)
                .NotEmpty();

            RuleFor(b => b.BreweryId)
                .NotEmpty();
        }
    }
}