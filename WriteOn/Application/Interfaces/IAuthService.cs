using Application.DTOs.Create;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface IAuthService
{
    public bool Register(RegisterDto registerDto);
    public LoginResponse Login(LoginDto loginDto);
}