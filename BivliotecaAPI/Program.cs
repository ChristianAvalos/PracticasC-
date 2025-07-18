using BivliotecaAPI;
using BivliotecaAPI.Datos;
using BivliotecaAPI.Entidades;
using BivliotecaAPI.Servicios;
using BivliotecaAPI.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
                    .WithExposedHeaders("cantidadTotalRegistros");
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

builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Biblioteca API",
        Version = "v1",
        Description = "API para gestionar una biblioteca",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "christiand.avalos.e@gmail.com",
            Name = "Christian Avalos",
            Url = new Uri("https://christianavalos.com")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/license/mit/")
        }
    });

    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer' seguido de un espacio y su token JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"

    });
    opciones.OperationFilter<FiltroAutorizacion>();
    //opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[] {}
    //    }
    //});
});

var app = builder.Build();
//area de middlewares
app.UseSwagger();
app.UseSwaggerUI();


app.UseCors();

app.MapControllers();   

app.Run();
