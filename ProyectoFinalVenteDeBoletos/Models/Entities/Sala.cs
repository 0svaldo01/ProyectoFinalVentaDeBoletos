using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Sala
{
    public int Id { get; set; }

    public int Capacidad { get; set; }

    public int IdTipoPantalla { get; set; }

    public virtual ICollection<Horario> Horario { get; set; } = new List<Horario>();

    public virtual Tipopantalla IdTipoPantallaNavigation { get; set; } = null!;
}
