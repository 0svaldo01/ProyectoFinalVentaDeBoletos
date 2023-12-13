using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculas: Repository<Pelicula>
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioPeliculas(CinemaventaboletosContext ctx):base(ctx)  
        {
            Ctx = ctx;
        }

        public override IEnumerable<Pelicula> GetAll()
        {
            return Ctx.Pelicula
                .Include(x => x.IdClasificacionNavigation).Include(x=>x.PeliculaGenero)
                //Ordenar por nombre de clasificacion
                .OrderBy(x => x.PeliculaGenero.First(g=>g.IdGeneroNavigation.Nombre!=null)).ThenBy(x=>x.Nombre);
        }
        public IEnumerable<Pelicula> GetPeliculasByGenero(string genero)
        {
            return GetAll().Where(x => x.PeliculaGenero.Any(g => g.IdGeneroNavigation.Nombre == genero));
        }
        public Pelicula? GetPeliculaById(int id)
        {
            return Ctx.Pelicula.Find(id);
        }
        
        public Pelicula? GetPeliculaByNombre(string nombre)
        {
            return Ctx.Pelicula.Find(nombre);
        }
    }
}