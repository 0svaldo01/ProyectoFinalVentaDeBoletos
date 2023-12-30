using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculaHorario:Repository<PeliculaHorario>
    {
        public Sistem21VentaboletosdbContext Context { get; }
        public RepositorioPeliculaHorario(Sistem21VentaboletosdbContext Ctx):base(Ctx)
        {
            Context = Ctx;
        }
        public PeliculaHorario? GetAnterior(int IdPelicula,int IdHorario)
        {
            return Context.PeliculaHorario.FirstOrDefault(x => x.IdPelicula == IdPelicula && x.IdHorario == IdHorario);
        }
        public PeliculaHorario? GetById(int id)
        {
            return Context.PeliculaHorario.Find(id);
        }
    }
}
