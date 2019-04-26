namespace Brewdude.Application.User.Commands.UpdateUser
{
    using Common.Constants;
    using FluentValidation;
    using Helpers;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(u => u.UpdatedEmail)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.UpdatedFirstName)
                .MaximumLength(BrewdudeConstants.MaxNameLength)
                .HasValidName();

            RuleFor(u => u.UpdatedLastName)
                .MaximumLength(BrewdudeConstants.MaxNameLength)
                .HasValidName();
        }
    }
}