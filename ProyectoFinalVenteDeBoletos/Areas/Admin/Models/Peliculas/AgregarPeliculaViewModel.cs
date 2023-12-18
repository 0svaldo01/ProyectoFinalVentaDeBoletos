using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas
{
    public class AgregarPeliculaViewModel
    {
        public IEnumerable<ClasificacionModel> Clasificaciones { get; set; } = null!;
        public Pelicula DatosPeli { get; set; } = null!;
        public IFormFile Imagen { get; set; } = null!;
        public IFormFile Pelicula { get; set; } = null!;
    }
    public class ClasificacionModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
