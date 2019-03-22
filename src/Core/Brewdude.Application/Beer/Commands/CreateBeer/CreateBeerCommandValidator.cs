using System;
using Brewdude.Domain.Entities;
using FluentValidation;

namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    public class CreateBeerCommandValidator : AbstractValidator<CreateBeerCommand>
    {
        public CreateBeerCommandValidator()
        {
            RuleFor(b => b.Name).MaximumLength(32).NotEmpty();
            RuleFor(b => b.Description).MaximumLength(128).NotEmpty();
            RuleFor(b => b.BeerStyle).Custom((beerStyle, context) =>
            {
                if (!Enum.IsDefined(typeof(BeerStyle), context.PropertyValue))
                    context.AddFailure($"{context.PropertyValue} is not a valid beer style");
            }).NotEmpty();
            RuleFor(b => b.Ibu).GreaterThan(0).LessThan(256).NotEmpty();
            RuleFor(b => b.Abv).NotEmpty().GreaterThanOrEqualTo(0.0).LessThan(100.0).NotEmpty();
            RuleFor(b => b.BreweryId).NotEmpty();
        }
    }
}