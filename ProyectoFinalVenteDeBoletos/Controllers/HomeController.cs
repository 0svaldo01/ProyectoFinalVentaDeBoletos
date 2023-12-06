using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using ProyectoFinalVentaDeBoletos.Repositories;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVenteDeBoletos.Controllers
{
    public class HomeController : Controller
    {
        RepositorioClasificaciones ClasificacionRepositorio { get; }
        public HomeController(RepositorioClasificaciones repositorioPeliculas)
        {
            ClasificacionRepositorio = repositorioPeliculas;
        }
        public IActionResult Index()
        {
            IndexViewModel vm = new()
            {
                Clasificaciones = ClasificacionRepositorio.GetAll().Select(x => new ClasificacionModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Peliculas = x.Pelicula
                    .OrderBy(p => p.Nombre)
                    .Select(p => new PeliculaModel
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Sinopsis = p.Sinopsis
                    })
                })
            };
            return View(vm);
        }
    }
}