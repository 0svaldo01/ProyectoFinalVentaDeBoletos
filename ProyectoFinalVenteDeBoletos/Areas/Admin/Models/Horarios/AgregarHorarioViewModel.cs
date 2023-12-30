using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios
{
    public class AgregarHorarioViewModel
    {
        public int IdPelicula {  get; set; }
        public IEnumerable<Pelicula> Peliculas { get; set; } = null!;
        public int IdHorario { get; set; }
        public IEnumerable<Horario> Horarios { get; set; } = null!;
    }
}
