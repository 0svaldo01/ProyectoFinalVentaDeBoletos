namespace ProyectoFinalVentaDeBoletos.Models.ViewModels
{
    
    public class  PeliculasViewModel
    {
        public IEnumerable<ClasificacionModel> Clasificaciones { get; set; } = null!;
    }
    public class ClasificacionModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public IEnumerable<PeliculaModel> Peliculas { get; set; } = null!;
    }

    public class PeliculaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Sinopsis { get; set; } = null!;
    }
}
