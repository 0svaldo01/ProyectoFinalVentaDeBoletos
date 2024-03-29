﻿using System;
using System.Collections.Generic;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Horario
{
    public int Id { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraTerminacion { get; set; }

    public int IdSala { get; set; }

    public virtual ICollection<Boleto> Boleto { get; set; } = new List<Boleto>();

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual ICollection<PeliculaHorario> PeliculaHorario { get; set; } = new List<PeliculaHorario>();
}
