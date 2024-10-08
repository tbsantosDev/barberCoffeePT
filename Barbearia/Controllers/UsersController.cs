using Barbearia.Dto.User;
using Barbearia.Models;
using Barbearia.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        public UsersController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpGet("ListUsersClient")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> ListUsersClient()
        {
            var users = await _userInterface.ListUsersClient();
            return Ok(users);
        }

        [HttpGet("ListUsersAdmin")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> ListUsersAdmin()
        {
            var users = await _userInterface.ListUsersAdmin();
            return Ok(users);
        }

        [HttpGet("ListPointsByUserId")]
        public async Task<ActionResult<ResponseModel<List<UserPointsDto>>>> ListPointsByUserId(int userId)
        {
            var userPoint = await _userInterface.ListPointsByUserId(userId);
            return Ok(userPoint);
        }

        [HttpGet("FindUserById/{id}")]
        
        public async Task<ActionResult<ResponseModel<UserModel>>> FindUserById(int id)
        {
            var user = await _userInterface.FindUserById(id);
            return Ok(user);
        }

        [HttpGet("FindUserByIdSchedule/{id}")]
        public async Task<ActionResult<ResponseModel<UserModel>>> FindUserByIdSchedule(int id)
        {
            var user = await _userInterface.FindUserByIdSchedule(id);
            return Ok(user);
        }
        [HttpPost("CreateUserAdmin")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> CreateAdminUser(CreateUserDto createUserAdminDto)
        {
            var user = await _userInterface.CreateAdminUser(createUserAdminDto);
            return Ok(user);
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = await _userInterface.UpdateUser(updateUserDto);
            return Ok(user);
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> DeleteUser(int id)
        {
            var user = await _userInterface.DeleteUser(id);
            return Ok(user);
        }
    }
}
