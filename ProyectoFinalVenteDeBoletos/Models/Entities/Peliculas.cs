using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Peliculas
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public decimal? Duracion { get; set; }

    public string? Sinopsis { get; set; }

    public string? Trailer { get; set; }

    public int IdClasifiacion { get; set; }

    public int IdGenero { get; set; }

    public virtual ICollection<Horario> Horario { get; set; } = new List<Horario>();

    public virtual Clasifiacion IdClasifiacionNavigation { get; set; } = null!;

    public virtual Genero IdGeneroNavigation { get; set; } = null!;
}
