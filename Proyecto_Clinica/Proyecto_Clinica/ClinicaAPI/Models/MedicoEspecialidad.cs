using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class MedicoEspecialidad
{
    public int MedicoId { get; set; }

    public int EspecialidadId { get; set; }

    public DateOnly FechaAsignacion { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Especialidad Especialidad { get; set; } = null!;

    public virtual Medico Medico { get; set; } = null!;
}
