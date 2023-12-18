using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    public class PeliculasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
