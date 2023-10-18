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
public class FriendshipsController : ControllerBase
{
    private readonly IFriendshipService _friendshipService;
    private readonly IJwtService _jwtService;
    private readonly IAuthorizationHelper _authorizationHelper;
    private readonly Mapper _mapper;
    private readonly IUserService _userService;
    private readonly IValidate _validate;

    public FriendshipsController(
        IFriendshipService friendshipService,
        IJwtService jwtService,
        IAuthorizationHelper authorizationHelper,
        Mapper mapper,
        IUserService userService,
        IValidate validate)
    {
        _friendshipService = friendshipService;
        _jwtService = jwtService;
        _authorizationHelper = authorizationHelper;
        _mapper = mapper;
        _userService = userService;
        _validate = validate;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var userReceiverId = GetUserId();
            var user = await _userService.GetUserById(userReceiverId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var receivedFriendshipRequests = _userService.GetUsersWithReceivedFriendshipRequests(userReceiverId);
            return receivedFriendshipRequests == null || !receivedFriendshipRequests.Any()
                ? BadRequest(new { message = "You don't have any friendship requests" })
                : Ok(new { friendships = receivedFriendshipRequests });
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

    [HttpPost("send/{login}")]
    public async Task<IActionResult> Send(string login)
    {
        try
        {
            _validate.ValidateSendFriendship(login);
            var userReceiver = await _userService.GetUserByLoginOrEmail(login);
            if (userReceiver == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userSenderId = GetUserId();
            var friendshipDto = GetFriendshipDto(userSenderId, userReceiver.Id);
            var friendship = await _friendshipService.SendFriendshipRequest(friendshipDto);

            return friendship == null
                ? BadRequest(new { message = "This friendship request already exists" })
                : Ok(new { message = "Successful" });
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

    [HttpPatch("userAccept")]
    public async Task<IActionResult> Accept([FromForm] string login)
    {
        try
        {
            _validate.ValidateSendFriendship(login);
            var userSender = await _userService.GetUserByLoginOrEmail(login);
            if (userSender == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userReceiverId = GetUserId();
            var friendshipDto = GetFriendshipDto(userSender.Id, userReceiverId);
            var friendship = await _friendshipService.AcceptFriendshipRequest(friendshipDto);

            return friendship == null
                ? NotFound(new { message = "This friendship request doesn't exist" })
                : Ok(new { message = "Successful" });
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

    [HttpDelete("userReceiver")]
    public async Task<IActionResult> Reject(string login)
    {
        try
        {
            _validate.ValidateSendFriendship(login);
            var userSender = await _userService.GetUserByLoginOrEmail(login);
            if (userSender == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userReceiverId = GetUserId();
            var friendshipDto = GetFriendshipDto(userSender.Id, userReceiverId);
            var friendship = await _friendshipService.RejectFriendshipRequest(friendshipDto);

            return friendship == null
                ? NotFound(new { message = "This friendship request doesn't exist" })
                : Ok(new { message = "Successful" });
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
    public FriendshipDto GetFriendshipDto(int userSenderId, int userReceiverId)
    {
        var friendshipDto = _mapper.Map<FriendshipDto>(userSenderId);
        friendshipDto.UserReceiverId = userReceiverId;

        return friendshipDto;
    }

    [NonAction]
    public int GetUserId()
    {
        var token = _authorizationHelper
               .GetTokenFromAuthorizationHeader(HttpContext);

        return _jwtService.GetUserIdFromToken(token);
    }
}