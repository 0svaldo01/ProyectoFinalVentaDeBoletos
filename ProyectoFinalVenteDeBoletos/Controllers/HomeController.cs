using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Controllers
{
    public class HomeController : Controller
    {
        private readonly Random r = new();
        RepositorioClasificaciones ClasificacionRepositorio { get; }
        RepositorioHorario HorarioRepositorio { get; }
        RepositorioPeliculas PeliculasRepositorio { get; }
        public HomeController(
            RepositorioClasificaciones repositorioClasificaciones,
            RepositorioHorario repositorioHorarios,
            RepositorioPeliculas repositorioPeliculas
            )
        {
            ClasificacionRepositorio = repositorioClasificaciones;
            HorarioRepositorio = repositorioHorarios;
            PeliculasRepositorio = repositorioPeliculas;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Peliculas")]
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
                    .Select(p => new PeliculasModel
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Sinopsis = p.Sinopsis
                    })
                })
            };
            return View(vm);
        }

        [HttpGet("/Pelicula/{nombre}")]
        public IActionResult Pelicula(string nombre)
        {
            nombre = nombre.Replace('-',' ');
            var peli = PeliculasRepositorio.GetPeliculaByNombre(nombre);
            if (peli != null)
            {
                var listapelis = PeliculasRepositorio.GetAll();
                if (listapelis.Any())
                {
                    PeliculaViewModel vm = new()
                    {
                        Horarios = HorarioRepositorio.GetAll().Where(h => h.IdPeliculaNavigation.Nombre == nombre).Select(h => new HorariosModel
                        {
                            Id = h.Id,
                            HorarioDisponible = $"{h.FechaHora} - {h.FechaHora}"
                        }),
                        Pelicula = new PeliculaModel()
                        {
                            Id = peli.Id,
                            Nombre = peli.Nombre,
                            Sinapsis = peli.Sinopsis,
                            //Informacion = $"{peli.IdClasificacionNavigation.Nombre} | {peli.Duracion} | {peli.PeliculaGenero.All(pg => !string.IsNullOrWhiteSpace(pg.IdGeneroNavigation.Nombre))}"
                        },
                        OtrasPeliculas = listapelis.Where(p => p.Nombre != nombre).OrderBy(p => r.Next(0, listapelis.Count())).Take(4).Select(p => new OtrasPeliculasModel
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Año = p.Año
                        })
                    };
                    return View(vm);
                }
            }
            return RedirectToAction("Index");
        }
    }
}