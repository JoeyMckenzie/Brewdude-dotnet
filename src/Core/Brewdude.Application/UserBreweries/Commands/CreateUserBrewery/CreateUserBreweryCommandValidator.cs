namespace Brewdude.Application.UserBreweries.Commands.CreateUserBrewery
{
    using FluentValidation;

    public class CreateUserBreweryCommandValidator : AbstractValidator<CreateUserBreweryCommand>
    {
        public CreateUserBreweryCommandValidator()
        {
            RuleFor(ub => ub.BreweryId)
                .NotEmpty();

            RuleFor(ub => ub.UserId)
                .NotEmpty();
        }
    }
}