namespace Brewdude.Application.User.Queries.GetUserByUsername
{
    using FluentValidation;

    public class GetUserByUsernameCommandValidator : AbstractValidator<GetUserByUsernameCommand>
    {
        public GetUserByUsernameCommandValidator()
        {
            RuleFor(r => r.Username)
                .NotEmpty();

            RuleFor(r => r.Password)
                .NotEmpty();
        }
    }
}