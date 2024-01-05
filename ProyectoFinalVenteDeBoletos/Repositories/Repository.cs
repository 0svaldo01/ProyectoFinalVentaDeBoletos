using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly Sistem21VentaboletosdbContext context;
        public Repository(Sistem21VentaboletosdbContext ctx)
        {
            context = ctx;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }
        public virtual T? Get(object id)
        {
            return context.Find<T>(id);
        }
        public virtual void Insert(T entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }
        public virtual void Delete(object entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }
    }
}