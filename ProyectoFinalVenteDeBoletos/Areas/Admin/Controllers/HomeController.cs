using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [HttpGet("/Admin")]
        [HttpGet("/Admin/Home/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
