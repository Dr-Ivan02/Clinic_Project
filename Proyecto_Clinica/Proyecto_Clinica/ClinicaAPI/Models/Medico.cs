using System;
using System.Collections.Generic;

namespace ClinicaAPI.Models;

public partial class Medico
{
    public int MedicoId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string NumeroExequatur { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<ConsultaMedica> ConsultaMedicas { get; set; } = new List<ConsultaMedica>();

    public virtual ICollection<MedicoEspecialidad> MedicoEspecialidads { get; set; } = new List<MedicoEspecialidad>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
