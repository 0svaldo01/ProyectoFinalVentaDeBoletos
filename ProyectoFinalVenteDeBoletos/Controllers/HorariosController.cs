using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Controllers
{
    public class HorariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AgregarHorario() 
        {
            return View();
        }
        public IActionResult EditarHorario()
        {
            return View();
        }
        public IActionResult EliminarrHorario()
        {
            return View();
        }
    }
}
