using Barbearia.Models.Enums;

namespace Barbearia.Dto.User
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleEnums Role { get; set; }
    }
}
