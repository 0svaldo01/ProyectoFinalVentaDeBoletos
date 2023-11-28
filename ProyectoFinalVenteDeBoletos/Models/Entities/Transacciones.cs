using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Transacciones
{
    public int IdTransacciones { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Total { get; set; }

    public int IdHorario { get; set; }

    public int IdSala { get; set; }

    public int IdTipoUsuarios { get; set; }

    public virtual ICollection<Boletosvendidos> Boletosvendidos { get; set; } = new List<Boletosvendidos>();

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Salas IdSalaNavigation { get; set; } = null!;

    public virtual Tipousuarios IdTipoUsuariosNavigation { get; set; } = null!;
}
