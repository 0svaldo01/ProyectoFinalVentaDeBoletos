using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculas
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioPeliculas(CinemaventaboletosContext ctx)
        {
            Ctx = ctx;
        }
        public IEnumerable<Pelicula> GetAll()
        {
            return Ctx.Pelicula
                .Include(x => x.IdClasificacionNavigation)
                .Include(x => x.IdGenerosNavigation)
                //Ordenar por nombre de clasificacion
                .OrderBy(x => x.Nombre);
        }
        public IEnumerable<Pelicula> GetPeliculasOrdenadasByIdNombre()
        {
            return GetAll()
                //Ordenar por nombre de pelicula
                .OrderBy(x => x.Nombre);
        }
        public IEnumerable<Pelicula> GetPeliculasPorGeneroOrdenadasPorIdGenero(int genero)
        {
            return GetAll().Where(x=>x.IdGeneros == genero)
                //Ordenar por Generos
                .OrderBy(x=>x.IdGeneros);
        }
        public Pelicula? GetPeliculaById(int id)
        {
            return Ctx.Pelicula.Find(id);
        }
        public Pelicula? GetPeliculaByNombre(string nombre)
        {
            return Ctx.Pelicula.Find(nombre);
        }


        public void Insert(Pelicula entity)
        {
            Ctx.Add(entity);
            Ctx.SaveChanges();
        }
        public void Update(Pelicula entity)
        {
            Ctx.Update(entity);
            Ctx.SaveChanges();
        }
        public void Delete(object id)
        {
            Ctx.Remove(id);
            Ctx.SaveChanges();
        }
    }
}