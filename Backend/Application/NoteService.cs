﻿using Application.DTOs.Create;
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
    
    public NoteResponse Create(NoteCreate createDto, int userId)
    {
        Note create = _mapper.Map<NoteCreate, Note>(createDto);
        
        create.OwnerId = userId;
        create.Created  = DateTime.Now;
        create.Modified = DateTime.Now;
        
        var validationResult = _validator.Validate(create);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }
        
        return _mapper.Map<Note, NoteResponse>(_noteRepository.Create(create));
    }

    public NoteResponse ReadById(int id, int userId)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException("Id must be greater than 0");
        }

        var note = _noteRepository.Read(id);

        if (note.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to access this resource.");
        }
        return _mapper.Map<Note, NoteResponse>(note);
    }

    public List<NoteResponse> ReadByUser(int id)
    {
        return _mapper.Map<List<NoteResponse>>(_noteRepository.ReadAllById(id));
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