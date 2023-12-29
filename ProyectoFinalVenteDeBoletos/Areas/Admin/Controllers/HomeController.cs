using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Helpers;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using System.Security.Claims;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public Repository<RepositorioUsuarios> UserReposiroty { get;}
        public HomeController(Repository<RepositorioUsuarios> UserRepository)
        {
            this.UserReposiroty = UserRepository;
        }
        [HttpGet("/Admin")]
        [HttpGet("/Admin/Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult login()
        {
            return View();
        }

        public IActionResult login(LoginViewModel vm)
        {
            if(string.IsNullOrWhiteSpace(vm.Username))
            {
                ModelState.AddModelError("", "Username incorrecto");
            }
            if (string.IsNullOrWhiteSpace(vm.Contraseña))
            {
                ModelState.AddModelError("", "Contraseña incorrecta");
            }

            if (ModelState.IsValid)
            {
                var user = UserReposiroty.GetAll().FirstOrDefault(x => x.CorreoElectronico == vm.Contraseña
               && x.contraseña == Encriptacion.StringToSHA512(vm.Contraseña));
                if (user == null)
                {
                    ModelState.AddModelError("", "Contraseña o correo electrónico incorrectos");
                }
                else
                {
                    //logear
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role, user.Rol == 1 ? "Administrador" : "Supervisor"));

                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                    return RedirectToAction("Index", "Home", new { area = "Admin" });


                }

            }
                return View(vm);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Denied()
        {
          return View();
        }
    }
}
