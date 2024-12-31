using Application.DTOs.Create;
using FluentValidation;

namespace Application.Validator;

public class CreateNoteValidator : AbstractValidator<NoteCreate>
{
    public CreateNoteValidator()
    {
        RuleFor(n => n.Title).NotNull().WithMessage("Title is null");
        RuleFor(n => n.Content).NotNull().WithMessage("Content is null");
    }
}