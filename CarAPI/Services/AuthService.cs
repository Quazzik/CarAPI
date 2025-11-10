using AutoMapper;
using CarAPI.Models;
using CarAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarAPI.DTOs;

namespace CarAPI.Services;

public class AuthService
{
    private readonly AuthRepository _authRepository;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(AuthRepository authRepository, IMapper mapper, JwtSettings jwtSettings)
    {
        _authRepository = authRepository;
        _mapper = mapper;
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthResponseDto> RegisterAsync(UserDataDto userData)
    {
        if (await _authRepository.ExistsAsync(userData.Login))
            throw new ArgumentException("User with this login already exists");

        var user = new User
        {
            Login = userData.Login,
            Password = HashPassword(userData.Password)
        };

        var createdUser = await _authRepository.AddAsync(user);

        var token = GenerateJwtToken(createdUser);

        return new AuthResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(createdUser)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(UserDataDto userData)
    {
        var user = await _authRepository.GetByLoginAsync(userData.Login);
        if (user == null || !VerifyPassword(userData.Password, user.Password))
            throw new ArgumentException("Invalid login or password");
        if (!user.Verified)
            throw new ArgumentException("User is not verified");
        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Login) }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    private bool VerifyPassword(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
