using NuGet.Protocol.Core.Types;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculas
    {
        public readonly Repository<Pelicula> Repositorio;
        public RepositorioPeliculas(Repository<Pelicula> repository)
        {
            Repositorio = repository;
        }

        public IEnumerable<Pelicula> GetPeliculasByIdGenero(int genero)
        {
            return Repositorio.GetAll().Where(x=>x.IdGeneros == genero);
        }

        public Pelicula? GetPeliculaByNombre(string nombre)
        {

            return Repositorio.Get(nombre);
        }
        
    }
}
