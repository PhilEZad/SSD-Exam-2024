using Application.DTOs.Create;
using Application.DTOs.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            return Ok(_authService.Register(registerDto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        try
        {
            return Ok(_authService.Login(loginDto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}