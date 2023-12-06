using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioClasificaciones
    {
        public CinemaventaboletosContext Ctx { get; }
        public RepositorioClasificaciones(CinemaventaboletosContext ctx)
        {
            Ctx = ctx;
        }
        public IEnumerable<Clasificacion> GetAll()
        {
            return Ctx.Clasificacion.Include(x => x.Pelicula).OrderBy(x=>x.Nombre);
        }
        public IEnumerable<Clasificacion> GetClasificaciones()
        {
            return GetAll().OrderBy(x => x.Nombre);
        }
        public Clasificacion? GetClasificacionByNombre(string nombre)
        {
            return Ctx.Clasificacion.Find(nombre);
        }

        public Clasificacion? GetClasificacionById(int id)
        {
            return Ctx.Clasificacion.Find(id);
        }
        
    }
}
