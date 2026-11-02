using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Factura
{
    public long FacturaId { get; set; }

    public long ConsultaId { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public DateTime FechaEmision { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuestos { get; set; }

    public decimal? Total { get; set; }

    public string Estado { get; set; } = null!;

    public int? CreadaPorUsuarioId { get; set; }

    public virtual ConsultaMedica Consulta { get; set; } = null!;

    public virtual Usuario? CreadaPorUsuario { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
