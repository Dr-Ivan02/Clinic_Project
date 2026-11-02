using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Tratamiento
{
    public long TratamientoId { get; set; }

    public long ConsultaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Instrucciones { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public virtual ConsultaMedica Consulta { get; set; } = null!;
}
