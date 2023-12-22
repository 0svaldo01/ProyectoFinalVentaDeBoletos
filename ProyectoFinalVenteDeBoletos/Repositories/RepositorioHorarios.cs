using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioHorarios : Repository<Horario>
    {
        private readonly Sistem21VentaboletosdbContext Ctx;
        public RepositorioHorarios(Sistem21VentaboletosdbContext ctx) : base(ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Horario> GetAll()
        {
            return Ctx.Horario.Include(x => x.IdPeliculaNavigation).Include(x => x.IdSalaNavigation).ThenInclude(x=>x.IdSalaAsientoNavigation);
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
        public Horario? GetHorarioByNombrePelicula(string nombre)
        {
            return Ctx.Horario.Include(x=>x.IdSalaNavigation).ThenInclude(x=>x.IdSalaAsientoNavigation).FirstOrDefault(x=>x.IdPeliculaNavigation.Nombre == nombre);
        }
    }
}