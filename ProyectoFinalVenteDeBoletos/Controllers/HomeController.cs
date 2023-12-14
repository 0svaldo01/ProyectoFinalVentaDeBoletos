using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using ProyectoFinalVentaDeBoletos.Repositories;
using System.Text;

namespace ProyectoFinalVentaDeBoletos.Controllers
{
    public class HomeController : Controller
    {
        private readonly Random r = new();
        private RepositorioClasificaciones ClasificacionRepositorio { get; }
        private RepositorioHorario HorarioRepositorio { get; }
        private RepositorioPeliculas PeliculasRepositorio { get; }
        private RepositorioGeneros Generosrepositorio { get; }

        public HomeController(
            RepositorioClasificaciones repositorioClasificaciones,
            RepositorioHorario repositorioHorarios,
            RepositorioPeliculas repositorioPeliculas,
            RepositorioGeneros repositorioGeneros
            )
        {
            ClasificacionRepositorio = repositorioClasificaciones;
            HorarioRepositorio = repositorioHorarios;
            PeliculasRepositorio = repositorioPeliculas;
            Generosrepositorio = repositorioGeneros;
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
                var generospeli = peli.PeliculaGenero;
                var generos = new StringBuilder();
                if (generospeli!=null)
                {
                    foreach (var genero in generospeli)
                    {
                        generos.Append(genero.IdGeneroNavigation.Nombre).Append(' ');
                    }
                }
                var listapeliculas = PeliculasRepositorio.GetAll();
                if (peli?.Id != null)
                {
                    PeliculaViewModel vm = new()
                    {
                        Pelicula = new PeliculaModel
                        {
                            Id = peli.Id,
                            Informacion = $"{peli.IdClasificacionNavigation.Nombre} | {peli.Duracion.Hour:D2}:{peli.Duracion.Minute:D2}:{peli.Duracion.Second:D2} | {generos}",
                            Nombre = peli.Nombre,
                            Sinopsis = peli.Sinopsis
                        },
                        Horarios = HorarioRepositorio.GetAllOrderByClasificacion().Select(h => new HorariosModel
                        {
                            Id = h.Id,
                            HorarioDisponible = $"{h.HoraInicio} - {h.HoraTerminacion}"
                        }),
                        OtrasPeliculas = listapeliculas
                        .OrderBy(x => r.Next(0, listapeliculas.Count())).Take(5)
                        .Select(op => new OtrasPeliculasModel
                        {
                            Id = op.Id,
                            Año = op.Año,
                            Nombre = op.Nombre
                        })
                    };
                    return View(vm);
                }
            }
            return RedirectToAction("VerPeliculas");
        }
    } 
}