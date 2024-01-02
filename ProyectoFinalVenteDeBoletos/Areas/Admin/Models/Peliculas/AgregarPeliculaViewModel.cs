using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas
{
    public class AgregarPeliculaViewModel
    {
        public int IdClasificacion { get; set; }
        public IEnumerable<ClasificacionModel> Clasificaciones { get; set; } = null!;
        public List<int> GenerosSeleccionados { get; set; } = null!;
        public IEnumerable<GeneroModel> Generos { get; set; } = null!;
        public Pelicula Pelicula { get; set; } = null!;
        public IFormFile Imagen { get; set; } = null!;
        //public IFormFile Pelicula { get; set; } = null!;
    }
    public class ClasificacionModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
    public class GeneroModel
    {
        public int IdGenero { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
