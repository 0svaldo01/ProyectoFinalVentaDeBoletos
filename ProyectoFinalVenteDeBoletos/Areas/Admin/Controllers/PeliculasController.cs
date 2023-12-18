using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PeliculasController : Controller
    {
        public IActionResult Index()
        {
            PeliculaViewModel vm = new();

            return View(vm);
        }
        public IActionResult Agregar(AgregarPeliculaViewModel vm)
        {
            //Validar
            if (ModelState.IsValid)
            {
                //Redireccionar si se agrego correctamente
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
            }
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
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
        public IActionResult Eliminar(Pelicula p)
        {
            return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
        }
    }
}