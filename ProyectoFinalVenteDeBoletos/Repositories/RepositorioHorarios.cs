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
            return Ctx.Horario
                .Include(x => x.IdSalaNavigation).ThenInclude(x => x.SalaAsiento).ThenInclude(x=>x.IdAsientoNavigation)
                .Include(x=>x.PeliculaHorario).ThenInclude(x=>x.IdPeliculaNavigation)
                .ThenInclude(x=>x.IdClasificacionNavigation);
        }
        public IEnumerable<Horario> GetAllOrderByNombrePelicula()
        {
            return GetAll().OrderBy(x=>x.PeliculaHorario.OrderBy(p=>p.IdPeliculaNavigation.Nombre));
        }
        public IEnumerable<Horario> GetAllOrderByClasificacion()
        {
            return GetAll().OrderBy(x => x.PeliculaHorario.OrderBy(p=>p.IdPeliculaNavigation.IdClasificacionNavigation.Nombre));
        }
        public Horario? GetHorarioById(int id)
        {
            return Ctx.Horario.Find(id);
        }
        public Horario? GetHorarioByNombrePelicula(string nombre)
        {
            return GetAll().FirstOrDefault(x => x.PeliculaHorario.Where(p => p.IdPeliculaNavigation.Nombre == nombre).First().IdPelicula == x.IdPelicula);
        }
    }
}