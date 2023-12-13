using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class PeliculaGenero
{
    public int Id { get; set; }

    public int IdPelicula { get; set; }

    public int IdGenero { get; set; }

    public virtual Genero IdGeneroNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;
}
