using FluentValidation;

namespace Brewdude.Application.UserBeers.Commands.CreateUserBeer
{
    public class CreateUserBeerCommandValidator : AbstractValidator<CreateUserBeerCommand>
    {
        public CreateUserBeerCommandValidator()
        {
            RuleFor(ub => ub.UserId)
                .NotEmpty();
            
            RuleFor(ub => ub.BeerId)
                .NotEmpty();
        }
    }
}