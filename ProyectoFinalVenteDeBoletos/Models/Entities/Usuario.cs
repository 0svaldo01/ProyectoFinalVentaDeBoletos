using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int IdBoleto { get; set; }

    public int IdRol { get; set; }

    public virtual Boleto IdBoletoNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
