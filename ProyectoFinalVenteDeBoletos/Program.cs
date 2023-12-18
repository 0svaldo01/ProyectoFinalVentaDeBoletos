using Microsoft.AspNetCore.Authentication.Cookies;
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
    builder.Services.AddTransient<RepositorioHorarios>();
    builder.Services.AddTransient<RepositorioPeliculas>();
    builder.Services.AddTransient<RepositorioSalas>();

    #endregion

    //#region Autentificacion
    //builder.Services
    //    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //    .AddCookie(x =>
    //    {
    //        x.AccessDeniedPath = "/Home/Denied";
    //        x.LoginPath = "/Home/Login";
    //        x.LogoutPath = "/Home/Logout";
    //        x.ExpireTimeSpan = TimeSpan.FromSeconds(50); //TimeSpan.FromMinutes(5); //Tiempo en la que la cookie esta activa.
    //        x.Cookie.Name = "noticiaCookie";
    //    });
    //#endregion

#endregion

#region Configuracion General(App)

    var app = builder.Build();
    app.UseStaticFiles();
    app.UseFileServer();
    app.MapDefaultControllerRoute();
    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

#endregion

app.Run();