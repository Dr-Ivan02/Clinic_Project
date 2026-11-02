using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Especialidad
{
    public int EspecialidadId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activa { get; set; }

    public virtual ICollection<MedicoEspecialidad> MedicoEspecialidads { get; set; } = new List<MedicoEspecialidad>();
}
