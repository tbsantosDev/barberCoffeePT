using Barbearia.Data;
using Barbearia.Models;
using Barbearia.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (await context.Users.AnyAsync())
        {
            Console.WriteLine("Usuários já existentes na base de dados.");
            return;
        }

        var adminUser = new UserModel
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@barbercoffee.com",
            Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = RoleEnums.Admin,
            EmailConfirmed = true,
            EmailConfirmationToken = Guid.NewGuid().ToString()
        };

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();

        Console.WriteLine("Usuário administrador criado com sucesso!");
    }
}
