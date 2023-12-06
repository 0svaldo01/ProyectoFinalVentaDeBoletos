using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVenteDeBoletos.Repositories;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioHorario
    {
        private readonly Repository<Horario> Repositorio;

        public RepositorioHorario(Repository<Horario> repository)
        {
            Repositorio = repository;
        }
        public IEnumerable<Horario> GetHorarios()
        {
            return Repositorio.GetAll().OrderBy(x=>x.FechaHora);
        }

        public Horario? GetHorarioById(int id)
        {
            return Repositorio.Get(id);
        }


    }
}
