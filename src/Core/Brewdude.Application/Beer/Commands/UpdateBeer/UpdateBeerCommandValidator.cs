using System;
using System.Data;
using Brewdude.Application.Helpers;
using Brewdude.Domain.Entities;
using FluentValidation;

namespace Brewdude.Application.Beer.Commands.UpdateBeer
{
    public class UpdateBeerCommandValidator : AbstractValidator<UpdateBeerCommand>
    {
        public UpdateBeerCommandValidator()
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

            RuleFor(b => b.Abv)
                .GreaterThanOrEqualTo(0.0)
                .LessThan(100.0)
                .NotEmpty();
        }
    }
}