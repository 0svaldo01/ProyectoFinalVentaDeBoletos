using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioSalas
    {
        private readonly CinemaventaboletosContext Ctx;
        public RepositorioSalas(CinemaventaboletosContext ctx)
        {
            Ctx = ctx;
        }
        public IEnumerable<Sala> GetAll()
        {
            return Ctx.Sala.Include(x=>x.IdTipoPantallaNavigation);
        }
        public IEnumerable<Sala> GetSalasOrdenadasByCapacidad()
        {
            return GetAll().OrderBy(x => x.Capacidad);
        }
        public IEnumerable<Sala> GetSalasOrdenadasById()
        {
            return GetAll().OrderBy(x => x.Id);
        }
        public IEnumerable<Sala> GetSalasOrdenadasByTipoPantalla()
        {
            return GetAll().OrderBy(x => x.IdTipoPantalla);
        }
        public void Insert(Sala entity)
        {
            Ctx.Add(entity);
            Ctx.SaveChanges();
        }
        public void Update(Sala entity)
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
