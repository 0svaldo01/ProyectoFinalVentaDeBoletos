using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioAsientos:Repository<Asiento>
    {
        public Sistem21VentaboletosdbContext Context { get; }
        public RepositorioAsientos(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
            Context = Ctx;
        }
        public Asiento? GetAsiento(int fila, int columna)
        {
            return Context.Asiento.FirstOrDefault(x => x.Fila == fila && x.Columna == columna);
        }
    }
}
