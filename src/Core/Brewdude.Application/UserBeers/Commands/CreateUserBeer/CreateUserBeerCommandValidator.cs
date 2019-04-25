namespace Brewdude.Application.UserBeers.Commands.CreateUserBeer
{
    using FluentValidation;

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