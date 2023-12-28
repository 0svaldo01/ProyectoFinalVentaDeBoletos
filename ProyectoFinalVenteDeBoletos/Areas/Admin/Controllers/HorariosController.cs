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
        public RepositorioHorarios HorarioRepositorio { get; }

        public HorariosController(RepositorioHorarios repositorioHorarios)
        {
            HorarioRepositorio = repositorioHorarios;
        }
        public IActionResult Index()
        {
            HorarioViewModel vm = new();

           // vm.Horarios = HorarioRepositorio.GetAll().OrderBy(x => x.Id).Select(x=> new HorariosModel)


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

        [HttpGet("{id}")]
        public IActionResult Editar(int id)
        {
            var horario = HorarioRepositorio.Get(id);
            if (horario == null)
            {
                return RedirectToAction("Index");
                     
            }
            return View();
        }
        public IActionResult Editar(AgregarHorarioViewModel vm)
        {
            if (vm != null)
            {
                //Validar
                if (ModelState.IsValid)
                {

                    //Redireccionar si se edito correctamente
                    return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
                }
            }
            //Regresar el viewmodel si no se edito
            return RedirectToAction("Index");
        
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