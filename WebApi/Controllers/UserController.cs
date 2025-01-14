using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;
    
    public UserController(UserService service)
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
    
    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _service.GetAllUsers();
        return Ok(users);
    }
}