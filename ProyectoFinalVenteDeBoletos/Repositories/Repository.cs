using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVenteDeBoletos.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly CinemaventaboletosContext Ctx;
        public Repository(CinemaventaboletosContext ctx)
        {
            Ctx = ctx;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return Ctx.Set<T>();
        }
        public virtual T? Get(object id)
        {
            return Ctx.Find<T>(id);
        }
        public virtual void Insert(T entity)
        {
            Ctx.Add(entity);
            Ctx.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            Ctx.Update(entity);
            Ctx.SaveChanges();
        }
        public virtual void Delete(object id)
        {
            Ctx.Remove(id);
            Ctx.SaveChanges();
        }
    }
}