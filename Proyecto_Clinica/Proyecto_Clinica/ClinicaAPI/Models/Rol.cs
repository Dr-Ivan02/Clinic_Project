using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Rol
{
    public byte RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
