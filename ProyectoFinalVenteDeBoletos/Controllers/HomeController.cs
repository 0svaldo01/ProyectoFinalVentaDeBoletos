using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using ProyectoFinalVentaDeBoletos.Repositories;
using System.Text;

namespace ProyectoFinalVentaDeBoletos.Controllers
{
    public class HomeController : Controller
    {
        private readonly Random r = new();

        #region Repositorios
        public RepositorioAsientos AsientosRepositorio { get; }
        private RepositorioClasificaciones ClasificacionRepositorio { get; }
        private RepositorioHorarios HorarioRepositorio { get; }
        private RepositorioPeliculas PeliculasRepositorio { get; }
        public RepositorioSalas SalasRepositorio { get; }
        #endregion

        public HomeController
        (
        #region Inyeccion de Repositorios
            RepositorioClasificaciones repositorioClasificaciones,
            RepositorioHorarios repositorioHorarios,
            RepositorioPeliculas repositorioPeliculas,
            RepositorioSalas repositorioSalas,
            RepositorioAsientos repositorioAsientos
        #endregion
        )
        {
            AsientosRepositorio = repositorioAsientos;
            ClasificacionRepositorio = repositorioClasificaciones;
            HorarioRepositorio = repositorioHorarios;
            PeliculasRepositorio = repositorioPeliculas;
            SalasRepositorio = repositorioSalas;

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
            nombre = nombre.Replace('-', ' ');
            var peli = PeliculasRepositorio.GetPeliculaByNombre(nombre);
            if (peli != null)
            {
                var generospeli = peli.PeliculaGenero;
                var generos = new StringBuilder();
                if (generospeli != null)
                {
                    foreach (var genero in generospeli)
                    {
                        generos.Append(genero.IdGeneroNavigation.Nombre).Append(',');
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
                            Informacion = $"{peli.IdClasificacionNavigation.Nombre} | {peli.Duracion.Hour:D2}:{peli.Duracion.Minute:D2}:" +
                            $"{peli.Duracion.Second:D2} | {generos}",
                            Nombre = peli.Nombre,
                            Sinopsis = peli.Sinopsis
                        },

                        Horarios = HorarioRepositorio
                        .GetAll()
                        .Where(x => x.IdPeliculaNavigation.Id == peli.Id)
                        .Select(h => new HorariosModel
                        {
                            Id = h.Id,
                            HorarioDisponible = $"{h.HoraInicio} - {h.HoraTerminacion}"
                        }),

                        OtrasPeliculas = listapeliculas
                            //Ordena aleatoriamente la lista y toma 5 peliculas
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
        [HttpGet("/ComprarAsiento/{pelicula}")]
        public IActionResult ComprarAsiento(string pelicula, ComprarAsientoViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(pelicula))
            {
                return RedirectToAction("Index");
            }
            pelicula = pelicula.Replace('-', ' ');
            var peli = PeliculasRepositorio.GetPeliculaByNombre(pelicula);
            if (peli == null)
            {
                return RedirectToAction("Index");
            }
            vm.Pelicula = new PeliModel
            {
                Nombre = peli.Nombre ?? "",
                Precio = peli.Precio
            };
            var horario = HorarioRepositorio.GetAll().FirstOrDefault(h => h.IdPeliculaNavigation.Nombre == pelicula);
            if (horario != null)
            {
                vm.Sala = new SalaModel()
                {
                    Columnas = horario.IdSalaNavigation.Columnas,
                    Filas = horario.IdSalaNavigation.Filas,
                    Id = horario.IdSalaNavigation.Id,
                    SalaAsientos = AsientosRepositorio.GetAsientosByIdSala(horario.IdSalaNavigation.IdSalaAsiento)
                };
            }
            else
            {
                vm.Sala = new SalaModel() { };   
                ModelState.AddModelError("Error", "No hay horarios disponibles para esta película");
            }
            if (vm.Pelicula == null || vm.Sala == null || vm.Sala.SalaAsientos.Any() || !ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(vm); 
        }
    }
}