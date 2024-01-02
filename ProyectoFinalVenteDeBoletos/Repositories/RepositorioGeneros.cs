using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioGeneros: Repository<Genero>
    {
        public RepositorioGeneros(Sistem21VentaboletosdbContext Ctx): base(Ctx)
        {
        }

    }
}
