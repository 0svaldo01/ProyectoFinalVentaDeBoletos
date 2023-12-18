using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PeliculasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
