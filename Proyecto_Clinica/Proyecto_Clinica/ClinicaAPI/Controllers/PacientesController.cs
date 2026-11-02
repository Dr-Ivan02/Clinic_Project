using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaAPI.Data;
using ClinicaAPI.Models;
using ClinicaAPI.DTOs;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacientes()
        {
            var pacientes = await _context.Pacientes
                .Where(p => p.Activo)
                .Select(p => new PacienteDto
                {
                    PacienteId = p.PacienteId,
                    CodigoPaciente = $"PAC-{p.PacienteId:D4}",
                    Cedula = p.Cedula,
                    Nombres = p.Nombres,
                    Apellidos = p.Apellidos,
                    FechaNacimiento = p.FechaNacimiento,
                    Telefono = p.Telefono,
                    Correo = p.Correo,
                    Direccion = p.Direccion,
                    FechaRegistro = p.FechaRegistro
                })
                .ToListAsync();

            return Ok(pacientes);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDto>> GetPaciente(int id)
        {
            var p = await _context.Pacientes.FindAsync(id);
            if (p == null || !p.Activo) return NotFound();

            return Ok(new PacienteDto
            {
                PacienteId = p.PacienteId,
                CodigoPaciente = $"PAC-{p.PacienteId:D4}",
                Cedula = p.Cedula,
                Nombres = p.Nombres,
                Apellidos = p.Apellidos,
                FechaNacimiento = p.FechaNacimiento,
                Telefono = p.Telefono,
                Correo = p.Correo,
                Direccion = p.Direccion,
                FechaRegistro = p.FechaRegistro
            });
        }


        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<PacienteDto>>> BuscarPacientes([FromQuery] string termino)
        {
            var query = _context.Pacientes.Where(p => p.Activo);

            if (!string.IsNullOrWhiteSpace(termino))
            {
                query = query.Where(p => p.Cedula.Contains(termino) || p.Nombres.Contains(termino) || p.Apellidos.Contains(termino));
            }

            var pacientes = await query.Select(p => new PacienteDto
            {
                PacienteId = p.PacienteId,
                CodigoPaciente = $"PAC-{p.PacienteId:D4}",
                Cedula = p.Cedula,
                Nombres = p.Nombres,
                Apellidos = p.Apellidos,
                FechaNacimiento = p.FechaNacimiento,
                Telefono = p.Telefono,
                Correo = p.Correo,
                Direccion = p.Direccion,
                FechaRegistro = p.FechaRegistro
            }).ToListAsync();

            return Ok(pacientes);
        }


        [HttpPost]
        public async Task<ActionResult<PacienteDto>> PostPaciente([FromBody] PacienteGuardarDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { mensaje = string.Join(" ", errores) });
            }

            var paciente = new Paciente
            {
                
                Cedula = dto.Cedula,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                FechaNacimiento = dto.FechaNacimiento,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Direccion = dto.Direccion,
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            _context.Pacientes.Add(paciente);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (_context.Pacientes.Any(e => e.Cedula == dto.Cedula))
                {
                    return BadRequest(new { mensaje = "Ya existe un paciente registrado con esta cédula." });
                }
                throw;
            }

            var pacienteDto = new PacienteDto
            {
                PacienteId = paciente.PacienteId,
                Cedula = paciente.Cedula,
                Nombres = paciente.Nombres,
                Apellidos = paciente.Apellidos,
                FechaNacimiento = paciente.FechaNacimiento,
                Telefono = paciente.Telefono,
                Correo = paciente.Correo,
                Direccion = paciente.Direccion,
                FechaRegistro = paciente.FechaRegistro
            };

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.PacienteId }, pacienteDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, [FromBody] PacienteGuardarDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { mensaje = string.Join(" ", errores) });
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null || !paciente.Activo) return NotFound();

            if (!string.Equals(dto.Cedula, paciente.Cedula, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { mensaje = "El documento no se puede modificar. Si se digitó por error, solicite el cambio al perfil administrativo." });
            }

            paciente.Nombres = dto.Nombres;
            paciente.Apellidos = dto.Apellidos;
            paciente.FechaNacimiento = dto.FechaNacimiento;
            paciente.Telefono = dto.Telefono;
            paciente.Correo = dto.Correo;
            paciente.Direccion = dto.Direccion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            paciente.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.PacienteId == id);
        }
    }
}