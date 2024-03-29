﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;
using ProyectoFinalVentaDeBoletos.Repositories;
using System.Security.Claims;
using System.Text;
using ProyectoFinalVentaDeBoletos.Helpers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinalVentaDeBoletos.Controllers
{
    public class HomeController : Controller
    {
        private readonly Random r = new();
        #region Repositorios
        public RepositorioAsientos AsientosRepositorio { get; }
        public RepositorioBoletos BoletosRepositorio { get; }
        private RepositorioClasificaciones ClasificacionRepositorio { get; }
        private RepositorioPeliculas PeliculasRepositorio { get; }
        public RepositorioUsuarios UsuarioRepositorio { get; }
        public RepositorioUsuarioBoletos UsuarioBoletosRepositorio { get; }
        public RepositorioSalas SalasRepositorio { get; }
        #endregion
        public HomeController
        (
        #region Inyeccion de Repositorios
            RepositorioClasificaciones repositorioClasificaciones,
            RepositorioPeliculas repositorioPeliculas,
            RepositorioSalas repositorioSalas,
            RepositorioAsientos repositorioAsientos,
            RepositorioBoletos repositorioBoletos,
            RepositorioUsuarios repositorioUsuarios,
            RepositorioUsuarioBoletos repositorioUsuarioBoletos
        #endregion
        )
        {
            AsientosRepositorio = repositorioAsientos;
            BoletosRepositorio = repositorioBoletos;
            ClasificacionRepositorio = repositorioClasificaciones;
            PeliculasRepositorio = repositorioPeliculas;
            UsuarioRepositorio = repositorioUsuarios;
            SalasRepositorio = repositorioSalas;
            UsuarioBoletosRepositorio = repositorioUsuarioBoletos;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/Nosotros")]
        public IActionResult Nosotros()
        {
            return View();
        }
        //terminado
        #region Peliculas
        [Route("/Peliculas")]
        [Authorize(Roles = "Usuario, Admin")]
        public IActionResult VerPeliculas()
        {
            PeliculasViewModel vm = new()
            {
                Clasificaciones = ClasificacionRepositorio.GetAll().Select(x => new ClasificacionModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Peliculas = x.Pelicula
                    .OrderBy(p => p.Nombre)
                    .Select(p => new PeliculasModel
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Sinopsis = p.Sinopsis
                    })
                })
            };
            return View(vm);
        }
        [HttpGet("/Pelicula/{nombre}")]
        [Authorize(Roles = "Usuario, Admin")]
        public IActionResult Pelicula(string nombre)
        {
            nombre = nombre.Replace('-', ' ');
            var peli = PeliculasRepositorio.GetPeliculaByNombre(nombre);
            if (peli != null)
            {
                var generospeli = peli.PeliculaGenero;
                var generos = new StringBuilder();
                if (generospeli != null)
                {
                    foreach (var genero in generospeli)
                    {
                        if (genero != null)
                        {
                            generos.Append(genero.IdGeneroNavigation.Nombre).Append(',');
                        }
                    }
                }

                var peliculas = PeliculasRepositorio.GetAll();
                PeliculaViewModel vm = new()
                {
                    Pelicula = new PeliculaModel()
                    {
                        Id = peli.Id,
                        Informacion = $"{peli.IdClasificacionNavigation.Nombre} | {peli.Duracion} | {generos}",
                        Nombre = peli.Nombre,
                        Sinopsis = peli.Nombre
                    },
                    Horarios = peli.PeliculaHorario.Select(h => new HorariosModel
                    {
                        Id = h.IdHorarioNavigation.Id,
                        HorarioDisponible = $"{h.IdHorarioNavigation.HoraInicio} - {h.IdHorarioNavigation.HoraTerminacion}",
                    }),

                    OtrasPeliculas = peliculas
                    //Trae las peliculas que no sean la que se muestra
                    .Where(p => p.Id != peli.Id)
                    //Toma 5 al azar
                    .OrderBy(p => r.Next(0, peliculas.Count())).Take(5)
                    .Select(p => new OtrasPeliculasModel
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Año = p.Año,
                    })
                };

                return View(vm);
                
            }
            return RedirectToAction("VerPeliculas");
        }
        #endregion
        // en Proceso
        #region Boletos
        //OK
        [Authorize(Roles = "Usuario, Admin")]
        [HttpGet("/ComprarAsiento/{pelicula}")]
        public IActionResult ComprarAsiento(string pelicula,PeliculaViewModel pvm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(pelicula))
            {
                ModelState.AddModelError("","No existen las peliculas sin nombre");
            }
            if (pvm.IdHorario<=0)
            {
                ModelState.AddModelError("","No hay un horario seleccionado");
            }
            pelicula = pelicula.Replace('-', ' ');
            var peli = PeliculasRepositorio.GetPeliculaByNombre(pelicula);
            if (peli != null) { 
                //Obtenemos la sala usando el id del horario obtenido en el viewmodel
                var sala = SalasRepositorio.GetSalaByHorario(pvm.IdHorario);
                if (sala != null)
                {
                    ComprarAsientoViewModel vm = new()
                    {
                        IdHorario = peli.PeliculaHorario.First().IdHorario,
                        Pelicula = new PeliModel
                        {
                            Nombre = peli.Nombre,
                            Precio = peli.Precio
                        },
                        Sala = new SalaModel
                        {
                            Id = sala.Id,
                            Columnas = sala.Columnas,
                            Filas = sala.Filas,
                            SalaAsientos = sala.SalaAsiento
                            .Where(s => s.IdSalaNavigation.IdSalaAsiento == s.IdSala)
                            .Select(x => x.IdAsientoNavigation)
                            .Select(a => new AsientoModel
                            {
                                Columna = a.Columna,
                                Fila = a.Fila,
                                Id = a.Id,
                                Ocupado = a.Ocupado,
                                Seleccionado = a.Seleccionado
                            })
                        }
                    };

                    if (ModelState.IsValid)
                        return View(vm);
                }
            }
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "Usuario, Admin")]
        [HttpPost("/ComprarAsiento/{pelicula}")]
        public IActionResult ComprarAsiento(string pelicula,ComprarAsientoViewModel vm)
        {
            ModelState.Clear();
            #region Validacion
            if (vm.Pelicula == null)
            {
                ModelState.AddModelError("", "No hay pelicula en el modelo");
            }
            if (vm.Sala == null)
            {
                ModelState.AddModelError("", "No hay sala en el modelo");
            }
            else
            {
                //Si no hay asientos desocupados
                if(!vm.Sala.SalaAsientos.Where(x=>x.Ocupado==false).Any())
                {
                    ModelState.AddModelError("", "No hay asientos disponibles");
                }
            }
            if (vm.IdHorario <= 0)
            {
                ModelState.AddModelError("", "Selecciona un horario");
            }
            #endregion
            var peli = PeliculasRepositorio.GetPeliculaByNombre(pelicula);
            if (peli == null)
            {
                return RedirectToAction("Index");
            }
            if (vm.Sala != null && ModelState.IsValid)
            {
                foreach (var asiento in vm.Sala.SalaAsientos)
                {
                    //Obtener asiento
                    var antiguo = AsientosRepositorio.GetAsiento(asiento.Fila, asiento.Columna);
                    if (antiguo != null)
                    {
                        //Si el asiento esta seleccionado
                        if (asiento.Seleccionado)
                        {
                            //Ocupar Asiento
                            antiguo.Ocupado = true;
                            //quitamos el seleccionado
                            antiguo.Seleccionado = false;
                            //Actualizamos asiento en DB
                            AsientosRepositorio.Update(antiguo);
                        }
                    }
                }
                var usuario = UsuarioRepositorio.Get(int.Parse(User.Claims.First(x => x.Type == "Id").Value));
                if (usuario != null)
                {  
                    //Agregamos el boleto a la tabla
                    Boleto b = new()
                    {
                        Id = 0,
                        IdHorario = vm.IdHorario,
                        IdSala = vm.Sala.Id,
                    };
                    //Si no existe un boletos anterior
                    if (!BoletosRepositorio.GetAll().Where(x=>x.IdHorario == b.IdHorario && x.IdSala == b.IdSala).Any())
                    {
                        //Agregamos el boleto al usuario utilizando la tabla usuarioboleto
                        BoletosRepositorio.Insert(b);
                    }
                    
                    //si ya existe la relacion entre el usuario y el boleto, entonces no se agregara
                    if (!UsuarioBoletosRepositorio.GetAll().Where(x => x.IdBoletos == b.Id && x.IdUsuario == usuario.Id).Any())
                    {
                        //si no existe la relacion entre el usuario y el boleto, se crea
                        UsuarioBoleto ub = new()
                        {
                            Id = 0,
                            IdBoletos = b.Id,
                            IdUsuario = usuario.Id,
                        };
                        if (!UsuarioBoletosRepositorio.GetAll().Where(x => x.IdUsuario == ub.IdUsuario && x.IdBoletos == ub.IdBoletos).Any())
                        {
                            UsuarioBoletosRepositorio.Insert(ub);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
        #endregion
        //terminado
        #region Usuarios
        public IActionResult LogOut()
        {
            //Cerrar sesion
            HttpContext.SignOutAsync();
            //Redirigir al logon
            return RedirectToAction("Login", "Home");
        }
        public IActionResult Denied()
        {
            return View();
        }
        [Route("Home/Login")]
        public IActionResult Login()
        {
            //Mostrar Login
            return View();
        }
        //Utilizando Encriptacion SHA512
        [HttpPost("Home/Login")]
        public IActionResult Login(LoginViewModel vm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.Username))
            {
                ModelState.AddModelError("", "Escriba el nombre del usuario");
            }
            if (string.IsNullOrWhiteSpace(vm.Contraseña))
            {
                ModelState.AddModelError("", "Escriba la contraseña");
            }
            if (ModelState.IsValid)
            {
                var user = UsuarioRepositorio.GetAll().FirstOrDefault(x => x.Username == vm.Username && x.Contraseña == Encriptacion.StringToSHA512(vm.Contraseña));
                if (user == null)
                {
                    ModelState.AddModelError("", "Nombre de usuario o Contraseña Incorrectos.");
                }
                else
                {
                    List<Claim> claims = new() {
                        new Claim("Id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.IdRol == 1 ? "Admin" : "Usuario")
                    };
                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true,  //Login persistente 
                    });
                    
                    //Si es admin redirigir al index de admin
                    if (user.IdRol == 1)
                    {
                        return RedirectToAction("Index", "Home", new { area = "admin" });
                    }
                    //Si no redirecciona al index de Usuario
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            //Quitar la contraseña
            vm.Contraseña = "";
            //Regresar al login si fallo al iniciar sesion
            return View(vm);
        }
        #endregion
    }
}