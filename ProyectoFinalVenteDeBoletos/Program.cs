using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

string? Db = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddMySql<CinemaventaboletosContext>(Db,ServerVersion.AutoDetect(Db));

builder.Services.AddMvc();
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.Run();
