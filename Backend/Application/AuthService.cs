using Application.DTOs.Create;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain;
using FluentValidation;

namespace Application;

public class AuthService : IAuthService
{
    private readonly IValidator<User> _validator;

    public AuthService(IValidator<User> validator)
    {
        _validator = validator;
    }
    
    public bool Register(RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    public LoginResponse Login(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }
}