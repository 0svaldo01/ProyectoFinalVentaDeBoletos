using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Tipopantalla
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Sala> Sala { get; set; } = new List<Sala>();
}
