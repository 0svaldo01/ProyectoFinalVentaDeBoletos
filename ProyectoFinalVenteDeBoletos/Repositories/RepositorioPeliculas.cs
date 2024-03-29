﻿using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioPeliculas : Repository<Pelicula>
    {
        private readonly Sistem21VentaboletosdbContext Ctx;
        public RepositorioPeliculas(Sistem21VentaboletosdbContext ctx) : base(ctx)
        {
            Ctx = ctx;
        }
        public override IEnumerable<Pelicula> GetAll()
        {
            return Ctx.Pelicula
                .Include(x => x.PeliculaHorario)
                    .ThenInclude(x => x.IdHorarioNavigation)
                        .ThenInclude(x => x.IdSalaNavigation)
                            .ThenInclude(x => x.SalaAsiento)
                                .ThenInclude(x => x.IdAsientoNavigation)
                .Include(x => x.IdClasificacionNavigation)
                .Include(x => x.PeliculaGenero)
                    .ThenInclude(x => x.IdGeneroNavigation);        
        }
        public IEnumerable<Pelicula> GetAllOrderByClasificacion()
        {
            return GetAll().OrderBy(x=> x.IdClasificacionNavigation.Nombre);
        }
        public IEnumerable<Pelicula> GetPeliculasByGenero(string genero)
        {
            return GetAll().Where(x => x.PeliculaGenero.Any(g => g.IdGeneroNavigation.Nombre == genero));
        }
        public IEnumerable<Pelicula> GetPeliculasByClasificacion(string clasificacion)
        {
            return GetAll().Where(x => x.IdClasificacionNavigation.Nombre == clasificacion);
        }
        public Pelicula? GetPeliculaById(int id)
        {
            return Ctx.Pelicula.FirstOrDefault(x=>x.Id == id);
        }
        public Pelicula? GetPeliculaByNombre(string nombre)
        {
            return GetAll().FirstOrDefault(x => x.Nombre == nombre);
        }
    }
}