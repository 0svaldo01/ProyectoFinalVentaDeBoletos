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
        public IEnumerable<PeliculasModel> Peliculas { get; set; } = null!;
    }
    public class PeliculasModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Sinopsis { get; set; } = null!;
    }
}