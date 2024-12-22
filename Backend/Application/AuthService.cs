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
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthRepository _authRepository;

    public AuthService(IValidator<User> validator, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IAuthRepository authRepository)
    {
        _validator = validator;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _authRepository = authRepository;
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