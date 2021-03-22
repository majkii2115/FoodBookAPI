using FluentValidation;
using FoodBook.Common.DTOs.UserDTOs;

namespace FoodBook.Business.Validators.UserValidators
{
    public class LoginUserValidator : AbstractValidator<LoginUserRequestDTO>
    {
        public LoginUserValidator()
        {
            RuleFor(user => user.Email).NotNull().MinimumLength(5);
            RuleFor(user => user.Password).NotNull().MinimumLength(6);
        }
    }
}