using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Diagnostico
{
    public long DiagnosticoId { get; set; }

    public long ConsultaId { get; set; }

    public string? Codigo { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public virtual ConsultaMedica Consulta { get; set; } = null!;
}
