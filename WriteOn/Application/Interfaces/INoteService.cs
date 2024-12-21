﻿using Application.DTOs.Create;
using Application.DTOs.Response;
using Application.DTOs.Update;

namespace Application.Interfaces;

public interface INoteService
{
    public NoteResponse Create(NoteCreate createDto);
    public NoteResponse ReadById(int id);
    public NoteResponse Update(NoteUpdate updateDto);
    public bool Delete(int id);
}