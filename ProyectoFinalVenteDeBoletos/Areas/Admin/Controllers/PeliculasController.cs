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
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Nombre))
            {
                ModelState.AddModelError("", "La pelicula debe tener un nombre");
            }
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Sinopsis))
            {
                ModelState.AddModelError("", "La pelicula debe tener una sinopsis");
            }
            if (vm.Pelicula.Duracion.Hour > 23 || vm.Pelicula.Duracion.Hour < 0)
            {
                ModelState.AddModelError("", "La pelicula debe durar menos de 23 horas");
            }
            if (vm.Pelicula.Año < 1850)
            {
                ModelState.AddModelError("", "No existian peliculas antes de 1850, Ingrese una fecha valida");
            }
            if(vm.Pelicula.Año > DateTime.Now.Year)
            {
                ModelState.AddModelError("", "No se puedes registrar una pelicula con un año superior al actual");
            }
            if (vm.Pelicula.Precio<0 || vm.Pelicula.Precio>5000)
            {
                ModelState.AddModelError("", "Ingrese un precio entre 0 y 5000");
            }
            if (ModelState.IsValid)
            {
                var p = new Pelicula()
                {
                    Id=0,
                    Año = vm.Pelicula.Año,
                    Duracion= vm.Pelicula.Duracion,
                    IdClasificacion = vm.Pelicula.IdClasificacion,
                    Nombre= vm.Pelicula.Nombre,
                    Sinopsis = vm.Pelicula.Sinopsis,
                    Trailer = vm.Pelicula.Trailer,
                    Precio = vm.Pelicula.Precio,
                };
                PeliculasRepositorio.Insert(p);
                //Redireccionar si se agrego correctamente
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
            }
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        [HttpGet("/{id}")]
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
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Nombre))
            {
                ModelState.AddModelError("", "La pelicula debe tener un nombre");
            }
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Sinopsis))
            {
                ModelState.AddModelError("", "La pelicula debe tener una sinopsis");
            }
            if (vm.Pelicula.Duracion.Hour > 23 || vm.Pelicula.Duracion.Hour < 0)
            {
                ModelState.AddModelError("", "La pelicula debe durar menos de 23 horas");
            }
            if (vm.Pelicula.Año < 1850)
            {
                ModelState.AddModelError("", "No existian peliculas antes de 1850, Ingrese una fecha valida");
            }
            if (vm.Pelicula.Año > DateTime.Now.Year)
            {
                ModelState.AddModelError("", "No se puedes registrar una pelicula con un año superior al actual");
            }
            if (vm.Pelicula.Precio < 0 || vm.Pelicula.Precio > 5000)
            {
                ModelState.AddModelError("", "Ingrese un precio entre 0 y 5000");
            }
            if (ModelState.IsValid)
            {
                var anterior = PeliculasRepositorio.Get(id);
                if (anterior != null)
                {
                    anterior.Año = vm.Pelicula.Año;
                    anterior.Duracion = vm.Pelicula.Duracion;
                    anterior.IdClasificacion = vm.Pelicula.IdClasificacion;
                    anterior.Nombre = vm.Pelicula.Nombre;
                    anterior.Sinopsis = vm.Pelicula.Sinopsis;
                    anterior.Trailer = vm.Pelicula.Trailer;
                    anterior.Precio = vm.Pelicula.Precio;
                    
                    PeliculasRepositorio.Update(anterior);
                    //Redireccionar si se edito correctamente
                    return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
                }
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