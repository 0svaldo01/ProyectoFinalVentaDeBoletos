using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class Repository<T> where T : class
    {

        //Nota:
        //Los demas repositorios heredaran los siguentes metodos,
        //pero pueden ser editados para cambiar el funcionamiento de los metodos con un override en el repositorio que se desee,
        //no desde aqui, esto ni se te ocurra moverle >:c
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
        public virtual void Delete(object id)
        {
            context.Remove(id);
            context.SaveChanges();
        }
    }
}