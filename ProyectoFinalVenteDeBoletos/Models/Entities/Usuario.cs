using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<UsuarioBoleto> UsuarioBoleto { get; set; } = new List<UsuarioBoleto>();
}
