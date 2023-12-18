using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioAsientos:Repository<Asiento>
    {
        private readonly CinemaventaboletosContext Context;
        public RepositorioAsientos(CinemaventaboletosContext Ctx):base(Ctx)
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
