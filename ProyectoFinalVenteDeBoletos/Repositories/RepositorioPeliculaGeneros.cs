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
        public IEnumerable<PeliculaGenero> GetGenerosAEliminar(IEnumerable<PeliculaGenero> generosPelicula, IEnumerable<PeliculaGenero> nuevosGeneros)
        {
            //buscar los generos que no esten en la antigua y que esten en la lista nueva
            /* Por ejemplo:
             *  generosPelicula                 --        nuevosGeneros
             *  PeliculaId GeneroId                       PeliculaId  GeneroId   
             *      1          1                               1          3 
             *      1          2                               1          4
             *      1          3                               1          5
             *      
             *  Resultado esperado:
             *  PeliculaId GeneroId 
             *      1          1
             *      1          2      
             */
            return generosPelicula.Where(x=>!nuevosGeneros.Any(pg => pg.IdGenero ==x.IdGenero));
        }
        public IEnumerable<PeliculaGenero> GetGenerosNuevos(IEnumerable<PeliculaGenero> generosPelicula,IEnumerable<PeliculaGenero> nuevosGeneros)
        {
            //Buscar los generos que esten en la nueva lista y que no esten en la lista antigua
            /* Por ejemplo:
             *  generosPelicula                 --        nuevosGeneros
             *  PeliculaId GeneroId                       PeliculaId  GeneroId   
             *      1          1                               1          3 
             *      1          2                               1          4
             *      1          3                               1          5
             *      
             *  Resultado esperado:
             *  PeliculaId GeneroId 
             *      1          4
             *      1          5      
             */
            return nuevosGeneros.Where(x=>!generosPelicula.Any(pg=>pg.IdGenero == x.IdGenero));
        }
        public List<int> GetGenerosSeleccionados(int id)
        {
            return Context.PeliculaGenero.Where(x=>x.IdPelicula == id).Select(x=>x.IdGenero).ToList();
        }
    }
}