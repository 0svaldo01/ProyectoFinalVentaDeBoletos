using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class SalaAsiento
{
    public int Id { get; set; }

    public int IdSala { get; set; }

    public int IdAsiento { get; set; }

    public virtual Asiento IdAsientoNavigation { get; set; } = null!;
}
