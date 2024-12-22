using Application.DTOs.Create;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;

namespace Application;

public class AuthService : IAuthService
{
    private readonly IValidator<User> _validator;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;

    public AuthService(IValidator<User> validator, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IAuthRepository authRepository, IMapper mapper)
    {
        _validator = validator;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _authRepository = authRepository;
        _mapper = mapper;
    }
    
    public bool Register(RegisterDto registerDto)
    {
        var register = _mapper.Map<RegisterDto, User>(registerDto);
        
        register.PlainPassword = _passwordHasher.Hash(register.PlainPassword, register.Username);
        
        throw new NotImplementedException();
    }

    public LoginResponse Login(LoginDto loginDto)
    {
        var login = _mapper.Map<LoginDto, User>(loginDto);
        
        throw new NotImplementedException();
    }
}