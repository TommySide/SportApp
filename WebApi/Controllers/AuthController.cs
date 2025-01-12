using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using SharedLibrary.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;
    
    public AuthController(AuthService service)
    {
        _service = service;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _service.Register(registerDto);

        if (!result)
        {
            return BadRequest("E-mail already exists");
        }

        return Ok("User created");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var loginResponse = await _service.Login(loginDto);
            return Ok(loginResponse);  
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message); 
        }
    }
}