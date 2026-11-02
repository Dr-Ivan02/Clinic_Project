using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Pago
{
    public long PagoId { get; set; }

    public long FacturaId { get; set; }

    public DateTime FechaPago { get; set; }

    public decimal Monto { get; set; }

    public string MetodoPago { get; set; } = null!;

    public string? Referencia { get; set; }

    public int? RegistradoPorUsuarioId { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual Usuario? RegistradoPorUsuario { get; set; }
}
