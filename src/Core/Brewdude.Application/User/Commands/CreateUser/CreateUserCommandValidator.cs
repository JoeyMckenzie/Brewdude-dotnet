using Brewdude.Application.Helpers;
using Brewdude.Common.Constants;
using FluentValidation;

namespace Brewdude.Application.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .NotEmpty()
                .MaximumLength(BrewdudeConstants.MaxEmailLength);
            
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .MaximumLength(BrewdudeConstants.MaxNameLength)
                .Custom(CustomValidationHandlers.ValidNameHandler);
            
            RuleFor(u => u.LastName)
                .NotEmpty()
                .MaximumLength(BrewdudeConstants.MaxNameLength)
                .Custom(CustomValidationHandlers.ValidNameHandler);
            
            RuleFor(u => u.Password)
                .Custom(CustomValidationHandlers.ValidPasswordHandler)
                .NotEmpty();
            
            RuleFor(u => u.Role)
                .Custom(CustomValidationHandlers.ValidRoleHandler)
                .NotEmpty();
        }
    }
}