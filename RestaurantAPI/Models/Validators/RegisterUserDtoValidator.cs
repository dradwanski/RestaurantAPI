using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.password)
                .MinimumLength(6);

            RuleFor(x => x.confirmpassword).Equal(e => e.password);

            RuleFor(x => x.email).Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "Email is taken");
                }
            });
        }
    }
}
