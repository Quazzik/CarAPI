using CarAPI.DTOs;
using CarAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] UserDataDto userData)
    {
        try
        {
            var response = await _authService.RegisterAsync(userData);
            return CreatedAtAction(nameof(Register), response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] UserDataDto userData)
    {
        try
        {
            var response = await _authService.LoginAsync(userData);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}