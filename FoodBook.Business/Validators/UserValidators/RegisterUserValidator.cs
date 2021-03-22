using FluentValidation;
using FoodBook.Common.DTOs.UserDTOs;

namespace FoodBook.Business.Validators.UserValidators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequestDTO>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.User.Email).NotNull().MinimumLength(5);
            RuleFor(user => user.User.FirstName).NotNull().MinimumLength(2);
            RuleFor(user => user.User.Surname).NotNull().MinimumLength(2);
            RuleFor(user => user.Password).NotNull().MinimumLength(6);
        }
    }
}