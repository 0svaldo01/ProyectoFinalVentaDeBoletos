using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Boletosvendidos
{
    public int IdBoletosVendidos { get; set; }

    public string? NumeroAsientol { get; set; }

    public string? Estado { get; set; }

    public int IdTransacciones { get; set; }

    public virtual Transacciones IdTransaccionesNavigation { get; set; } = null!;
}
