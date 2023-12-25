using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioUsuarios:Repository<Usuario>
    {
        public Sistem21VentaboletosdbContext Context { get; }
        public RepositorioUsuarios(Sistem21VentaboletosdbContext Ctx) : base(Ctx)
        {
            Context = Ctx;
        }
        public override IEnumerable<Usuario> GetAll()
        {
            return Context.Usuario
                .Include(x => x.IdRolNavigation)
                .Include(x => x.UsuarioBoleto)
                .ThenInclude(x => x.IdBoletosNavigation)
                .ThenInclude(x => x.IdHorarioNavigation)
                .ThenInclude(x => x.PeliculaHorario)
                .ThenInclude(x => x.IdPeliculaNavigation)
                .ThenInclude(x => x.IdClasificacionNavigation)
                .ThenInclude(x => x.Pelicula)
                .ThenInclude(x => x.PeliculaGenero)
                .ThenInclude(x => x.IdGeneroNavigation);
        }

        public Usuario? GetUsuario(string username, string password)
        {
            return GetAll().FirstOrDefault(x => x.Contraseña == password && x.Username == username);
        }
        public IEnumerable<Boleto> GetBoletosByIdUsuario(int Id)
        {
            /*
             * Navegacion a usuarios,boletos,horario,pelicula,genero
             *                                       pelicula,clasificacion
            */
            return Context.Usuario
                .Include(x => x.IdRolNavigation)
                .Include(x => x.UsuarioBoleto)
                .ThenInclude(x => x.IdBoletosNavigation)
                .ThenInclude(x => x.IdHorarioNavigation)
                .ThenInclude(x => x.PeliculaHorario)
                .ThenInclude(x => x.IdPeliculaNavigation)
                .ThenInclude(x => x.IdClasificacionNavigation)
                .ThenInclude(x => x.Pelicula)
                .ThenInclude(x => x.PeliculaGenero)
                .ThenInclude(x => x.IdGeneroNavigation)
                .FirstOrDefault(u => u.Id == Id)?.UsuarioBoleto
                .Where(ub => ub.IdBoletos == ub.IdBoletosNavigation.Id)
                .Select(b => b.IdBoletosNavigation)
                //Regresar una lista vacia si no encontro nada
                ?? new List<Boleto>();
        }
    }
}
