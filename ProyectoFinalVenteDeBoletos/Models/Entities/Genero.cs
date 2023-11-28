using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Genero
{
    public int IdGenero { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Peliculas> Peliculas { get; set; } = new List<Peliculas>();
}
