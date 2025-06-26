using System.Security.Cryptography.Xml;
using BivliotecaAPI;
using BivliotecaAPI.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var diccionarioConfiguraciones = new Dictionary<string, string>
{
    {"quien_soy", "un diccionario en memoria"}
};
builder.Configuration.AddInMemoryCollection(diccionarioConfiguraciones!);

//area de servicios 
builder.Services.AddOptions<PersonaOpciones>()
    .Bind(builder.Configuration.GetSection(PersonaOpciones.Seccion))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<tarifaOpciones>()
    .Bind(builder.Configuration.GetSection(tarifaOpciones.Seccion))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<PagosProcesamiento>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();
//area de middlewares


app.MapControllers();   

app.Run();
