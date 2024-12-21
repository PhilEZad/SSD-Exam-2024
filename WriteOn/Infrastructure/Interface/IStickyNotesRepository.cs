﻿using Domain;

namespace Infrastructure.Interface;

public interface IStickyNotesRepository
{
    public StickyNote Add(StickyNote add);
    public StickyNote Update(StickyNote update);
    public StickyNote Get(int id);
    public IList<StickyNote> GetAll();
    public bool Delete(int id);
}