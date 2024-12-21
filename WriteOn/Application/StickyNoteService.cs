using Application.DTOs.Create;
using Application.DTOs.Response;
using Application.DTOs.Update;
using Application.Interfaces;
using Application.Validator;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure.Interface;

namespace Application;

public class StickyNoteService : IStickyNoteService
{
    private readonly IMapper _mapper;
    private readonly IValidator<StickyNote> _validator;
    private readonly IStickyNotesRepository _stickyNotesRepository;
    
    public StickyNoteService(IMapper mapper, IValidator<StickyNote> validator, IStickyNotesRepository stickyNotesRepository)
    {
        _mapper = mapper;
        _validator = validator;
        _stickyNotesRepository = stickyNotesRepository;
    }
    
    public StickyNoteResponse Create(StickyNoteCreate createDto)
    {
        StickyNote create = _mapper.Map<StickyNoteCreate, StickyNote>(createDto);
        
        var validationResult = _validator.Validate(create);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }
        
        var returnCreate = _stickyNotesRepository.Add(create);
        return _mapper.Map<StickyNote, StickyNoteResponse>(returnCreate);
    }

    public StickyNoteResponse ReadById(int id)
    {
        throw new NotImplementedException();
    }

    public StickyNoteResponse Update(StickyNoteUpdate update)
    {
        throw new NotImplementedException();
    }

    public StickyNoteResponse Delete(int id)
    {
        throw new NotImplementedException();
    }
}