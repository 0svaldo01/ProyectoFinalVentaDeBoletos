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
        public RepositorioBoletos BoletosRepositorio { get; }
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
            RepositorioAsientos repositorioAsientos,
            RepositorioBoletos repositorioBoletos
        #endregion
        )
        {
            AsientosRepositorio = repositorioAsientos;
            BoletosRepositorio = repositorioBoletos;
            ClasificacionRepositorio = repositorioClasificaciones;
            PeliculasRepositorio = repositorioPeliculas;
            SalasRepositorio = repositorioSalas;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Nosotros")]
        public IActionResult Nosotros()
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

                var peliculas = PeliculasRepositorio.GetAll();
                PeliculaViewModel vm = new()
                {
                    Pelicula = new PeliculaModel()
                    {
                        Id = peli.Id,
                        Informacion = $"{peli.IdClasificacionNavigation.Nombre} | {peli.Duracion} | {generos}",
                        Nombre = peli.Nombre,
                        Sinopsis = peli.Nombre
                    },
                    Horarios = peli.PeliculaHorario.Select(h => new HorariosModel
                    {
                        Id = h.IdHorarioNavigation.Id,
                        HorarioDisponible = $"{h.IdHorarioNavigation.HoraInicio} - {h.IdHorarioNavigation.HoraTerminacion}",
                    }),

                    OtrasPeliculas = peliculas
                    //Trae las peliculas que no sean la que se muestra
                    .Where(p => p.Id != peli.Id)
                    //Toma 5 al azar
                    .OrderBy(p => r.Next(0, peliculas.Count())).Take(5)
                    .Select(p => new OtrasPeliculasModel
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
        public IActionResult ComprarAsiento(string pelicula)
        {
            if (string.IsNullOrWhiteSpace(pelicula))
            {
                return RedirectToAction("Index");
            }
            pelicula = pelicula.Replace('-', ' ');

            var peli = PeliculasRepositorio.GetPeliculaByNombre(pelicula);
            var sala = SalasRepositorio.GetSalaByNombrePelicula(pelicula);

            if (peli == null || sala == null)
            {
                return RedirectToAction("Index");
            }
            ComprarAsientoViewModel vm = new()
            {
                Pelicula = new PeliModel
                {
                    Nombre = peli.Nombre,
                    Precio = peli.Precio
                },
                Sala = new SalaModel
                {
                    Id = sala.Id,
                    Columnas = sala.Columnas,
                    Filas = sala.Filas,
                    SalaAsientos = sala.SalaAsiento
                    .Where(s => s.IdSalaNavigation.IdSalaAsiento == s.IdSala)
                    .Select(x => x.IdAsientoNavigation)
                    .Select(a => new AsientoModel
                    {
                        Columna = a.Columna,
                        Fila = a.Fila,
                        Id = a.Id,
                        Ocupado = a.Ocupado,
                        Seleccionado = a.Seleccionado
                    })
                }
            };
            if (ModelState.IsValid) return View(vm);
            return RedirectToAction("Index");
        }

        [HttpPost("/ComprarAsiento/{pelicula}")]
        public IActionResult ComprarAsiento(ComprarAsientoViewModel vm)
        {
            //Verificar que el vm este completo
            if(vm!=null && vm.Pelicula != null && vm.Sala!=null && vm.Sala.SalaAsientos != null)
            {
                //BoletosRepositorio.Insert(b);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}