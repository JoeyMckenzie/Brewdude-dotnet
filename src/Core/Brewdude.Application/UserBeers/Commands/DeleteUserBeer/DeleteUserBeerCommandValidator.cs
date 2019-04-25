namespace Brewdude.Application.UserBeers.Commands.DeleteUserBeer
{
    using FluentValidation;

    public class DeleteUserBeerCommandValidator : AbstractValidator<DeleteUserBeerCommand>
    {
        public DeleteUserBeerCommandValidator()
        {
            RuleFor(ub => ub.BeerId)
                .NotEmpty();

            RuleFor(ub => ub.UserId)
                .NotEmpty();
        }
    }
}