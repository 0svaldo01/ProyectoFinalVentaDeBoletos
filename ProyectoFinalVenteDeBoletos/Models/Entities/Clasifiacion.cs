using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Clasifiacion
{
    public int IdClasifiacion { get; set; }

    public string? Edad { get; set; }

    public virtual ICollection<Peliculas> Peliculas { get; set; } = new List<Peliculas>();
}
