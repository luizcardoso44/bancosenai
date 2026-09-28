using BancoSENAIAPI.Data;
using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var clientes = await _context.Cliente
                .AsNoTracking()
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{codigo:int}")]
        public async Task<IActionResult> ConsultarPorCodigo(int codigo)
        {
            var cliente = await _context.Cliente
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CodigoCliente == codigo);

            if (cliente == null)
                return NotFound(new { message = "Cliente não encontrado." });

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Cliente novoCliente)
        {
            if (string.IsNullOrWhiteSpace(novoCliente.NomeCliente))
            {
                return BadRequest(new { message = "O nome do cliente é obrigatório." });
            }

            if (string.IsNullOrWhiteSpace(novoCliente.Cpf))
            {
                return BadRequest(new { message = "O CPF é obrigatório." });
            }

            bool cpfExiste = await _context.Cliente
                .AnyAsync(c => c.Cpf == novoCliente.Cpf);

            if (cpfExiste)
            {
                return BadRequest(new { message = "Este CPF já está cadastrado." });
            }

            if (novoCliente.NumeroAgencia <= 0)
            {
                novoCliente.NumeroAgencia = 10;
            }

            await _context.Cliente.AddAsync(novoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ConsultarPorCodigo),
                new { codigo = novoCliente.CodigoCliente },
                novoCliente
            );
        }

        [HttpPut("{codigo:int}")]
        public async Task<IActionResult> Alterar(int codigo, [FromBody] Cliente clienteAtualizado)
        {
            if (string.IsNullOrWhiteSpace(clienteAtualizado.NomeCliente))
            {
                return BadRequest(new { message = "O nome do cliente é obrigatório." });
            }

            if (string.IsNullOrWhiteSpace(clienteAtualizado.Cpf))
            {
                return BadRequest(new { message = "O CPF é obrigatório." });
            }

            var clienteExistente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.CodigoCliente == codigo);

            if (clienteExistente == null)
                return NotFound(new { message = "Cliente não encontrado." });

            clienteExistente.NomeCliente = clienteAtualizado.NomeCliente;
            clienteExistente.Cpf = clienteAtualizado.Cpf;
            clienteExistente.NumeroAgencia = clienteAtualizado.NumeroAgencia <= 0 ? 10 : clienteAtualizado.NumeroAgencia;
            clienteExistente.SaldoTotal = clienteAtualizado.SaldoTotal;
            clienteExistente.Sexo = clienteAtualizado.Sexo;
            clienteExistente.Endereço = clienteAtualizado.Endereço;
            clienteExistente.cidade = clienteAtualizado.cidade;
            clienteExistente.estado = clienteAtualizado.estado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{codigo:int}")]
        public async Task<IActionResult> Excluir(int codigo)
        {
            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.CodigoCliente == codigo);

            if (cliente == null)
                return NotFound(new { message = "Cliente não encontrado." });

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}