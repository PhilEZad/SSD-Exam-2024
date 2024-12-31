using System.Security.Claims;
using Application.DTOs.Create;
using Application.DTOs.Update;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost]
    public IActionResult AddNote([FromBody] NoteCreate note)
    {
        try
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            
            return Ok(_noteService.Create(note, int.Parse(userId)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route("{id}")]
    public IActionResult ReadNoteById([FromRoute] int id)
    {
        try
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            
            return Ok(_noteService.ReadById(id, int.Parse(userId)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAllUserNotes()
    {
        try
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            
            return Ok(_noteService.ReadByUser(int.Parse(userId)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize]
    [HttpPatch]
    public IActionResult UpdateNote([FromBody] NoteUpdate note)
    {
        try
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            
            return Ok(_noteService.Update(note, int.Parse(userId)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteNote([FromRoute] int id)
    {
        try
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            
            return Ok(_noteService.Delete(id, int.Parse(userId)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}