namespace Brewdude.Application.Beer.Commands.UpdateBeer
{
    using FluentValidation;
    using Helpers;

    public class UpdateBeerCommandValidator : AbstractValidator<UpdateBeerCommand>
    {
        public UpdateBeerCommandValidator()
        {
            RuleFor(b => b.Name)
                .HasValidName();

            RuleFor(b => b.Description)
                .MaximumLength(128)
                .NotEmpty();

            RuleFor(b => b.BeerStyle)
                .HasValidBeerStyle();

            RuleFor(b => b.Ibu)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(b => b.Abv)
                .GreaterThanOrEqualTo(0.0)
                .LessThan(100.0)
                .NotEmpty();
        }
    }
}