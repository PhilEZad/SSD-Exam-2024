using Domain;
using FluentValidation;

namespace Application.Validator;

public class NoteValidator : AbstractValidator<Note>
{
    public NoteValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Id).NotNull().LessThanOrEqualTo(0).WithMessage("Id cannot be null");
        RuleFor(x => x.Id).LessThanOrEqualTo(0).WithMessage("Id cannot be 0");
        
        RuleFor(x => x.Title).NotNull().WithMessage("Title cannot be null");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        
        RuleFor(x => x.Content).NotNull().WithMessage("Content cannot be null");
    }
}