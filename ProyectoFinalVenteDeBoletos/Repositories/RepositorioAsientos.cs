using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioAsientos:Repository<Asiento>
    {
        private readonly Sistem21VentaboletosdbContext Context;
        public RepositorioAsientos(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
            this.Context = Ctx;
        }

        public IEnumerable<Asiento> GetAsientos()
        {
            var Asientos = Context.Asiento;
            return Asientos;
        }
    }
}
