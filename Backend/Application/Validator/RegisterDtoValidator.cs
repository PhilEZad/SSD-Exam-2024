using Application.DTOs.Create;
using FluentValidation;

namespace Application.Validator;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty");
        RuleFor(x => x.PlainPassword).NotEmpty().WithMessage("Password cannot be empty");
    }
}