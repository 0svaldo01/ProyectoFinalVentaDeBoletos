namespace ProyectoFinalVentaDeBoletos.Models.ViewModels
{
    public class PeliculaViewModel
    {
        public int IdHorario { get; set; }
        public PeliculaModel Pelicula { get; set; } = null!;
        public IEnumerable<OtrasPeliculasModel> OtrasPeliculas { get; set; } = null!;
        public IEnumerable<HorariosModel> Horarios { get; set; } = null!;
    }
    public class HorariosModel
    {
        public int Id { get; set; }
        public string HorarioDisponible { get; set; } = null!;
    }
    public class PeliculaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Informacion { get; set; } = null!;
        public string Sinopsis { get; set; } = null!;
    }
    public class OtrasPeliculasModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Año { get; set; }
    }
}
