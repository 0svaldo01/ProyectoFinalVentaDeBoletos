using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Repositories;
using System.Text.RegularExpressions;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PeliculasController : Controller
    {
        public RepositorioPeliculas PeliculasRepositorio { get; }
        public RepositorioClasificaciones ClasificacionesRepositorio { get; }

        public PeliculasController(
            RepositorioPeliculas repositorioPeliculas,
            RepositorioClasificaciones repositorioClasificaciones
            )
        {
            PeliculasRepositorio = repositorioPeliculas;
            ClasificacionesRepositorio = repositorioClasificaciones;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Agregar()
        {
            AgregarPeliculaViewModel vm = new()
            {
                Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                }),
                Pelicula = new(),
                Imagen = null!
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(AgregarPeliculaViewModel vm)
        {
            vm.Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            var peli = PeliculasRepositorio.GetPeliculaByNombre(vm.Pelicula.Nombre);
            if (peli != null)
            {
                ModelState.AddModelError("", "La pelicula ya esta registrada");
            }

            //Validar
            if (ModelState.IsValid)
            {
                //Redireccionar si se agrego correctamente
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
            }
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        [HttpGet("/HOLA/{id}")]
        public IActionResult Editar(int id)
        {
            AgregarPeliculaViewModel vm = new()
            {
                Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                }),
                Pelicula = new()
            };
            return View(vm);
        }
        [HttpPost("/{id}")]
        public IActionResult Editar(int id,AgregarPeliculaViewModel vm)
        {
            //Verificar que el id sea solo numeros
            foreach(var n in id.ToString())
            {
                if (!char.IsNumber(n))
                {
                    return RedirectToAction("Index");
                }
            }
            //Validar
            if (ModelState.IsValid)
            {
                //Redireccionar si se edito correctamente
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
            }
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Nombre))
            {
                ModelState.AddModelError("","La pelicula debe tener un nombre");
            }
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Sinopsis))
            {
                ModelState.AddModelError("","La pelicula debe tener una sinopsis");
            }
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Duracion.ToShortTimeString()))
            {

            }

            //Regresar el viewmodel si no se edito
            return View(vm);
        }
        public IActionResult Eliminar(int p)
        {
            return View();
        }
        public IActionResult Eliminar(Pelicula p)
        {
            if (p != null)
            {

            }
            return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
        }
    }
}