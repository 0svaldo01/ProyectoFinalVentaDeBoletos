using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Tipousuarios
{
    public int IdTipoUsuarios { get; set; }

    public sbyte? TipoUsuario { get; set; }

    public virtual ICollection<Transacciones> Transacciones { get; set; } = new List<Transacciones>();
}
