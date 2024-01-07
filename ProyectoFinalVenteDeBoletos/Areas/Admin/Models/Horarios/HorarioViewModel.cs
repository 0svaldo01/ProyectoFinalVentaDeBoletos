namespace ProyectoFinalVentaDeBoletos.Areas.Admin.Models.Horarios
{
    public class HorariosViewModel
    {
        public IEnumerable<HorariosModel> Horarios { get; set; } = null!;
    }
    public class HorariosModel
    {
        public int Id { get; set; }
        public HorarioModel Horario { get; set; } = null!;
        public PeliculasModel Pelicula { get; set; } = null!;
        public SalasModel Sala { get; set; } = null!;
    }
    public class HorarioModel 
    {
        public int Id { get; set; } 
        public string HoraInicio { get; set; } = null!;
        public string HoraTerminacion { get; set; } = null!;
    }
    public class PeliculasModel 
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
    public class SalasModel 
    {
        public int Id { get; set; }
    }
}
