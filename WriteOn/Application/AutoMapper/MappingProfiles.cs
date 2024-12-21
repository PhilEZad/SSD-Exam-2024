﻿using Application.DTOs.Create;
using Application.DTOs.Response;
using Application.DTOs.Update;
using AutoMapper;
using Domain;

namespace Application.AutoMapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<NoteCreate, Note>();
        CreateMap<NoteUpdate, Note>();
        
        CreateMap<Note, NoteResponse>();
    }
}