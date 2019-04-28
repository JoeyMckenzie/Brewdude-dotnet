namespace Brewdude.Application.UserBreweries.Commands.DeleteUserBrewery
{
    using FluentValidation;

    public class DeleteUserBreweryCommandValidator : AbstractValidator<DeleteUserBreweryCommand>
    {
        public DeleteUserBreweryCommandValidator()
        {
            RuleFor(ub => ub.BreweryId)
                .NotEmpty();

            RuleFor(ub => ub.UserId)
                .NotEmpty();
        }
    }
}