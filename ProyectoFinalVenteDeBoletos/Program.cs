using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Repositories;

#region Configuracion General(Builder)

var builder = WebApplication.CreateBuilder(args);

#region Conexion a la base de datos
//DB es la conexion especificada en appsettings.json
string? Db = builder.Configuration.GetConnectionString("DbConnectionString");
//Esta es la conexion a la base de datos
builder.Services.AddMySql<CinemaventaboletosContext>(Db, ServerVersion.AutoDetect(Db));
#endregion

//Utilizar MVC
builder.Services.AddMvc();

#region Repositorios Utilizando AddTransient
//Para inyectar los repositorios directamente sin dar contexto
builder.Services.AddTransient<RepositorioClasificaciones>();
builder.Services.AddTransient<RepositorioPeliculas>();
builder.Services.AddTransient<RepositorioSalas>();
builder.Services.AddTransient<RepositorioHorario>();
#endregion

#endregion

#region Configuracion General(App)
var app = builder.Build();
app.MapDefaultControllerRoute();
app.UseStaticFiles();
#endregion
app.Run();
