using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Controllers
{
    public class HomeController : Controller
    {
        RepositorioClasificaciones ClasificacionRepositorio { get; }
        public HomeController(
            RepositorioClasificaciones repositorioClasificaciones
            )
        {
            ClasificacionRepositorio = repositorioClasificaciones;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult VerPeliculas()
        {
            PeliculasViewModel vm = new()
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

        [HttpGet("/Home/Pelicula")]
        public IActionResult VerPelicula()
        { 
            return View();
        }
    }
}