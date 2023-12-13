using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Boleto
{
    public int Id { get; set; }

    public int IdAsiento { get; set; }

    public int IdHorario { get; set; }

    public virtual Asiento IdAsientoNavigation { get; set; } = null!;

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
