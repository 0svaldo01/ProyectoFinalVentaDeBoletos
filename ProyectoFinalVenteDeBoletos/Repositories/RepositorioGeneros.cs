using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioGeneros:Repository<Genero>
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioGeneros(CinemaventaboletosContext ctx):base(ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Genero> GetAll()
        {
            return Ctx.Genero.Include(x => x.PeliculaGenero);
        }
        public IEnumerable<Genero> GetAllOrderByNombre()
        {
            return GetAll().OrderBy(g => g.Nombre);
        }
    }
}
