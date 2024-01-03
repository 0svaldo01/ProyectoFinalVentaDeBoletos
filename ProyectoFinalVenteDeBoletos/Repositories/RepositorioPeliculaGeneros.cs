using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculaGeneros:Repository<PeliculaGenero>
    {
        public Sistem21VentaboletosdbContext Context { get; }
        public RepositorioPeliculaGeneros(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
            Context = Ctx;
        }

        public override IEnumerable<PeliculaGenero> GetAll()
        {
            return Context.PeliculaGenero.Include(x=>x.IdPeliculaNavigation).Include(x=>x.IdGeneroNavigation);
        }
        public IEnumerable<PeliculaGenero> GetPeliculaGeneroByIdPelicula(int idPeli)
        {
            return GetAll().Where(x => x.IdPelicula == idPeli);
        }
    }
}
