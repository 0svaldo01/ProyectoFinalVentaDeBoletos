using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculas : Repository<Pelicula>
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioPeliculas(CinemaventaboletosContext ctx) : base(ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Pelicula> GetAll()
        {
            return Ctx.Pelicula.Include(x => x.IdClasificacionNavigation).Include(x => x.PeliculaGenero);
        }
        public IEnumerable<Pelicula> GetAllOrderByClasificacion()
        {
            return GetAll().OrderBy(x=> x.IdClasificacionNavigation.Nombre);
        }
        public IEnumerable<Pelicula> GetPeliculasByGenero(string genero)
        {
            return GetAll().Where(x => x.PeliculaGenero.Any(g => g.IdGeneroNavigation.Nombre == genero));
        }
        public IEnumerable<Pelicula> GetPeliculasByClasificacion(string clasificacion)
        {
            return GetAll().Where(x => x.IdClasificacionNavigation.Nombre == clasificacion);
        }
        public Pelicula? GetPeliculaById(int id)
        {
            return Ctx.Pelicula.Find(id);
        }
        public Pelicula? GetPeliculaByNombre(string nombre)
        {
            return Ctx.Pelicula.Include(x=>x.IdClasificacionNavigation).Include(x => x.PeliculaGenero).ThenInclude(pg => pg.IdGeneroNavigation)
                .Where(x => x.Nombre == nombre).FirstOrDefault();
        }
    }
}