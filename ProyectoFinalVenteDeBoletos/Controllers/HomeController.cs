using Microsoft.AspNetCore.Mvc;
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
            PeliculasRepositorio = repositorioPeliculas;
            SalasRepositorio = repositorioSalas;

        }
        public IActionResult Index()
        {
            return View();
        }
        #region Peliculas
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
                        if (genero != null)
                        {
                            generos.Append(genero.IdGeneroNavigation.Nombre).Append(',');
                        }
                    }
                }

                PeliculaViewModel vm = new()
                {
                    Pelicula = new PeliculaModel()
                    {
                        Id = peli.Id,
                        Informacion = $"{peli.IdClasificacionNavigation.Nombre} | {peli.Duracion} | {generos}",
                        Nombre = peli.Nombre,
                        Sinopsis = peli.Nombre
                    },
                    Horarios = peli.PeliculaHorario.Select(h=> new HorariosModel
                    {
                        Id = h.IdHorarioNavigation.Id,
                        HorarioDisponible = $"{h.IdHorarioNavigation.HoraInicio} - {h.IdHorarioNavigation.HoraTerminacion}",
                    }),
                    OtrasPeliculas = PeliculasRepositorio.GetAll().Select(p => new OtrasPeliculasModel
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Año = p.Año,
                    })
                };

                return View(vm);
                
            }
            return RedirectToAction("VerPeliculas");
        }
        #endregion

        #region Boletos
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
            //if (horario != null)
            //{
                
                //vm.Sala = new()
                //{
                //    Columnas = horario.IdSalaNavigation.Columnas,
                //    Filas = horario.IdSalaNavigation.Filas,
                //    Id = horario.IdSalaNavigation.Id,
                //    SalaAsientos = AsientosRepositorio.GetAsientosByIdSala(horario.IdSalaNavigation.IdSalaAsiento)
                //};
            //}
            //else
            //{
            //    ModelState.AddModelError("Error", "No hay horarios disponibles para esta película");
            //}
            if (vm.Pelicula == null || vm.Sala == null || !vm.Sala.SalaAsientos.Any() || !ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(vm);
        }
        #endregion
    }
}