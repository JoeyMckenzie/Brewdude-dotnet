using Brewdude.Application.Helpers;
using Brewdude.Common.Constants;
using FluentValidation;

namespace Brewdude.Application.User.Commands.UpdateUser
{
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
                .Custom(ValidationHandlers.ValidNameHandler)
                .NotEmpty()
                .MaximumLength(BrewdudeConstants.MaxNameLength);
            
            RuleFor(u => u.UpdatedLastName)
                .Custom(ValidationHandlers.ValidNameHandler)
                .NotEmpty()
                .MaximumLength(BrewdudeConstants.MaxNameLength);
        }
    }
}