﻿using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("Admin/Horarios")]
        public IActionResult Index()
        {
            HorariosViewModel vm = new()
            {
                Horarios = PeliculasHorarioRepositorio.GetAll().Select(x => new HorariosModel
                {
                    Id = x.Id,
                    Horario = new HorarioModel
                    {
                        Id = x.Id,
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
        #region Read
        [HttpGet("/Admin/Horario/Agregar")]
        public IActionResult Agregar()
        {
            AgregarHorarioViewModel vm = new()
            {
                Peliculas = PeliculasRepositorio.GetAll(),
                Horarios = HorarioRepositorio.GetAll().Select(x=> new HorariovModel
                {
                    IdHorario = x.Id,
                    Horario = $"{x.HoraInicio} - {x.HoraTerminacion}"
                })
            };
            return View(vm);
        }
        [HttpGet("/Admin/Horario/Editar/{id}")]
        public IActionResult Editar(int id)
        {
            var anterior = PeliculasHorarioRepositorio.GetById(id); 
            if (anterior != null)
            {
                AgregarHorarioViewModel vm = new()
                {
                    Id = anterior.Id,
                    IdHorario = anterior.IdHorario,
                    IdPelicula = anterior.IdPelicula,
                    Horarios = HorarioRepositorio.GetAll().Select(x=> new HorariovModel
                    {
                        IdHorario = x.Id,
                        Horario = $"{x.HoraInicio} - {x.HoraTerminacion}"
                    }),
                    Peliculas = PeliculasRepositorio.GetAll().Where(x => x.Id != anterior.IdPelicula)
                };

                if (vm != null)
                    return View(vm);
            }
            return RedirectToAction("Index");
        }

        [HttpGet("Admin/Horario/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            if (id >= 0)
            {
                var peliculahorario = PeliculasHorarioRepositorio.GetById(id);
                if (peliculahorario != null)
                {
                    EliminarHorarioViewModel vm = new()
                    {
                        Id = peliculahorario != null ? peliculahorario.Id : 0,
                        Horario = peliculahorario != null ? $"{peliculahorario.IdHorarioNavigation.HoraInicio} - {peliculahorario.IdHorarioNavigation.HoraTerminacion}" : ""
                    };
                    return View(vm);
                }
            }
            return RedirectToAction("Index");
        }
        #endregion
        //Aqui estan las acciones para mostrar el crear, editar y eliminar
        #region Create
        [HttpPost("/Admin/Horario/Agregar")]
        public IActionResult Agregar(AgregarHorarioViewModel vm)
        {
            //por alguna razon, recibe el ModelState tiene errores cuando entra, para evitar que de errores limpiamos el ModelState
            ModelState.Clear();
            //Validar (Aqui agregamos los errores que tenga el modelo al ModelState)
            var anterior = PeliculasHorarioRepositorio.Get(vm.IdPelicula);
            vm.Peliculas = PeliculasRepositorio.GetAll();
            vm.Horarios = HorarioRepositorio.GetAll().Select(x => new HorariovModel
            {
                IdHorario = x.Id,
                Horario = $"{x.HoraInicio} - {x.HoraTerminacion}"
            });

            if (anterior != null)
            {
                ModelState.AddModelError("", "El horario ya ha sido establecido anteriormente");
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
                var relacion = PeliculasHorarioRepositorio.GetAnterior(vm.IdPelicula,vm.IdHorario);
                if (relacion == null)
                {
                    //Creamos la relacion entre la pelicula y el horario
                    PeliculaHorario peliculahorario = new()
                    {
                        Id = 0,
                        IdHorario = vm.IdHorario,
                        IdPelicula = vm.IdPelicula
                    };
                    PeliculasHorarioRepositorio.Insert(peliculahorario);
                    //Redireccionar si se agrego correctamente
                    return RedirectToAction("Index", "Horarios", new { Area = "Admin" });
                }
            }
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        #endregion      
        #region Update
        [HttpPost("/Admin/Horario/Editar/{id}")]
        public IActionResult Editar(int id,AgregarHorarioViewModel vm)
        {
            //Validar
            if (vm.IdPelicula <= 0 || vm.IdHorario <= 0)
            {
                //Esto no redirecciona a nada
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
            var anterior = PeliculasHorarioRepositorio.GetById(id);

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
                return RedirectToAction("Index", "Horarios", new { Area = "Admin" });
            };
            vm.Horarios = HorarioRepositorio.GetAll().Select(x => new HorariovModel
            {
                IdHorario = x.Id,
                Horario = $"{x.HoraInicio} - {x.HoraTerminacion}"
            });
            vm.Peliculas = PeliculasRepositorio.GetAll();
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        #endregion
        #region Delete
        [HttpPost("/Admin/Horario/Eliminar/{id}")]
        public IActionResult Eliminar(int id,EliminarHorarioViewModel p)
        {
            if (id > 0)
            {
                if (p != null)
                {
                    var anterior = PeliculasHorarioRepositorio.Get(p.Id);
                    if (anterior != null)
                    {
                        PeliculasHorarioRepositorio.Delete(anterior);
                    }
                }
            }
            return RedirectToAction("Index", "Horarios", new { Area = "Admin" });
        }
        #endregion
        #endregion
    }
}