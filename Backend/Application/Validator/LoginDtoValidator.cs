using Application.DTOs.Request;
using FluentValidation;

namespace Application.Validator;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty");
        RuleFor(x => x.Username).MinimumLength(1).WithMessage("Username must be more than 1 character");
        RuleFor(x => x.Username).MaximumLength(256).WithMessage("Username must be less than 256 characters");
        
        RuleFor(x => x.PlainPassword).NotEmpty().WithMessage("Password cannot be empty");
        RuleFor(x => x.PlainPassword).MinimumLength(10).WithMessage("Password must be at least 10 characters");
        RuleFor(x => x.PlainPassword).MaximumLength(256).WithMessage("Password must be less than 256 characters");
        RuleFor(x => x.PlainPassword).Matches(@"[^\w\s]").WithMessage("Password must contain at least one special character");
        RuleFor(x => x.PlainPassword).Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter");
        RuleFor(x => x.PlainPassword).Matches(@"\d").WithMessage("Password must contain at least 1 digit");
    }
}