using Microsoft.EntityFrameworkCore;
using SpaceConnect.ApiService.Data;
using SpaceConnect.ApiService.Data.Interfaces;
using SpaceConnect.ApiService.Data.Repositories;
using SpaceConnect.ApiService.Filters;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("spaceconnect")
    ?? builder.Configuration["ConnectionStrings:Default"]
    ?? "Server=localhost;Database=spaceconnect;User=root;Password=spaceconnect@2025;CharSet=utf8mb4;";

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });

builder.Services.AddDbContext<SpaceConnectContext>(opt =>
    opt.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

builder.Services.AddScoped<ITecnologiaRepository, TecnologiaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<AdminApiAuthorizationFilter>();

builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SpaceConnectContext>();
    await DataEncodingFix.ApplyAsync(db);
    await DemoUserSeed.ApplyAsync(db);
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
