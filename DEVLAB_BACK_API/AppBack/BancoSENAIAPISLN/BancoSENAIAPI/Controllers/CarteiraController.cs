using BancoSENAIAPI.Data;
using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CarteiraController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeção de dependência do AppDbContext
        public CarteiraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var carteiras = await _context.Carteira.ToListAsync();

            return Ok(carteiras);
        }

        [HttpGet("{codigo:int}")]
        public async Task<IActionResult> ConsultarPorCodigo(int codigo)
        {
            var carteira = await _context.Carteira
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.NumeroCarteira == codigo);

            if (carteira == null)
                return NotFound(new { message = "Carteira não encontrada." });

            return Ok(carteira);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Carteira novaCarteira)
        {
            if (novaCarteira.ApetiteCarteira < 0)
            {
                return BadRequest(new { message = "O valor do apetite carteira deve ser maior ou igual a zero." });
            }

            bool jaExiste = await _context.Carteira
                .AnyAsync(c => c.NumeroCarteira == novaCarteira.NumeroCarteira);

            if (jaExiste)
                return BadRequest(new { message = "Este número de carteira já existe." });

            await _context.Carteira.AddAsync(novaCarteira);
            await _context.SaveChangesAsync();

            // Retorna HTTP 201 Created apontando para a URL da rota de consulta por código
            return CreatedAtAction(
                nameof(ConsultarPorCodigo),
                new { codigo = novaCarteira.NumeroCarteira },
                novaCarteira
            );
        }

        [HttpPut("{codigo:int}")]
        public async Task<IActionResult> Alterar(int codigo, [FromBody] Carteira carteiraAtualizada)
        {
            if (carteiraAtualizada.ApetiteCarteira < 0)
            {
                return BadRequest(new { message = "O valor do apetite carteira deve ser maior ou igual a zero." });
            }

            var carteiraExistente = await _context.Carteira
                .FirstOrDefaultAsync(c => c.NumeroCarteira == codigo);

            if (carteiraExistente == null)
                return NotFound(new { message = "Carteira não encontrada." });

            // Atualiza apenas os atributos mutáveis
            carteiraExistente.NomeCarteira = carteiraAtualizada.NomeCarteira;
            carteiraExistente.ApetiteCarteira = carteiraAtualizada.ApetiteCarteira;

            await _context.SaveChangesAsync();

            return NoContent(); // HTTP 204 No Content
        }

        [HttpDelete("{codigo:int}")]
        public async Task<IActionResult> Excluir(int codigo)
        {
            var carteira = await _context.Carteira
                .FirstOrDefaultAsync(c => c.NumeroCarteira == codigo);

            if (carteira == null)
                return NotFound(new { message = "Carteira não encontrada." });

            _context.Carteira.Remove(carteira);
            await _context.SaveChangesAsync();

            return NoContent(); // HTTP 204 No Content para remoções bem-sucedidas
        }
    }
}