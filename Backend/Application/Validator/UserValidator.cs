using Domain;
using FluentValidation;

namespace Application.Validator;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty");
        RuleFor(x => x.HashedPassword).NotEmpty().WithMessage("Password cannot be empty");
    }
}