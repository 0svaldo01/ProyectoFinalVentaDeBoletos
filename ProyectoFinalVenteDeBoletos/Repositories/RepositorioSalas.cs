using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioSalas
    {
        private readonly Repository<Sala> Repositorio;

        public RepositorioSalas(Repository<Sala> repository)
        {
            Repositorio = repository;
        }

        public IEnumerable<Sala> GetAllSalas()
        {
            return Repositorio.GetAll();
        }

        public Sala? GetSalaById(int id)
        {
            return Repositorio.Get(id);
        }
    }
}
