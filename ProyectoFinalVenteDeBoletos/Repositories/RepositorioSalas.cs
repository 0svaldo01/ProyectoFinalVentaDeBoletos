using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioSalas : Repository<Sala>
    {
        private readonly Sistem21VentaboletosdbContext Ctx;
        public RepositorioSalas(Sistem21VentaboletosdbContext ctx) : base(ctx)
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
