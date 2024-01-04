using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PeliculasController : Controller
    {
        #region Repositorios
        public RepositorioPeliculas PeliculasRepositorio { get; }
        public RepositorioPeliculaGeneros PeliculaGenerosRepositorio { get; }
        public RepositorioClasificaciones ClasificacionesRepositorio { get; }
        public RepositorioHorarios HorariosRepositorio { get; }
        public RepositorioGeneros Generosrepositorio { get; }
        #endregion
        public IWebHostEnvironment Env { get; }
        public PeliculasController(
        #region Inyeccion de repositorios
            RepositorioPeliculas repositorioPeliculas,
            RepositorioPeliculaGeneros repositorioPeliculaGeneros,
            RepositorioClasificaciones repositorioClasificaciones,
            RepositorioGeneros repositorioGeneros,
            RepositorioHorarios repositorioHorarios,
            IWebHostEnvironment env
        #endregion
        )
        {
            #region Asignacion de repositorios
            PeliculasRepositorio = repositorioPeliculas;
            PeliculaGenerosRepositorio = repositorioPeliculaGeneros;
            ClasificacionesRepositorio = repositorioClasificaciones;
            HorariosRepositorio = repositorioHorarios;
            Generosrepositorio = repositorioGeneros;
            #endregion
            Env = env;
        }

        [HttpGet("Admin/Peliculas")]
        public IActionResult Index()
        {
            PeliculasViewModel vm = new()
            {
                Peliculas = PeliculasRepositorio.GetAll().Select(x=> new PeliculaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                }),
                
            };
            return View(vm);
        }
        #region CRUD
        #region Read
        [HttpGet("Admin/Pelicula/Agregar")]
        public IActionResult Agregar()
        {
            AgregarPeliculaViewModel vm = new()
            {
                Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                }),
                Generos = Generosrepositorio.GetAll().Select(x => new GeneroModel
                {
                    IdGenero = x.Id,
                    Nombre = x.Nombre
                }),
                Pelicula = new(),
                GenerosSeleccionados = new(),
            };
            return View(vm);
        }
        [HttpGet("Admin/Pelicula/Editar/{id}")]
        public IActionResult Editar(int id)
        {
            var peli = PeliculasRepositorio.Get(id);
            if (peli == null)
            {
                return RedirectToAction("");
            }
            AgregarPeliculaViewModel vm = new()
            {
                Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                }),
                Generos = Generosrepositorio.GetAll().Select(x => new GeneroModel
                {
                    IdGenero = x.Id,
                    Nombre = x.Nombre
                }),
                Pelicula = peli
            };
            return View(vm);
        }
        [HttpGet("Admin/Pelicula/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var peli = PeliculasRepositorio.GetPeliculaById(id);
            if (peli == null)
            {
                return RedirectToAction("Index");
            }
            EliminarPeliculaViewModel vm = new()
            {
                Id = peli.Id,
                Nombre = peli.Nombre
            };
            return View(vm);
        }
        #endregion
        #region Create
        [HttpPost("Admin/Pelicula/Agregar")]
        public IActionResult Agregar(AgregarPeliculaViewModel vm)
        {
            ModelState.Clear();
           
            vm.Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });

            vm.Generos = Generosrepositorio.GetAll().Select(x => new GeneroModel
            {
                IdGenero = x.Id,
                Nombre = x.Nombre
            });
            var peli = PeliculasRepositorio.GetPeliculaByNombre(vm.Pelicula.Nombre);
            #region Validacion
            if (peli != null)
            {
                ModelState.AddModelError("", "La pelicula ya esta registrada");
            }
            //Validar
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Nombre))
            {
                ModelState.AddModelError("", "La pelicula debe tener un nombre");
            }
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Sinopsis))
            {
                ModelState.AddModelError("", "La pelicula debe tener una sinopsis");
            }
            if (vm.Pelicula.Duracion.Hour > 23 || vm.Pelicula.Duracion.Hour < 0)
            {
                ModelState.AddModelError("", "La pelicula debe durar menos de 23 horas");
            }
            if (vm.Pelicula.Año < 1850)
            {
                ModelState.AddModelError("", "No existian peliculas antes de 1850, Ingrese una fecha valida");
            }
            if(vm.Pelicula.Año > DateTime.Now.Year)
            {
                ModelState.AddModelError("", "No se puedes registrar una pelicula con un año superior al actual");
            }
            if (vm.Pelicula.Precio<0 || vm.Pelicula.Precio>5000)
            {
                ModelState.AddModelError("", "Ingrese un precio entre 0 y 5000");
            }
            if (vm.Imagen == null)
            {
                ModelState.AddModelError("", "Seleccione una imagen");
            }
            else
            {
                if(vm.Imagen.ContentType != "Image/png" && vm.Imagen.ContentType != "Image/jpg")
                {
                    ModelState.AddModelError("", "Seleccione una imagen en formato png o jpg");
                }
            }
            #endregion
            if (ModelState.IsValid)
            {
                #region Agregar Pelicula
                var p = new Pelicula()
                {
                    Id = 0,
                    Año = vm.Pelicula.Año,
                    Duracion = vm.Pelicula.Duracion,
                    IdClasificacion = vm.Pelicula.IdClasificacion,
                    Nombre = vm.Pelicula.Nombre,
                    Sinopsis = vm.Pelicula.Sinopsis,
                    Trailer = vm.Pelicula.Trailer,
                    Precio = vm.Pelicula.Precio,
                };
                PeliculasRepositorio.Insert(p);
                #endregion
                #region Agregar los generos a la pelicula
                foreach (var id in vm.GenerosSeleccionados)
                {
                    var genero = Generosrepositorio.Get(id);
                    if (genero != null) 
                    {
                        var peligenero = new PeliculaGenero()
                        {
                            Id = 0,
                            IdGenero = genero.Id,
                            IdPelicula = p.Id
                        };
                        PeliculaGenerosRepositorio.Insert(peligenero);
                    }
                }
                #endregion
                #region Guardar la imagen
                if (vm.Imagen != null)
                {
                    var imagePath = Path.Combine(Env.WebRootPath, "images", vm.Pelicula.Id.ToString() + ".png");
                    var stream = new FileStream(imagePath, FileMode.Create);
                    vm.Imagen.CopyToAsync(stream).Wait();
                    stream.Close();
                }
                else
                {
                    var noPhoto = Path.Combine(Env.WebRootPath, "images/cinetec.png");
                    var imagePath = Path.Combine(Env.WebRootPath, "images", vm.Pelicula.Id.ToString() + ".png");
                    System.IO.File.Copy(noPhoto, imagePath);
                }
                #endregion
                //Redireccionar al index
                return RedirectToAction("Index");
            }
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        #endregion
        #region Update
        [HttpPost("Admin/Pelicula/Editar/{id}")]
        public IActionResult Editar(int id,AgregarPeliculaViewModel vm)
        {
            ModelState.Clear();
            vm.Clasificaciones = ClasificacionesRepositorio.GetAll().Select(x => new ClasificacionModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            var peli = PeliculasRepositorio.GetPeliculaByNombre(vm.Pelicula.Nombre);
            #region Validacion
            if (peli != null)
            {
                ModelState.AddModelError("", "La pelicula ya esta registrada");
            }
            //Validar
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Nombre))
            {
                ModelState.AddModelError("", "La pelicula debe tener un nombre");
            }
            if (string.IsNullOrWhiteSpace(vm.Pelicula.Sinopsis))
            {
                ModelState.AddModelError("", "La pelicula debe tener una sinopsis");
            }
            if (vm.Pelicula.Duracion.Hour > 23 || vm.Pelicula.Duracion.Hour < 0)
            {
                ModelState.AddModelError("", "La pelicula debe durar menos de 23 horas");
            }
            if (vm.Pelicula.Año < 1850)
            {
                ModelState.AddModelError("", "No existian peliculas antes de 1850, Ingrese una fecha valida");
            }
            if (vm.Pelicula.Año > DateTime.Now.Year)
            {
                ModelState.AddModelError("", "No se puedes registrar una pelicula con un año superior al actual");
            }
            if (vm.Pelicula.Precio < 0 || vm.Pelicula.Precio > 5000)
            {
                ModelState.AddModelError("", "Ingrese un precio entre 0 y 5000");
            }
            if (vm.Imagen == null)
            {
                ModelState.AddModelError("", "Seleccione una imagen");
            }
            else
            {
                if (vm.Imagen.ContentType != "Image/png" && vm.Imagen.ContentType != "Image/jpg")
                {
                    ModelState.AddModelError("", "Seleccione una imagen en formato png o jpg");
                }
            }
            #endregion
            if (ModelState.IsValid)
            {
                var antigua = PeliculasRepositorio.Get(vm.Pelicula.Id);
                if (antigua != null)
                {
                    antigua.IdClasificacion = vm.Pelicula.IdClasificacion;
                    antigua.Nombre = vm.Pelicula.Nombre;
                    antigua.Sinopsis = vm.Pelicula.Sinopsis;
                    antigua.Año = vm.Pelicula.Año;
                    antigua.Duracion = vm.Pelicula.Duracion;
                    antigua.Trailer = vm.Pelicula.Trailer;
                    antigua.Precio = vm.Pelicula.Precio;
                    PeliculasRepositorio.Update(antigua);
                    #region Agregar los generos a la pelicula

                    var GenerosPelicula = PeliculaGenerosRepositorio.GetPeliculaGeneroByIdPelicula(antigua.Id);
                    //Identificar nuevos generos
                    var NuevosGeneros = GenerosPelicula.Where(x => !vm.GenerosSeleccionados.Any(g => g == x.IdGenero));
                    //Identificar generos por eliminar
                    var GenerosAEliminar = vm.GenerosSeleccionados.Where(x => GenerosPelicula.Any(g => x != g.IdGenero));
                    //
                    foreach (var item in GenerosPelicula)
                    {
                        //Obtener Nuevo enlace entre Pelicula y genero
                        var NuevoPeliculaGenero = NuevosGeneros.FirstOrDefault(x => x.IdGenero == item.IdGenero);
                        
                        item.IdGenero = NuevoPeliculaGenero != null ? NuevoPeliculaGenero.IdGenero : 0;
                        item.IdPelicula = NuevoPeliculaGenero != null ? NuevoPeliculaGenero.IdPelicula : 0;
                        var anterior = PeliculaGenerosRepositorio.Get(item.IdGenero);
                        //Agregar enlace si no existe
                        if(anterior == null)
                        {
                            PeliculaGenerosRepositorio.Insert(item);
                        }
                        //Editarlo si ya existe
                        else
                        {
                            PeliculaGenerosRepositorio.Update(anterior);
                        }
                    }
                    foreach (var item in GenerosAEliminar)
                    {
                        var anterior = PeliculaGenerosRepositorio.Get(item);
                        if (anterior != null)
                        {
                            PeliculaGenerosRepositorio.Delete(anterior);
                        }
                    }
                    #endregion
                    #region Guardar la imagen
                    if (vm.Imagen != null)
                    {
                        var imagePath = Path.Combine(Env.WebRootPath, "images", vm.Pelicula.Id.ToString() + ".png");
                        var stream = new FileStream(imagePath, FileMode.Create);
                        vm.Imagen.CopyToAsync(stream).Wait();
                        stream.Close();
                    }
                    else
                    {
                        var noPhoto = Path.Combine(Env.WebRootPath, "images/cinetec.png");
                        var imagePath = Path.Combine(Env.WebRootPath, "images", vm.Pelicula.Id.ToString() + ".png");
                        System.IO.File.Copy(noPhoto, imagePath);
                    }
                    #endregion
                    //Redireccionar al index
                    return RedirectToAction("Index");
                }
            }
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        #endregion
        #region Delete
        [HttpPost("Admin/Pelicula/Eliminar/{id}")]
        public IActionResult Eliminar(EliminarPeliculaViewModel p)
        {
            var peli = PeliculasRepositorio.GetPeliculaById(p.Id);
            if (peli != null)
            {
                PeliculasRepositorio.Delete(peli);
            }
            return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
        }
        #endregion
        #endregion
    }
}