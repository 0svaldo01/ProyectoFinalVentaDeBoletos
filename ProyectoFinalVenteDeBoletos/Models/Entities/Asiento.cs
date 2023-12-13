using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Asiento
{
    public int Id { get; set; }

    public int Columna { get; set; }

    public int Fila { get; set; }

    public bool Ocupado { get; set; }

    public bool? Seleccionado { get; set; }

    public virtual ICollection<Boleto> Boleto { get; set; } = new List<Boleto>();
}
