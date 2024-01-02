namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Peliculas
{
    public class PeliculasViewModel
    {
        public IEnumerable<PeliculaModel> Peliculas { get; set; } = null!;
    }
    public class PeliculaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
