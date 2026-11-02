using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class ConsultaMedica
{
    public long ConsultaId { get; set; }

    public int ExpedienteId { get; set; }

    public long? CitaId { get; set; }

    public int MedicoId { get; set; }

    public DateTime FechaHora { get; set; }

    public string? MotivoConsulta { get; set; }

    public string? Sintomas { get; set; }

    public string NotasClinicas { get; set; } = null!;

    public string? PlanSeguimiento { get; set; }

    public int? RegistradaPorUsuarioId { get; set; }

    public virtual Citum? Cita { get; set; }

    public virtual ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();

    public virtual Expediente Expediente { get; set; } = null!;

    public virtual Factura? Factura { get; set; }

    public virtual Medico Medico { get; set; } = null!;

    public virtual Usuario? RegistradaPorUsuario { get; set; }

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
}
