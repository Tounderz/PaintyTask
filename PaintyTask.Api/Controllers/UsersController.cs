using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintyTask.Api.Helpers.Interfaces;
using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Application.Validators.Interfaces;
using PaintyTask.Domain.Exceptions;
using PaintyTask.Domain.Models.DTO;

namespace PaintyTask.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IFriendshipService _friendshipService;
    private readonly IJwtService _jwtService;
    private readonly IAuthorizationHelper _authorizationHelper;
    private readonly Mapper _mapper;
    private readonly ISaveImgService _saveImgService;
    private readonly IValidate _validate;

    public UsersController(IUserService userService,
        IJwtService jwtService,
        IAuthorizationHelper authorizationHelper,
        Mapper mapper,
        IFriendshipService friendshipService,
        ISaveImgService saveImgService,
        IValidate validate)
    {
        _userService = userService;
        _jwtService = jwtService;
        _authorizationHelper = authorizationHelper;
        _mapper = mapper;
        _friendshipService = friendshipService;
        _saveImgService = saveImgService;
        _validate = validate;
    }

    [HttpGet("getPhotos")]
    public async Task<IActionResult> GetPhotosByUser(string? login)
    {
        try
        {
            var token = _authorizationHelper
                .GetTokenFromAuthorizationHeader(HttpContext);
            var userId = _jwtService.GetUserIdFromToken(token);
            var user = string.IsNullOrEmpty(login)
                ? await _userService.GetUserById(userId)
                : await _userService.GetUserByLoginOrEmail(login);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (!string.IsNullOrEmpty(login))
            {
                var friendshipDto = _mapper.Map<FriendshipDto>(user.Id);
                friendshipDto.UserReceiverId = userId;
                var isFriendship = await _friendshipService.IsFriendship(friendshipDto);
                if (!isFriendship)
                {
                    return BadRequest(new { message = "You are not friends with this user" });
                }
            }

            return user.Photos == null || !user.Photos.Any()
                ? BadRequest(new { message = "This user has no photos" })
                : Ok(new { photos = user.Photos.Select(i => i.Url) });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidJwtTokenException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {

            return BadRequest(new { message = "Error" });
        }
    }

    [HttpPost("createPhoto")]
    public async Task<IActionResult> CreatePhoto(IFormFile photo)
    {
        try
        {
            _validate.ValidateCreatePhoto(photo);
            var userId = GetUserId();
            var url = await _saveImgService.SaveImg(photo);
            var photoDto = _mapper.Map<PhotoDto>(url);
            photoDto.UserId = userId;
            var photoData = await _userService.CreatePhoto(photoDto);

            return photoData == null
                ? BadRequest(new { message = "Error" })
                : Ok(new { photoData });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidJwtTokenException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ValidateException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error" });
        }
    }

    [NonAction]
    public int GetUserId()
    {
        var token = _authorizationHelper
               .GetTokenFromAuthorizationHeader(HttpContext);

        return _jwtService.GetUserIdFromToken(token);
    }
}