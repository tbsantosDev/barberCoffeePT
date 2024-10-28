using Barbearia.Data;
using Barbearia.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Barbearia.Services.Barber;
using DotNetEnv;
using Barbearia.Services.Schedule;
using System.Globalization;
using Barbearia.Infra;
using Barbearia.Services.Point;
using Barbearia.Services.Product;
using Barbearia.Services.Exchange;
using Barbearia.Services.Category;

Env.Load(); // Carrega as variáveis do arquivo .env

var builder = WebApplication.CreateBuilder(args);

// Configurar a cultura para português de Portugal (pt-PT)
var cultureInfo = new CultureInfo("pt-PT");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configurar string de conexão do banco de dados
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

// Configurar chave JWT
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
    ?? builder.Configuration["JWT:SecretKey"];

var key = Encoding.ASCII.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Substitua pelo domínio que você quer permitir
               .AllowAnyHeader()
               .AllowAnyMethod();
    });

    // Para desenvolvimento, se quiser liberar todas as origens (não recomendado em produção)
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructionSwagger();

builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<IBarberInterface, BarberService>();
builder.Services.AddScoped<IScheduleInterface, ScheduleService>();
builder.Services.AddScoped<IPointInterface, PointService>();
builder.Services.AddScoped<IProductInterface, ProductService>();
builder.Services.AddScoped<ICategoryInterface, CategoryService>();
builder.Services.AddScoped<IExchangeInterface, ExchangeService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
await SeedData.SeedAdminUserAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar a política de CORS antes da autenticação
app.UseCors("AllowAll"); // Aplique a política que você configurou, ou "AllowAll" para desenvolvimento

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
