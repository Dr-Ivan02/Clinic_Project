using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.DTOs
{
    public class PacienteGuardarDto
    {
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "La cédula debe contener exactamente 11 dígitos.")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(80, ErrorMessage = "Los nombres no pueden superar los 80 caracteres.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(80, ErrorMessage = "Los apellidos no pueden superar los 80 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [FechaNoFutura]
        public DateOnly? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(254, ErrorMessage = "El correo no puede superar los 254 caracteres.")]
        [CorreoOpcional]
        public string? Correo { get; set; }

        [StringLength(250, ErrorMessage = "La dirección no puede superar los 250 caracteres.")]
        public string? Direccion { get; set; }
    }

    public class CorreoOpcionalAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || (value is string correo && string.IsNullOrWhiteSpace(correo)))
            {
                return true;
            }

            return value is string email && new EmailAddressAttribute().IsValid(email);
        }

        public override string FormatErrorMessage(string name)
        {
            return "El correo electrónico no tiene un formato válido.";
        }
    }

    public class FechaNoFuturaAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is DateOnly fecha)
            {
                return fecha <= DateOnly.FromDateTime(DateTime.Today);
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return "La fecha de nacimiento no puede ser futura.";
        }
    }
}