using GymControlAPI.Data;
using GymControlAPI.Repositories;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();    // <- Necesario para Swagger
//Permite a swagger leer JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GymControlAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Introduce el token JWT con el formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// <- Genera documentación Swagger
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//Variables de configuración

//JWT configuration from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

// Configurar JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddAuthorization();

// Leer cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registro del repositorio - Inyección de dependencias
builder.Services.AddScoped<IUsuario, UsuarioRepo>();
builder.Services.AddScoped<IRol, RolRepo>();
builder.Services.AddScoped<ITipoPago, TipoPagoRepo>();
builder.Services.AddScoped<IPlan, PlanRepo>();
builder.Services.AddScoped<IPago, PagoRepo>();
builder.Services.AddScoped<IAsistencia, AsistenciaRepo>();
builder.Services.AddScoped<ILogin, LoginRepo>();

//Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Dirección del frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();                          // <- Habilita Swagger en entorno dev
    app.UseSwaggerUI();                        // <- Habilita la interfaz visual
}


app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");             // <- Habilita CORS para Angular
app.UseAuthentication();            // <- Habilita la autenticación JWT
app.UseAuthorization();
app.MapControllers();
app.Run();
