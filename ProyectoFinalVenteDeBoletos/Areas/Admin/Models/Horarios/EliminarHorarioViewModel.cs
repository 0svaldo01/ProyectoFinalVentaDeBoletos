using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios
{
    public class EliminarHorarioViewModel
    {
        public int Id { get; set; }
        public string Horario { get; set; } = null!;
    }
}
