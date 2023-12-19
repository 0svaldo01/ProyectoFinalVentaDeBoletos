namespace ProyectoFinalVentaDeBoletos.Models.ViewModels
{
    public class ComprarAsientoViewModel
    {
        public IEnumerable<AsientoModel> Asientos { get; set; } = null!;
        public PeliModel Pelicula { get; set; } = null!;
    }
    public class AsientoModel
    {
        public int Id { get; set; }
        public int Columna { get; set; }
        public int Fila { get; set; }
        public bool Ocupado { get; set; }
        public bool Seleccionado { get; set; }
    }
    public class PeliModel
    {
        public int Nombre { get; set; }
        public decimal Precio { get; set; }
    }
}
