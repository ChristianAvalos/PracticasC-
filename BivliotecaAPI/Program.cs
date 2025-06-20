using System.Security.Cryptography.Xml;
using BivliotecaAPI;
using BivliotecaAPI.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//area de servicios 
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();
//area de middlewares


app.MapControllers();   

app.Run();
