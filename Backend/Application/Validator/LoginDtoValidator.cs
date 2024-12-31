using Application.DTOs.Request;
using FluentValidation;

namespace Application.Validator;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty");
        RuleFor(x => x.PlainPassword).NotEmpty().WithMessage("Password cannot be empty");
    }
}