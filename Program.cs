using GymControlAPI.Data;
using GymControlAPI.Repositories;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();    // <- Necesario para Swagger
builder.Services.AddSwaggerGen();              // <- Genera documentación Swagger
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Leer cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registro del repositorio
builder.Services.AddScoped<IUsuario, UsuarioRepo>();
builder.Services.AddScoped<IRol, RolRepo>();
builder.Services.AddScoped<ITipoPago, TipoPagoRepo>();
builder.Services.AddScoped<IPlan, PlanRepo>();
builder.Services.AddScoped<IPago, PagoRepo>();
builder.Services.AddScoped<IAsistencia, AsistenciaRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();                          // <- Habilita Swagger en entorno dev
    app.UseSwaggerUI();                        // <- Habilita la interfaz visual
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
