﻿namespace Barbearia.Dto.User
{
    public class ResetUserPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
