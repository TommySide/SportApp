using SharedLibrary.Entities;

namespace SharedLibrary.Dtos;

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required User User { get; set; }
}