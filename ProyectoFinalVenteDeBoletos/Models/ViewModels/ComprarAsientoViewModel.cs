namespace ProyectoFinalVentaDeBoletos.Models.ViewModels
{
    public class ComprarAsientoViewModel
    {
        public SalaModel Sala { get; set; } = null!;
        public PeliModel Pelicula { get; set; } = null!;
    }
    public class SalaModel
    {
        public int Id { get; set; }
        public int Columnas { get; set; }
        public int Filas { get; set; }
        public IEnumerable<AsientoModel> SalaAsientos { get; set; } = null!;
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
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
    }
}
