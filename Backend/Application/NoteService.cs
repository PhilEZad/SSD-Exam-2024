using Application.DTOs.Create;
using Application.DTOs.Response;
using Application.DTOs.Update;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;

namespace Application;

public class NoteService : INoteService
{
    private readonly IMapper _mapper;
    private readonly IValidator<Note> _validator;
    private readonly INoteRepository _noteRepository;
    
    public NoteService(IMapper mapper, IValidator<Note> validator, INoteRepository noteRepository)
    {
        _mapper = mapper;
        _validator = validator;
        _noteRepository = noteRepository;
    }
    
    public NoteResponse Create(NoteCreate createDto)
    {
        Note create = _mapper.Map<NoteCreate, Note>(createDto);
        
        var validationResult = _validator.Validate(create);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }
        
        return _mapper.Map<Note, NoteResponse>(_noteRepository.Create(create));
    }

    public NoteResponse ReadById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException("Id must be greater than 0");
        }

        return _mapper.Map<Note, NoteResponse>(_noteRepository.Read(id));
    }

    public NoteResponse Update(NoteUpdate updateDto)
    {
        Note update = _mapper.Map<NoteUpdate, Note>(updateDto);
        
        var validationResult = _validator.Validate(update);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }
        return _mapper.Map<Note, NoteResponse>(_mapper.Map<Note, Note>(update));
    }

    public bool Delete(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException("Id must be greater than 0");
        }
        
        return _noteRepository.Delete(id);
    }
}