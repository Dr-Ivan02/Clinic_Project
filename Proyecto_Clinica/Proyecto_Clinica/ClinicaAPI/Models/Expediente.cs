using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Expediente
{
    public int ExpedienteId { get; set; }

    public int PacienteId { get; set; }

    public DateTime FechaApertura { get; set; }

    public string? ObservacionesGenerales { get; set; }

    public virtual ICollection<Antecedente> Antecedentes { get; set; } = new List<Antecedente>();

    public virtual ICollection<ConsultaMedica> ConsultaMedicas { get; set; } = new List<ConsultaMedica>();

    public virtual Paciente Paciente { get; set; } = null!;
}
