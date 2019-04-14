using System;
using Brewdude.Application.Helpers;
using Brewdude.Domain.Entities;
using FluentValidation;

namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    public class CreateBeerCommandValidator : AbstractValidator<CreateBeerCommand>
    {
        public CreateBeerCommandValidator()
        {
            RuleFor(b => b.Name)
                .MaximumLength(32)
                .NotEmpty();
            
            RuleFor(b => b.Description)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(b => b.BeerStyle)
                .Custom(CustomValidationHandlers.ValidBeerStyleHandler)
                .NotEmpty();


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