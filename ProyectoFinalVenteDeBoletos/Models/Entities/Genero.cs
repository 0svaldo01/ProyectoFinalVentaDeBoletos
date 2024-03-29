﻿using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Genero
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<PeliculaGenero> PeliculaGenero { get; set; } = new List<PeliculaGenero>();
}
