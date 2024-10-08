using Barbearia.Models;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {  
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<BarberModel> Barbers { get; set; }
        public DbSet<ScheduleModel> Schedules { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduleModel>()
                .HasOne(s => s.Barber)
                .WithMany(b => b.Schedules)
                .HasForeignKey(s => s.BarberId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<PointModel> Points { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ExchangeModel> Exchanges { get; set; }

    }
}
