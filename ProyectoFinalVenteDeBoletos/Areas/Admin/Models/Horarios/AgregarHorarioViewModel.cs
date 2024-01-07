using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios
{
    public class AgregarHorarioViewModel
    {
        public int Id { get; set; }
        public int IdPelicula {  get; set; }
        public IEnumerable<Pelicula> Peliculas { get; set; } = null!;
        public int IdHorario { get; set; }
        public IEnumerable<HorariovModel> Horarios { get; set; } = null!;
    }
    public class HorariovModel
    {
        public int IdHorario { get; set; }
        public string Horario { get; set; } = null!;
    }
}
