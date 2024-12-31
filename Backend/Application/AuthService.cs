using System.Security.Authentication;
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
    private readonly IValidator<RegisterDto> _validatorRegisterDto;
    private readonly IValidator<LoginDto> _validatorLoginDto;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;

    public AuthService(IValidator<RegisterDto> validatorRegisterDto, IValidator<LoginDto> validatorLoginDto, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IAuthRepository authRepository, IMapper mapper)
    {
        _validatorRegisterDto = validatorRegisterDto;
        _validatorLoginDto = validatorLoginDto;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _authRepository = authRepository;
        _mapper = mapper;
    }
    
    public bool Register(RegisterDto registerDto)
    {
        var validationResult = _validatorRegisterDto.Validate(registerDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }
        
        var register = _mapper.Map<RegisterDto, User>(registerDto);
        
        register.HashedPassword = _passwordHasher.Hash(registerDto.PlainPassword, register.Username);
        register.Created = DateTime.Now;
        register.Modified = DateTime.Now;
        
        return _authRepository.Create(register);
    }

    public LoginResponse Login(LoginDto loginDto)
    {
        var validationResult = _validatorLoginDto.Validate(loginDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }
        
        var requestDb = _mapper.Map<User>(loginDto);
        
        var responseDb = _authRepository.Read(requestDb);

        var hashedPassword = _passwordHasher.Hash(loginDto.PlainPassword, loginDto.Username);

        if (responseDb.HashedPassword != hashedPassword)
        {
            throw new AuthenticationException("Incorrect password");
        }

        return new LoginResponse{ Jwt = _jwtProvider.GenerateToken(responseDb.Id, responseDb.Username)};
    }
}