﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary;
using SharedLibrary.Dtos;
using SharedLibrary.Entities;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class UserService
{
    private readonly UserRepository _repository;
    private readonly IConfiguration _configuration;

    public UserService(UserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<bool> Register(RegisterDto registerDto)
    {
        if (await _repository.GetUserByEmailAsync(registerDto.Email) != null)
        {
            return false;
        }

        registerDto.FirstName = registerDto.FirstName[0].ToString().ToUpper() + registerDto.FirstName.Substring(1);
        registerDto.LastName = registerDto.LastName[0].ToString().ToUpper() + registerDto.LastName.Substring(1);
        
        var user = new User
        {
            Password = String.Empty,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            Role = Role.Customer
        };
        
        var hasher = new PasswordHasher<User>();
        var hashedPwd = hasher.HashPassword(user, registerDto.Password);
        user.Password = hashedPwd;

        await _repository.AddUserAsync(user);
        return true;
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var user = await _repository.GetUserByEmailAsync(loginDto.Email);

        var hasher = new PasswordHasher<User>();

        if (user == null)
        {
            throw new UnauthorizedAccessException("Email doesn't exist");
        }

        if (hasher.VerifyHashedPassword(user, user.Password, loginDto.Password) == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Wrong password");
        }
        
        user.Password = "";
        
        return new LoginResponseDto
        {
            Token = GenerateJwtToken(user),
            User = user
        };
    }

    
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.FirstName + "" + user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException()));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<List<User>> GetAllUsers()
    {
        var users = await _repository.GetUsersAsync();
        return users;
    }
}