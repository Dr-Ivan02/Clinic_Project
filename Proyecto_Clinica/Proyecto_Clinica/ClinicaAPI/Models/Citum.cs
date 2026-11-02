using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Citum
{
    public long CitaId { get; set; }

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public int EspecialidadId { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime FechaHoraFin { get; set; }

    public string Estado { get; set; } = null!;

    public string? Motivo { get; set; }

    public int? CreadaPorUsuarioId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ConsultaMedica? ConsultaMedica { get; set; }

    public virtual Usuario? CreadaPorUsuario { get; set; }

    public virtual MedicoEspecialidad MedicoEspecialidad { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;
}
