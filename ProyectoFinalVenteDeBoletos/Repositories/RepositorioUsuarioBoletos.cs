using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioUsuarioBoletos : Repository<UsuarioBoleto>
    {
        public Sistem21VentaboletosdbContext Context { get; }
        public RepositorioUsuarioBoletos(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
            Context = Ctx;
        }

        public IEnumerable<UsuarioBoleto>? GetBoletos(int idUser)
        {
            return Context.UsuarioBoleto

                .Include(x=>x.IdBoletosNavigation)
                    .ThenInclude(x=>x.IdSalaNavigation)
                        .ThenInclude(x=>x.SalaAsiento)
                            .ThenInclude(x=>x.IdAsientoNavigation)
                .Include(x=>x.IdBoletosNavigation)
                    .ThenInclude(x=>x.IdHorarioNavigation)
                        .ThenInclude(x=>x.PeliculaHorario)
                            .ThenInclude(x=>x.IdPeliculaNavigation)
                .Include(x=>x.IdUsuarioNavigation)
                .Where(x => x.IdUsuario == idUser);
        }
    }
}
