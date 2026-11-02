namespace ClinicaAPI.DTOs;

// Datos que la API devuelve al consultar un paciente.
public class PacienteDto
{
    public int PacienteId { get; set; }
    public string CodigoPaciente { get; set; } = string.Empty;   // PAC-0001
    public string Cedula { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public DateOnly? FechaNacimiento { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaRegistro { get; set; }
}