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
            RepositorioUsuarios repositorioUsuarios
        #endregion
        )
        {
            AsientosRepositorio = repositorioAsientos;
            BoletosRepositorio = repositorioBoletos;
            ClasificacionRepositorio = repositorioClasificaciones;
            PeliculasRepositorio = repositorioPeliculas;
            UsuarioRepositorio = repositorioUsuarios;
            SalasRepositorio = repositorioSalas;
            
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
        [HttpGet("/ComprarAsiento/{pelicula}")]
        public IActionResult ComprarAsiento(string pelicula, string seatNumber, bool isSelected)
        {

            //estoy por ver donde irian las lineas del enlace, ando leyendo documentacion

            // Ejemplo de cómo imprimir la información en la consola del servidor
            Console.WriteLine($"Asiento: {seatNumber}, Seleccionado: {isSelected}");

            // Puedes devolver algún resultado al cliente si es necesario
            return Json(new { success = true });


            if (string.IsNullOrWhiteSpace(pelicula))
            {
                return RedirectToAction("Index");
            }
            pelicula = pelicula.Replace('-', ' ');
            var peli = PeliculasRepositorio.GetPeliculaByNombre(pelicula);
            var sala = SalasRepositorio.GetSalaByNombrePelicula(pelicula);

            if (peli == null || sala == null)
            {
                return RedirectToAction("Index");
            }
            ComprarAsientoViewModel vm = new()
            {
                IdHorario = peli.PeliculaHorario.First(x=>x.IdPelicula == x.IdHorarioNavigation.IdPelicula).Id,
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
            if (ModelState.IsValid) return View(vm);
            return RedirectToAction("Index");
        }
        [HttpPost("/ComprarAsiento/{pelicula}")]
        public IActionResult ComprarAsiento(ComprarAsientoViewModel vm)
        {
            foreach (var asiento in vm.Sala.SalaAsientos)
            {
                var antiguo = AsientosRepositorio.GetAsiento(asiento.Fila, asiento.Columna);
                if (antiguo != null)
                {
                    if (asiento.Seleccionado)
                    {
                        //el seleccionado ya es falso, asi que no se cambia
                        antiguo.Ocupado = true;
                        AsientosRepositorio.Update(antiguo);
                    }
                }
            }
            //Verificar que el vm este completo
            if (vm != null && vm.Pelicula != null && vm.Sala != null && vm.Sala.SalaAsientos != null)
            {
                Boleto b = new()
                {
                    Id = 0,
                    IdHorario = vm.IdHorario,
                    IdSala = vm.Sala.Id,
                };
                if (b!=null) 
                {
                    BoletosRepositorio.Insert(b);
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
        public IActionResult Login()
        {
            //Mostrar Login
            return View();
        }
        //Utilizando Encriptacion SHA512
        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
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
                var user = UsuarioRepositorio.GetAll()
                    .FirstOrDefault(x => x.Username == vm.Username && x.Contraseña == Encriptacion.StringToSHA512(vm.Contraseña));
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