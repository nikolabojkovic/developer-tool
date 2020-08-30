using FluentValidation;
using System.Text.RegularExpressions;

namespace WebApi.InputModels
{
    public class RegisterValidator : AbstractValidator<RegisterInputModel>
    {
        public RegisterValidator()
        {
            RuleFor(m => m.Username).Must(u => u.Length >= 3)
                                    .WithMessage("Username should be at least 3 characters long.");
            // RuleFor(m => m.Username).Must(u => Regex.IsMatch(u, @"^[a-zA-Z0-9]{3,}$"))
            //                        .WithMessage("Username should contain only alphanumeric values.");
            RuleFor(m => m.Password).Must(p => p.Length >= 8)
                                    .WithMessage("Password should be at least 8 characters long.");
            RuleFor(m => m.Password).Must(p => Regex.IsMatch(p, @"(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])"))
                                    .WithMessage("Password must contain at least lovercase and upercase letter and number.");
            // RuleFor(m => m.FirstName).Must(f => Regex.IsMatch(f, @"^[a-zA-Z0-9]{1,50}\s{0,}$"))
            //                         .WithMessage("First name have to be alphanumeric string at least 1 character and at most 50 characters long.");
            RuleFor(m => m.LastName).Must(l => Regex.IsMatch(l, @"^[a-zA-Z0-9]{0,100}$"))
                                    .WithMessage("First name have to be alphanumeric string at least 1 character and at most 100 characters long.");
        }
    }
}