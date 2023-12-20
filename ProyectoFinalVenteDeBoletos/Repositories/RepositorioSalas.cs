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
        public Sala? GetSalaByNombrePelicula(string Nombre)
        {
            //busca en la tabla de horario, obtiene la primera 
            var a = Ctx.Horario
                .Include(x=>x.IdPeliculaNavigation)
                .Include(x=>x.IdSalaNavigation)
                .ThenInclude(x=>x.IdSalaAsientoNavigation)
                .ThenInclude(x=>x.IdAsientoNavigation)
                .FirstOrDefault(x=>x.IdPeliculaNavigation.Nombre == Nombre)?.IdSalaNavigation;
            return a;
        }
        public IEnumerable<Sala> GetSalasOrdenadasByTipoPantalla()
        {
            return GetAll().OrderBy(x => x.IdTipoPantalla);
        }
    }
}
