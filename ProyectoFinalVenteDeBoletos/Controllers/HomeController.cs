using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalVenteDeBoletos.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
