using BivliotecaAPI;
using BivliotecaAPI.Datos;
using BivliotecaAPI.Entidades;
using BivliotecaAPI.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.Xml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//area de servicios 
builder.Services.AddDataProtection();

var origenesPermitidos = builder.Configuration.GetSection("origenesPermitidos").Get<string[]>()!;
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(
        politica =>
        {
            politica.WithOrigins(origenesPermitidos)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("mi-cabecera");
        });
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddIdentityCore<Usuario>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<Usuario>>();
builder.Services.AddScoped<SignInManager<Usuario>>();
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication().AddJwtBearer(opciones
    =>
{
    opciones.MapInboundClaims = false;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!)),
        ClockSkew = TimeSpan.Zero // Reduce el tiempo de tolerancia para la expiración del token
    };
}
);

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin", "true"));
    opciones.AddPolicy("EsLector", politica => politica.RequireClaim("esLector", "true"));
    opciones.AddPolicy("EsAutor", politica => politica.RequireClaim("esAutor", "true"));
});

var app = builder.Build();
//area de middlewares
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("mi-cabecera", "valor");
    await next();
});

app.UseCors();

app.MapControllers();   

app.Run();
