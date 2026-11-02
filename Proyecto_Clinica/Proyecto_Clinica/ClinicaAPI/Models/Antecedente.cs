using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Antecedente
{
    public int AntecedenteId { get; set; }

    public int ExpedienteId { get; set; }

    public string Tipo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public int? RegistradoPorUsuarioId { get; set; }

    public virtual Expediente Expediente { get; set; } = null!;

    public virtual Usuario? RegistradoPorUsuario { get; set; }
}
