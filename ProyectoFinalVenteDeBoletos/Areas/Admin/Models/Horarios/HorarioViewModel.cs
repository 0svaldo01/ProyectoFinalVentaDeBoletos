namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios
{
    public class HorarioViewModel
    {
        public IEnumerable<HorariosModel> Horarios { get; set; } = null!;
        public IEnumerable<PeliculasModel> Peliculas { get; set; } = null!;
        public IEnumerable<SalasModel> Salas { get; set; } = null!;
    }
    public class HorariosModel 
    {
        public string HoraInicio { get; set; }

        public string HoraTerminacion { get; set; }

    }
    public class PeliculasModel 
    {
        public string Nombre { get; set; } = null!;
    }
    public class SalasModel 
    {
        public int Id { get; set; }
    }
}
