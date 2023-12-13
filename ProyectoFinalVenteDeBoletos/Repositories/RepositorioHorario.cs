using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioHorario:Repository<Horario>
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioHorario(CinemaventaboletosContext ctx):base (ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Horario> GetAll()
        {
            return Ctx.Horario.Include(x => x.IdPeliculaNavigation).Include(x => x.IdSalaNavigation).OrderBy(x=>x.FechaHora);
        }
        public Horario? GetHorarioById(int id)
        {
            return Ctx.Horario.Find(id);
        }
    }
}