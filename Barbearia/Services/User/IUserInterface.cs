using Barbearia.Dto.User;
using Barbearia.Models;

namespace Barbearia.Services.User
{
    public interface IUserInterface
    {
        Task<ResponseModel<List<UserModel>>> ListUsersClient();
        Task<ResponseModel<List<UserModel>>> ListUsersAdmin();
        Task<ResponseModel<UserPointsDto>> ListPointsByUserId(int userId);
        Task<ResponseModel<UserModel>> FindUserById(int id);
        Task<ResponseModel<UserModel>> FindUserByIdSchedule(int id);
        Task<ResponseModel<List<UserModel>>> CreateAdminUser(CreateUserDto createUserAdminDto);
        Task<ResponseModel<List<UserModel>>> UpdateUser(UpdateUserDto updateUserDto);
        Task<ResponseModel<List<UserModel>>> DeleteUser(int id);
    }
}
