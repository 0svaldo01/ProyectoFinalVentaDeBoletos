using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Repositories;
using System.Data;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    //Finalizado
    public class PeliculasController : Controller
    {
        #region Repositorios
        public RepositorioPeliculas PeliculasRepositorio { get; }
        public RepositorioPeliculaGeneros PeliculaGenerosRepositorio { get; }
        public RepositorioClasificaciones ClasificacionesRepositorio { get; }
        public RepositorioHorarios HorariosRepositorio { get; }
        public RepositorioGeneros Generosrepositorio { get; }
        #endregion
        //Para buscar directorios
        public IWebHostEnvironment Enviroment { get; }
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
            Enviroment = env;
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
                return RedirectToAction("Index");
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
                Pelicula = peli,
                GenerosSeleccionados = PeliculaGenerosRepositorio.GetGenerosSeleccionados(peli.Id)
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
        public async Task<IActionResult> AgregarAsync(AgregarPeliculaViewModel vm)
        {
            ModelState.Clear();
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
                if (vm.Imagen.ContentType != "image/png" && vm.Imagen.ContentType != "image/jpg" && vm.Imagen.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Seleccione una imagen en formato png,jpg o jpeg");
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
                    var imagePath = Path.Combine(Enviroment.WebRootPath, "images", p.Id.ToString() + ".jpg");
                    var stream = new FileStream(imagePath, FileMode.Create);
                    await vm.Imagen.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Close();
                }
                #endregion
                //Redireccionar al index
                return RedirectToAction("Index");
            }
            #region Asignacion de listas en caso de que no agregue
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
            #endregion
            //Regresar el viewmodel si no se agrego
            return View(vm);
        }
        #endregion
        #region Update
        [HttpPost("Admin/Pelicula/Editar/{id}")]
        public async Task<IActionResult> EditarAsync(int id,AgregarPeliculaViewModel vm)
        {
            ModelState.Clear();
            var peli = PeliculasRepositorio.GetPeliculaById(id);
            #region Validacion
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
            //if (vm.Imagen == null)
            //{
            //    ModelState.AddModelError("", "Seleccione una imagen");
            //}
            //else
            //{
            //    if (vm.Imagen.ContentType != "image/png" && vm.Imagen.ContentType != "image/jpg" && vm.Imagen.ContentType != "image/jpeg")
            //    {
            //        ModelState.AddModelError("", "Seleccione una imagen en formato png,jpg o jpeg");
            //    }
            //}
            #endregion
            if (ModelState.IsValid)
            {
                if (peli != null)
                {
                    var antigua = PeliculasRepositorio.Get(peli.Id);
                    if (antigua != null)
                    {
                        #region Reemplazamos con los nuevos datos
                        antigua.Año = vm.Pelicula.Año;
                        antigua.Duracion = vm.Pelicula.Duracion;
                        antigua.IdClasificacion = vm.Pelicula.IdClasificacion;
                        antigua.Nombre = vm.Pelicula.Nombre;
                        antigua.Precio = vm.Pelicula.Precio;
                        antigua.Sinopsis = vm.Pelicula.Sinopsis;
                        antigua.Trailer = vm.Pelicula.Trailer;
                        #endregion
                        //Actualizamos pelicula
                        PeliculasRepositorio.Update(antigua);
                        #region Agregar los generos a la pelicula

                        //Generos antiguos de la pelicula
                        var GenerosPelicula = PeliculaGenerosRepositorio.GetPeliculaGeneroByIdPelicula(antigua.Id);
                        //Metodo implementado para evitar errores de conexiones abiertas con MySql
                        
                            PeliculaGenerosRepositorio.AgregarNuevosGeneros(GenerosPelicula, vm.GenerosSeleccionados, peli.Id);
                            PeliculaGenerosRepositorio.EliminarGenerosPelicula(GenerosPelicula, vm.GenerosSeleccionados, peli.Id);
                        
                        #endregion
                        #region Guardar la imagen
                        if (vm.Imagen != null)
                        {
                            var imagePath = Path.Combine(Enviroment.WebRootPath, "images", antigua.Id.ToString() + ".jpg");
                            var stream = new FileStream(imagePath, FileMode.Create);
                            await vm.Imagen.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.Close();
                        }
                        #endregion
                        //Redireccionar al index
                        return RedirectToAction("Index");
                    }
                }
            }
            #region Asignacion de listas en caso de que no agregue
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
            #endregion
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
                    EliminarImagen($"{p.Id}.jpg");
                }
                return RedirectToAction("Index", "Peliculas", new { Area = "Admin" });
        }
        public void EliminarImagen(string nombreArchivo)
        {
            if (!string.IsNullOrWhiteSpace(nombreArchivo))
            {
                string wwwRootPath = Path.Combine(Enviroment.WebRootPath,"images");
                string rutaArchivo = BuscarArchivoEnDirectorio(nombreArchivo, wwwRootPath)??"";
                if (rutaArchivo != null)
                {
                    //si el archivo existe
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        //Eliminar archivo
                        System.IO.File.Delete(rutaArchivo);
                    }
                }
            }
        }
        private static string? BuscarArchivoEnDirectorio(string nombreArchivo, string directorio)
        {
            // Busca el archivo en el directorio actual
            string rutaArchivo = Path.Combine(directorio, nombreArchivo);
            if (System.IO.File.Exists(rutaArchivo))
            {
                return rutaArchivo;
            }
            #region Busca el archivo en los subdirectorios
            //string[] subdirectorios = Directory.GetDirectories(directorio);

            //foreach (var subdirectorio in subdirectorios)
            //{
            //    rutaArchivo = BuscarArchivoEnDirectorio(nombreArchivo, subdirectorio)??"";

            //    if (rutaArchivo != null)
            //    {
            //        return rutaArchivo;
            //    }
            //}
            #endregion
            // El archivo no se encontró en el directorio ni en sus subdirectorios
            return null;
        }
        #endregion
        #endregion
    }
}