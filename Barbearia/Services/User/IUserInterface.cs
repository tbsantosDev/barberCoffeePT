using Barbearia.Dto.User;
using Barbearia.Models;

namespace Barbearia.Services.User
{
    public interface IUserInterface
    {
        Task<ResponseModel<List<UserModel>>> ListUsersClient();
        Task<ResponseModel<List<UserModel>>> ListUsersAdmin();
        Task<ResponseModel<UserPointsDto>> ListPointsByUser();
        Task<ResponseModel<UserModel>> FindUserById(int id);
        Task<ResponseModel<UserModel>> FindUserByIdSchedule(int id);
        Task<ResponseModel<List<UserModel>>> CreateAdminUser(CreateUserDto createUserAdminDto);
        Task<ResponseModel<UserModel>> LoggedUser();
        Task<ResponseModel<UserModel>> UpdateUser(UpdateUserDto updateUserDto);
        Task<ResponseModel<UserModel>> UpdateUserPassword(UpdateUserPasswordDto updateUserPasswordDto);
        Task<ResponseModel<string>> RequestPasswordReset(string email);
        Task<ResponseModel<string>> ResetPassword(string email, string token, string newPassword);
        Task<ResponseModel<List<UserModel>>> DeleteUser(int id);
    }
}
