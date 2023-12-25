using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Boleto
{
    public int Id { get; set; }

    public int IdSala { get; set; }

    public int IdHorario { get; set; }

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual ICollection<UsuarioBoleto> UsuarioBoleto { get; set; } = new List<UsuarioBoleto>();
}
