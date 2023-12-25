using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using System.Runtime.CompilerServices;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioBoletos : Repository<Boleto>
    {
        public Sistem21VentaboletosdbContext Context { get; }
        public RepositorioBoletos(Sistem21VentaboletosdbContext Ctx) : base(Ctx)
        {
            Context = Ctx;
        }
        public override IEnumerable<Boleto> GetAll()
        {
            return Context.Boleto
                 //Navegacion a Usuario y rol
                 .Include(x => x.UsuarioBoleto)
                 .ThenInclude(x => x.IdUsuarioNavigation)
                 .ThenInclude(x => x.IdRolNavigation)
                 //Navegacion a horario,pelicula y genero
                 .Include(x => x.IdHorarioNavigation)
                 .ThenInclude(x => x.PeliculaHorario)
                 .ThenInclude(x => x.IdHorarioNavigation)
                 .ThenInclude(x => x.PeliculaHorario)
                 .ThenInclude(x => x.IdPeliculaNavigation)
                 .ThenInclude(x => x.PeliculaGenero)
                 .ThenInclude(x => x.IdGeneroNavigation)
                 //Navegacion a sala y asiento
                 .Include(x => x.IdSalaNavigation).ThenInclude(x => x.SalaAsiento).ThenInclude(x => x.IdAsientoNavigation)
                 .ThenInclude(x => x.SalaAsiento).ThenInclude(x => x.IdAsientoNavigation);
        }
    }
}