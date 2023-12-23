using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioAsientos:Repository<Asiento>
    {
        public RepositorioAsientos(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
        }
        
    }
}
