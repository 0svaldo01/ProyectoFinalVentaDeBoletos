using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HorariosController : Controller
    {
        #region Repositorios
        public RepositorioHorarios HorarioRepositorio { get; }
        public RepositorioPeliculas PeliculasRepositorio { get; }
        public RepositorioPeliculaHorario PeliculasHorarioRepositorio { get; }
        public RepositorioSalas SalasRepositorio { get; }
        #endregion
        public HorariosController(
        #region Inyeccion de repositorios
            RepositorioHorarios repositorioHorarios,
            RepositorioPeliculas repositorioPeliculas,
            RepositorioSalas repositorioSalas,
            RepositorioPeliculaHorario repositorioPeliculasHorario
        #endregion
        )
        {
            #region Asignacion de repositorios
            HorarioRepositorio = repositorioHorarios;
            PeliculasRepositorio = repositorioPeliculas;
            PeliculasHorarioRepositorio = repositorioPeliculasHorario;
            SalasRepositorio = repositorioSalas;
            #endregion
        }
        public IActionResult Index()
        {
            HorariosViewModel vm = new()
            {
                Horarios = PeliculasHorarioRepositorio.GetAll().Select(x => new HorariosModel
                {
                    Horario = new HorarioModel
                    {
                        HoraInicio = x.IdHorarioNavigation.HoraInicio.ToShortTimeString(),
                        HoraTerminacion = x.IdHorarioNavigation.HoraTerminacion.ToShortTimeString()
                    },
                    Pelicula = new PeliculasModel
                    {
                        Id = x.IdPeliculaNavigation.Id,
                        Nombre = x.IdPeliculaNavigation.Nombre
                    },
                    Sala = new SalasModel
                    {
                        Id = x.IdHorarioNavigation.IdSala
                    }
                })
            };
            return View(vm);
        }
        #region CRUD
        #region Create
        public IActionResult Agregar(AgregarHorarioViewModel vm)
        {
            //Validar
            var anterior = PeliculasHorarioRepositorio.Get(vm.IdPelicula);
            if (anterior != null)
            {
                ModelState.AddModelError("", "El horario ya ah sido establecido anteriormente");
            }
            if (vm.IdPelicula <= 0)
            {
                ModelState.AddModelError("", "Seleccione una pelicula");
            }
            if (vm.IdHorario <= 0)
            {
                ModelState.AddModelError("", "Seleccione un horario");
            }
            if (ModelState.IsValid)
            {
                PeliculaHorario peliculahorario = new()
                {
                    Id = 0,
                    IdHorario = vm.IdHorario,
                    IdPelicula = vm.IdPelicula
                };
                PeliculasHorarioRepositorio.Insert(peliculahorario);
                //Redireccionar si se agrego correctamente
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
            }
            //Regresar el viewmodel si no se agrego
            return View(vm);

        }
        #endregion
        //Aqui estan las acciones para mostrar el crear, editar y eliminar
        #region Read
        [HttpGet]
        public IActionResult Agregar()
        {
            AgregarHorarioViewModel vm = new()
            {
                Peliculas = PeliculasRepositorio.GetAll(),
                Horarios = HorarioRepositorio.GetAll()
            };
            return View(vm);
        }
        
        [HttpGet("{id}")]
        public IActionResult Editar(int id)
        {
            var anterior = PeliculasHorarioRepositorio.GetById(id);
            if (anterior != null)
            {
                AgregarHorarioViewModel vm = new()
                {
                    //Pelicula y Horario seleccionados anteriormente
                    IdHorario = anterior.IdHorario,
                    IdPelicula = anterior.IdPelicula,
                    //Lista de todas las peliculas y horarios excepto los seleccionados anteriormente
                    Horarios = HorarioRepositorio.GetAll().Where(x => x.Id != anterior.IdHorario),
                    Peliculas = PeliculasRepositorio.GetAll().Where(x => x.Id != anterior.IdPelicula)
                };

                if (vm != null)
                    return View(vm);
            }
            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public IActionResult Eliminar(int id)
        {
            if (id >= 0)
            {
                var peliculahorario = PeliculasHorarioRepositorio.GetById(id);
                return View(peliculahorario);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpPost]
        public IActionResult Editar(AgregarHorarioViewModel vm)
        {
            vm.Horarios = HorarioRepositorio.GetAll();
            vm.Peliculas = PeliculasRepositorio.GetAll();
            //Validar
            if (vm.IdPelicula <= 0 || vm.IdHorario <= 0)
            {
                return RedirectToAction("", "", new { });
            }
            if (vm.IdPelicula <= 0)
            {
                ModelState.AddModelError("", "Seleccione una pelicula");
            }
            if (vm.IdHorario <= 0)
            {
                ModelState.AddModelError("", "Seleccione un horario");
            }
            //Si no hay peliculas o horarios
            if (!vm.Peliculas.Any() || !vm.Horarios.Any())
            {
                ModelState.AddModelError("", "No hay peliculas o horarios disponibles");
            }
            var anterior = PeliculasHorarioRepositorio.GetAnterior(vm.IdPelicula, vm.IdHorario);
            if (anterior == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                anterior.IdHorario = vm.IdHorario;
                anterior.IdPelicula = vm.IdPelicula;
                //Las peliculas y horarios se agregan automaticamente utilizando las ID
                PeliculasHorarioRepositorio.Update(anterior);
                //Redireccionar si se agrego correctamente
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
            };
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        #endregion
        #region Delete
        [HttpPost]
        public IActionResult Eliminar(PeliculaHorario p)
        {
            if (p != null)
            {
                var anterior = PeliculasHorarioRepositorio.GetAnterior(p.IdPelicula, p.IdHorario);
                if (anterior != null)
                {
                    PeliculasHorarioRepositorio.Delete(anterior);
                }
            }
            return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
        }
        #endregion
        #endregion
    }
}