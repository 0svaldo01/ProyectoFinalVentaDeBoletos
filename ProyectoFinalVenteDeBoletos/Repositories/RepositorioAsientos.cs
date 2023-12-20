using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioAsientos:Repository<Asiento>
    {
        private readonly Sistem21VentaboletosdbContext Context;
        public RepositorioAsientos(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
            this.Context = Ctx;
        }
        public IEnumerable<Asiento> GetAsientos()
        {
            var Asientos = Context.Asiento.Include(x=>x.SalaAsiento).ThenInclude(s=>s.Sala);
            return Asientos;
        }
        public IEnumerable<AsientoModel> GetAsientosByIdSala(int id)
        {
            var a = GetAsientos().Select(x=> new AsientoModel()
            {
                Columna= x.Columna,
                Fila = x.Fila,
                Id = x.Id,
                Ocupado = x.Ocupado,
                Seleccionado = x.Seleccionado
            });
            return a;
        }
    }
}
