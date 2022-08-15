using FluentValidation;
using GuestbookApplication.Models;

namespace GuestbookApplication.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotEmpty();
            RuleFor(user => user.Password).NotEmpty().MinimumLength(3);

        }
    }
}
