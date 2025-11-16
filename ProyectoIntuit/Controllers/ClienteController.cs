using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoIntuit.Context;
using ProyectoIntuit.Models;

namespace ProyectoIntuit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(AppDbContext context, ILogger<ClienteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cliente/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            _logger.LogInformation("Ejecutando GetAll clientes");

            try
            {
                var clientes = await _context.Cliente.ToListAsync();
                _logger.LogInformation("Se obtuvieron {count} clientes", clientes.Count);

                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo todos los clientes");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET SP: api/Cliente/GetAll_SP
        [HttpGet("GetAll_SP")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllDesdeStored()
        {
            _logger.LogInformation("Ejecutando SP GetAllClientes");

            try
            {
                var result = await _context.GetAllClientes_SP();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando el SP GetAllClientes");
                return StatusCode(500, "Error interno al ejecutar el SP");
            }
        }

        // GET: api/Cliente/Search?nombre=facu
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Cliente>>> Search([FromQuery] string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    // si no hay nombre, devolver todo o devolver BadRequest según preferencia
                    var todos = await _context.Cliente.ToListAsync();
                    return Ok(todos);
                }

                var nombreNormalized = nombre.Trim().ToLower();

                var clientes = await _context.Cliente
                    .Where(c => EF.Functions.Like(c.Nombre.ToLower(), $"%{nombreNormalized}%"))
                    .ToListAsync();

                if (clientes == null || clientes.Count == 0)
                    return NotFound();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando cliente por nombre");
                return StatusCode(500, "Error interno");
            }
        }


        // GET: api/Cliente/Get/5
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            _logger.LogInformation("Obteniendo cliente con id {id}", id);

            try
            {
                var cliente = await _context.Cliente.FindAsync(id);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con id {id} no encontrado", id);
                    return NotFound();
                }

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente por ID");
                return StatusCode(500, "Error interno");
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest("El ID no coincide");

            var existing = await _context.Cliente.FindAsync(id);

            if (existing == null)
                return NotFound();

            // SOLO actualizamos propiedades necesarias
            existing.Nombre = cliente.Nombre;
            existing.Apellido = cliente.Apellido;
            existing.Email = cliente.Email;
            existing.RazonSocial = cliente.RazonSocial;
            existing.FechaNacimiento = cliente.FechaNacimiento;
            existing.FechaModificacion = DateTime.Now;
            // agrega acá las demás propiedades que quieras actualizar

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando cliente");
                return StatusCode(500, "Error interno del servidor");
            }
        }


        // POST Insert
        [HttpPost("Insert")]
        public async Task<ActionResult<Cliente>> Insert(Cliente cliente)
        {
            _logger.LogInformation("Insertando nuevo cliente");

            try
            {
                cliente.FechaCreacion = DateTime.Now;
                cliente.FechaModificacion = DateTime.Now;
                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error insertando cliente");
                return StatusCode(500, "Error interno al insertar");
            }
        }

        // DELETE
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Eliminando cliente con ID {id}", id);

            try
            {
                var cliente = await _context.Cliente.FindAsync(id);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente no encontrado para eliminar {id}", id);
                    return NotFound();
                }

                _context.Cliente.Remove(cliente);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando cliente");
                return StatusCode(500, "Error interno al eliminar");
            }
        }
        
        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Id == id);
        }
    }
}
