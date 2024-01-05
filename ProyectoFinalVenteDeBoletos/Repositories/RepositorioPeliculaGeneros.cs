using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using System.Data;
using MySqlConnection = MySqlConnector.MySqlConnection;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculaGeneros:Repository<PeliculaGenero>
    {
        public Sistem21VentaboletosdbContext Context { get; }
        public RepositorioPeliculaGeneros(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
            Context = Ctx;
        }
        public PeliculaGenero? GetById(int id)
        {
            return GetAll().FirstOrDefault(x=>x.Id == id);
        }
        public override IEnumerable<PeliculaGenero> GetAll()
        {
            return Context.PeliculaGenero.Include(x=>x.IdGeneroNavigation).Include(x => x.IdPeliculaNavigation).ThenInclude(x=>x.IdClasificacionNavigation);
        }
        public IEnumerable<PeliculaGenero> GetPeliculaGeneroByIdPelicula(int idPeli)
        {
            return GetAll().Where(x => x.IdPelicula == idPeli);
        }
        
        public void EliminarGenerosPelicula(IEnumerable<PeliculaGenero> generosPelicula, IEnumerable<int> nuevosGeneros,int idpeli)
        {
            //buscar los generos que esten en la antigua y que no esten en la lista nueva
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
      
            IEnumerable<PeliculaGenero> lista = generosPelicula.Where(gp => gp.IdPelicula == idpeli && !nuevosGeneros.Any(x => x == gp.IdGenero));
            foreach (PeliculaGenero pg in lista)
            {
                Context.PeliculaGenero.Remove(pg);
            }
            Context.SaveChanges();
        }
        public void AgregarNuevosGeneros(IEnumerable<PeliculaGenero> generosPelicula,IEnumerable<int> nuevosGeneros,int idpeli)
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
            foreach (var nuevoGenero in nuevosGeneros)
            {
                //Verificar si existe
                if (!generosPelicula.Any(gp => gp.IdPelicula == idpeli && gp.IdGenero == nuevoGenero))
                {
                    //si no existe se crea y se agrega
                    var g = new PeliculaGenero
                    {
                        IdPelicula = idpeli,
                        IdGenero = nuevoGenero
                    };
                    Insert(g);
                }
            }
        }
        public List<int> GetGenerosSeleccionados(int id)
        {
            return Context.PeliculaGenero.Where(x=>x.IdPelicula == id).Select(x=>x.IdGenero).ToList();
        }
        private static void CerrarConexionMySql()
        {
            MySqlConnection connection = new();
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}