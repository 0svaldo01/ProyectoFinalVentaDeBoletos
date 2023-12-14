using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioSalas : Repository<Sala>
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioSalas(CinemaventaboletosContext ctx) : base(ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Sala> GetAll()
        {
            return Ctx.Sala.Include(x => x.IdTipoPantallaNavigation);
        }
        public IEnumerable<Sala> GetSalasOrdenadasByTipoPantalla()
        {
            return GetAll().OrderBy(x => x.IdTipoPantalla);
        }
    }
}
