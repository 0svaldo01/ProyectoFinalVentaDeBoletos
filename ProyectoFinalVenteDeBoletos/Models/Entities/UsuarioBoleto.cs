using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class UsuarioBoleto
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdBoletos { get; set; }

    public virtual Boleto IdBoletosNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
