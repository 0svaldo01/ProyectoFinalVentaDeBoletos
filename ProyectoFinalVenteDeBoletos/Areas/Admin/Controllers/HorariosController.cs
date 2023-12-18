using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HorariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Agregar(AgregarHorarioViewModel vm)
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
 
        public IActionResult Editar(AgregarHorarioViewModel vm)
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
        public IActionResult Eliminar(Horario p)
        {
            if (p != null)
            {

            }
            return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
        
        }
    }
}