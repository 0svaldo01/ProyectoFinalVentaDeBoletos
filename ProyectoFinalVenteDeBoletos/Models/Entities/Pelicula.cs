using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Pelicula
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public TimeOnly Duracion { get; set; }

    public string Sinopsis { get; set; } = null!;

    public string? Trailer { get; set; }

    public int IdClasificacion { get; set; }

    public int Año { get; set; }

    public virtual ICollection<Horario> Horario { get; set; } = new List<Horario>();

    public virtual Clasificacion IdClasificacionNavigation { get; set; } = null!;

    public virtual ICollection<PeliculaGenero> PeliculaGenero { get; set; } = new List<PeliculaGenero>();
}
