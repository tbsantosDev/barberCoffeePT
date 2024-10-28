using Barbearia.Dto.User;
using Barbearia.Models;
using Barbearia.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        public UsersController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }
        [Authorize]
        [HttpGet("ListUsersClient")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> ListUsersClient()
        {
            var users = await _userInterface.ListUsersClient();
            return Ok(users);
        }
        [Authorize]
        [HttpGet("ListUsersAdmin")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> ListUsersAdmin()
        {
            var users = await _userInterface.ListUsersAdmin();
            return Ok(users);
        }
        [Authorize]
        [HttpGet("FindUserById/{id}")]

        public async Task<ActionResult<ResponseModel<UserModel>>> FindUserById(int id)
        {
            var user = await _userInterface.FindUserById(id);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("FindUserByIdSchedule/{id}")]
        public async Task<ActionResult<ResponseModel<UserModel>>> FindUserByIdSchedule(int id)
        {
            var user = await _userInterface.FindUserByIdSchedule(id);
            return Ok(user);
        }
        [Authorize]
        [HttpPost("CreateUserAdmin")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> CreateAdminUser(CreateUserDto createUserAdminDto)
        {
            var user = await _userInterface.CreateAdminUser(createUserAdminDto);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("LoggedUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> LoggedUser()
        {
            var user = await _userInterface.LoggedUser();
            return Ok(user);
        }
        [Authorize]
        [HttpGet("ListPointsByUser")]
        public async Task<ActionResult<ResponseModel<List<UserPointsDto>>>> ListPointsByUser()
        {
            var userPoint = await _userInterface.ListPointsByUser();
            return Ok(userPoint);
        }
        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = await _userInterface.UpdateUser(updateUserDto);
            return Ok(user);
        }
        [Authorize]
        [HttpPut("UpdateUserPassword")]
        public async Task<ActionResult<ResponseModel<UserModel>>> UpdateUserPassword(UpdateUserPasswordDto updateUserPasswordDto)
        {
            var user = await _userInterface.UpdateUserPassword(updateUserPasswordDto);
            return Ok(user);
        }

        [HttpPost("requestPasswordReset")]
        public async Task<ActionResult<ResponseModel<string>>> RequestPasswordReset([FromBody] string email)
        {
            Console.WriteLine($"Parametros que chegaram ao backend {email}");
            var user = await _userInterface.RequestPasswordReset(email);
            return Ok(user);
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult<ResponseModel<string>>> ResetPassword([FromBody] ResetUserPasswordDto resetUserPasswordDto)
        {
            var result = await _userInterface.ResetPassword(
                resetUserPasswordDto.Email,
                resetUserPasswordDto.Token,
                resetUserPasswordDto.NewPassword
                );
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> DeleteUser(int id)
        {
            var user = await _userInterface.DeleteUser(id);
            return Ok(user);
        }
    }
}
