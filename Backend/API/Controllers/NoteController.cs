using Application.DTOs.Create;
using Application.DTOs.Update;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    
    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpPost]
    public IActionResult AddNoteAsync([FromBody] NoteCreate note)
    {
        try
        {
            return Ok(_noteService.Create(note));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult ReadNoteById([FromRoute] int id)
    {
        try
        {
            return Ok(_noteService.ReadById(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    public IActionResult UpdateNoteAsync([FromBody] NoteUpdate note)
    {
        try
        {
            return Ok(_noteService.Update(note));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult GetNoteById([FromRoute] int id)
    {
        try
        {
            return Ok(_noteService.Delete(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}