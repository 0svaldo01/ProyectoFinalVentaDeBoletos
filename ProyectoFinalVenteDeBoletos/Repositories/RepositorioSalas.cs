﻿using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;
using ProyectoFinalVentaDeBoletos.Models.ViewModels;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioSalas : Repository<Sala>
    {
        private readonly Sistem21VentaboletosdbContext Ctx;
        public RepositorioSalas(Sistem21VentaboletosdbContext ctx) : base(ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Sala> GetAll()
        {
            return Ctx.Sala.Include(x => x.IdTipoPantallaNavigation);
        }
        public Sala? GetSalaByNombrePelicula(string Nombre)
        {
            //busca en la tabla de horario, obtiene la primera 
            var a = Ctx.Horario
                .Include(x=>x.PeliculaHorario)
                .Include(x=>x.IdSalaNavigation)
                .ThenInclude(x=>x.SalaAsiento)
                .ThenInclude(x=>x.IdAsientoNavigation)
                .FirstOrDefault(x=>x.PeliculaHorario.Where(x=>x.IdPeliculaNavigation.Id == x.IdPelicula).First()
                .IdPeliculaNavigation.Nombre == Nombre)?.IdSalaNavigation;
            return a;
        }
        public Sala GetSalaByHorario(int IdHorario)
        {
            return Ctx.Sala.First(x=>x.Horario.FirstOrDefault(x=>x.Id == IdHorario) != null);
        }
        public IEnumerable<Sala> GetSalasOrdenadasByTipoPantalla()
        {
            return GetAll().OrderBy(x => x.IdTipoPantalla);
        }
    }
}
