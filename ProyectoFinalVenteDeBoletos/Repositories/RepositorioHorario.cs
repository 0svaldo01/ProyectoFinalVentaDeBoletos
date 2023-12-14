using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioHorario : Repository<Horario>
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioHorario(CinemaventaboletosContext ctx) : base(ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Horario> GetAll()
        {
            return Ctx.Horario.Include(x => x.IdPeliculaNavigation).Include(x => x.IdSalaNavigation);
        }
        public IEnumerable<Horario> GetAllOrderByNombrePelicula()
        {
            return GetAll().OrderBy(x => x.IdPeliculaNavigation.Nombre);
        }
        public IEnumerable<Horario> GetAllOrderByClasificacion()
        {
            return GetAll().OrderBy(x => x.IdPeliculaNavigation.IdClasificacionNavigation.Nombre);
        }
        public Horario? GetHorarioById(int id)
        {
            return Ctx.Horario.Find(id);
        }
    }
}