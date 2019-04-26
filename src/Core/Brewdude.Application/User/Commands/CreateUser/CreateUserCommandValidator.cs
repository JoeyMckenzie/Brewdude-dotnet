namespace Brewdude.Application.User.Commands.CreateUser
{
    using Common.Constants;
    using FluentValidation;
    using Helpers;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .NotEmpty()
                .MaximumLength(BrewdudeConstants.MaxEmailLength);

            RuleFor(u => u.FirstName)
                .MaximumLength(BrewdudeConstants.MaxNameLength)
                .HasValidName();

            RuleFor(u => u.LastName)
                .MaximumLength(BrewdudeConstants.MaxNameLength)
                .HasValidName();

            RuleFor(u => u.Password)
                .HasValidPassword();

            RuleFor(u => u.Role)
                .HasValidUserRole();
        }
    }
}