using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Salas
{
    public int IdSala { get; set; }

    public int? NumeroSala { get; set; }

    public int? Capacidad { get; set; }

    public int? IdTipoPantalla { get; set; }

    public virtual ICollection<Horario> Horario { get; set; } = new List<Horario>();

    public virtual ICollection<Transacciones> Transacciones { get; set; } = new List<Transacciones>();
}
