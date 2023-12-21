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
                DatosPeli = new(),
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
            var peli = PeliculasRepositorio.GetPeliculaByNombre(vm.DatosPeli.Nombre);
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
        //public IActionResult Agregar(AgregarPeliculaViewModel vm)
        //{
        //    vm.Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
        //    {
        //        Id = x.Id,
        //        Nombre = x.Nombre
        //    });
        //    var peli = PeliculasRepositorio.GetPeliculaByNombre(vm.DatosPeli.Nombre);
        //    if (peli != null)
        //    {
        //        ModelState.AddModelError("", "La pelicula ya esta registrada");
        //    }
        //    if (vm.Pelicula != null)
        //    {
        //        //Solo acepta videos mp4 y wav
        //        if (vm.Pelicula.ContentType != "video/mp4" && vm.Pelicula.ContentType != "video/wav")
        //        {
        //            //La pelicula tiene que durar menos de 3 horas
        //            if (vm.DatosPeli.Duracion <= new TimeOnly(3))
        //            {
        //                ModelState.AddModelError("", "La pelicula es muy larga");
        //            }
        //        }
        //        if (string.IsNullOrWhiteSpace(vm.DatosPeli.Nombre))
        //        {
        //            ModelState.AddModelError("", "La pelicula tiene que tener un nombre");
        //        }
               
        //    } 

        //    //Validar
        //    if (ModelState.IsValid)
        //    {
        //        //Redireccionar si se agrego correctamente
        //        return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
        //    }
        //    //Regresar el viewmodel si no se agrego
        //    return View(vm);
        //}

        public IActionResult Editar(AgregarPeliculaViewModel vm)
        {
            //Validar
            if (ModelState.IsValid)
            {
                //Redireccionar si se edito correctamente
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
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