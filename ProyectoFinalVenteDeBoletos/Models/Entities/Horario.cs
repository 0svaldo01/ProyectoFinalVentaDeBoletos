using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Horario
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public int IdPelicula { get; set; }

    public int IdSala { get; set; }

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;

    public virtual Sala IdSalaNavigation { get; set; } = null!;
}
