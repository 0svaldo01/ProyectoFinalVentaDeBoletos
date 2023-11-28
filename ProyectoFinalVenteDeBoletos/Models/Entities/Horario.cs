using System;
using System.Collections.Generic;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class Horario
{
    public int IdHorario { get; set; }

    public DateTime? FechaHora { get; set; }

    public decimal? Precio { get; set; }

    public int IdPelicula { get; set; }

    public int IdSala { get; set; }

    public virtual Peliculas IdPeliculaNavigation { get; set; } = null!;

    public virtual Salas IdSalaNavigation { get; set; } = null!;

    public virtual ICollection<Transacciones> Transacciones { get; set; } = new List<Transacciones>();
}
