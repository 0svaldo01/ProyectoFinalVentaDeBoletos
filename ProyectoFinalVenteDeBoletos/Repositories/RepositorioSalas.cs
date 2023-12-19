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
        //public IEnumerable<Asiento>? GetSalaByNombrePelicula(string Nombre)
        //{
        //    //busca en la tabla de horario, obtiene la primera 
        //   // var a = Ctx.Sala.Include(x=>x.)
        //    return a;
        //}
        public IEnumerable<Sala> GetSalasOrdenadasByTipoPantalla()
        {
            return GetAll().OrderBy(x => x.IdTipoPantalla);
        }
    }
}
