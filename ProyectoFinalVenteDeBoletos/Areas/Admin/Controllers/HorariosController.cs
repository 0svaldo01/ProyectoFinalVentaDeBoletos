using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HorariosController : Controller
    {
        public RepositorioHorarios HorarioRepositorio { get; }
        public RepositorioPeliculas PeliculasRepositorio { get; }
        public RepositorioSalas SalasRepositorio { get; }

        public HorariosController(RepositorioHorarios repositorioHorarios, RepositorioPeliculas repositorioPeliculas,RepositorioSalas repositorioSalas)
        {
            HorarioRepositorio = repositorioHorarios;
            PeliculasRepositorio = repositorioPeliculas;
            SalasRepositorio = repositorioSalas;
        }
        public IActionResult Index()
        {
            HorariosViewModel vm = new()
            {
                Horarios = PeliculasRepositorio.GetAll().Select(x => new HorariosModel
                {
                    Horario = x.PeliculaHorario.Select(h => new HorarioModel
                    {
                        HoraInicio = h.IdHorarioNavigation.HoraInicio.ToString(),
                        HoraTerminacion = h.IdHorarioNavigation.HoraTerminacion.ToString()
                    })
                    //Suponiendo que tenga mas de un horario
                    .First(),
                    Pelicula = new PeliculasModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre
                    },
                    Sala = x.PeliculaHorario.Select(x => x.IdHorarioNavigation).Select(s => new SalasModel
                    {
                        Id = s.IdSala
                    })
                    //Suponiendo que utiliza mas de una sala
                    .First()
                })
            };
            return View(vm);
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