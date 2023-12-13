using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioClasificaciones:Repository<Clasificacion>
    {
        private readonly CinemaventaboletosContext Context;
        public RepositorioClasificaciones(CinemaventaboletosContext context) : base(context)
        {
            this.Context = context;
        }
        public override IEnumerable<Clasificacion> GetAll()
        {
            return Context.Clasificacion.Include(x => x.Pelicula).OrderBy(x=>x.Nombre);
        }
        public IEnumerable<Clasificacion> GetClasificaciones()
        {
            return GetAll().OrderBy(x => x.Nombre);
        }
        public Clasificacion? GetClasificacionByNombre(string nombre)
        {
            return Context.Clasificacion.Find(nombre);
        }
        public Clasificacion? GetClasificacionById(int id)
        {
            return Context.Clasificacion.Find(id);
        }
    }
}
