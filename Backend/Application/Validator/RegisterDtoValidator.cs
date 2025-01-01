using Application.DTOs.Create;
using FluentValidation;

namespace Application.Validator;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty")
            .MinimumLength(6).WithMessage("Username must be at least 3 characters long")
            .MaximumLength(28).WithMessage("Username must be at most 28 characters long");;
        RuleFor(x => x.PlainPassword).NotEmpty().WithMessage("Password cannot be empty");
    }
}