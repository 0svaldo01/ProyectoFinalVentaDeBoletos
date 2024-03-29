﻿using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentaDeBoletos.Models.Entities;

namespace ProyectoFinalVentaDeBoletos.Repositories
{
    public class RepositorioClasificaciones: Repository<Clasificacion>
    {
        private readonly Sistem21VentaboletosdbContext Context;
        public RepositorioClasificaciones(Sistem21VentaboletosdbContext context) : base(context)
        {
            this.Context = context;
        }
        public override IEnumerable<Clasificacion> GetAll()
        {
            return Context.Clasificacion.Include(x => x.Pelicula).OrderBy(x => x.Nombre);
        }
        public IEnumerable<Clasificacion> GetClasificaciones()
        {
            return GetAll().OrderBy(x => x.Nombre);
        }
        public Clasificacion? GetClasificacionByNombre(string nombre)
        {
            return Context.Clasificacion.Find(nombre);
        }
        public Clasificacion? GetClasificacionById(int id)
        {
            return Context.Clasificacion.Find(id);
        }
    }
}