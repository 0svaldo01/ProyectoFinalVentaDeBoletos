using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Sala
{
    public int Id { get; set; }

    public int Columnas { get; set; }

    public int Filas { get; set; }

    public int IdTipoPantalla { get; set; }

    public int IdAsiento { get; set; }

    public virtual ICollection<Boleto> Boleto { get; set; } = new List<Boleto>();

    public virtual ICollection<Horario> Horario { get; set; } = new List<Horario>();

    public virtual Asiento IdAsientoNavigation { get; set; } = null!;

    public virtual Tipopantalla IdTipoPantallaNavigation { get; set; } = null!;
}
