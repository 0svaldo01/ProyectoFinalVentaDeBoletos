using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios
{
    public class AgregarHorarioViewModel
    {
        string HoraInicio { get; set; } = null!;
        string HoraTermino { get; set; } = null!;
        public int IdSala { get; set; }
        public IEnumerable<Sala> Salas { get; set; } = null!;
        public int IdPelicula {  get; set; }
        public IEnumerable<Pelicula> Peliculas { get; set; } = null!;
        public int IdHorario { get; set; }
        public IEnumerable<Horario> Horarios { get; set; } = null!;
    }
}
