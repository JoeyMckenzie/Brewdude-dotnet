using System;
using System.Text.RegularExpressions;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Domain.Entities;
using FluentValidation;

namespace Brewdude.Application.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email).EmailAddress().NotEmpty();
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.LastName).NotEmpty();
            RuleFor(u => u.Password).Custom((password, context) =>
            {
                var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,20}$");
                if(!passwordRegex.IsMatch(password))
                    context.AddFailure("Password does not meet the password requirements");
            }).NotEmpty();
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.Role).Custom((role, context) =>
            {
                if (!Enum.IsDefined(typeof(Role), context.PropertyValue))
                    context.AddFailure("User role is not valid");
            }).NotEmpty();
        }
    }
}