using Microsoft.AspNetCore.Mvc;
using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Application.Validators.Interfaces;
using PaintyTask.Domain.Exceptions;
using PaintyTask.Domain.Models.DTO;

namespace PaintyTask.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;
    private readonly IValidate _validate;

    public AuthController(IAuthService authService, IJwtService jwtService, IValidate validate)
    {
        _authService = authService;
        _jwtService = jwtService;
        _validate = validate;
    }

    [HttpPost("/signIn")]
    public async Task<IActionResult> SignIn([FromForm] AuthDto authDto)
    {
        try
        {
            _validate.ValidateAuth(authDto);
            var user = await _authService.SignIn(authDto);
            if (user == null)
            {
                return NotFound(new { message = "Invalid login or password" });
            }

            var response = new
            {
                access_toke = _jwtService.GenerateJwt(authDto.Login, user.Id),
                login = user.Login,
            };

            return Ok(new { response });
        }
        catch (ValidateException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPost("/signUp")]
    public async Task<IActionResult> SignUp([FromForm] RegisterDto registerDto)
    {
        try
        {
            await _validate.ValidateCreateUser(registerDto);
            var user = await _authService.SignUp(registerDto);

            return user == null
                ? BadRequest(new { message = "Error" })
                : Ok(new { user });
        }
        catch (ValidateException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}