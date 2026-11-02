using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public byte RolId { get; set; }

    public int? MedicoId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string HashContrasena { get; set; } = null!;

    public string? Correo { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Antecedente> Antecedentes { get; set; } = new List<Antecedente>();

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<ConsultaMedica> ConsultaMedicas { get; set; } = new List<ConsultaMedica>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Medico? Medico { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Rol Rol { get; set; } = null!;
}
