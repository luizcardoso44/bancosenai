using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentosController : Controller
    {
        private readonly string _caminhoraiz = Path.Combine(Directory.GetCurrentDirectory(), "ClienteArquivos");

        private static List<Models.DocumentoMetadados> _documentosMetadados = new List<Models.DocumentoMetadados>();

        private static int _nextId = 1;

        [HttpPost("Upload/{codigoCliente}")]
        public async Task<IActionResult> AnexarArquivo(int codigoCliente, IFormFile arquivo)
        {
            if(arquivo == null|| arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado. ");
            }
            string pastaCliente = Path.Combine(_caminhoraiz, codigoCliente.ToString());

            if (!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }
            string extensao = Path.GetExtension(arquivo.FileName);
            string nameOriginal = Path.GetFileNameWithoutExtension(arquivo.FileName);
            string novonome = $"{codigoCliente}_{nameOriginal}_{Guid.NewGuid()}{extensao}";
            string caminhoFinal = Path.Combine(pastaCliente, novonome);

            using (var stream = new FileStream(caminhoFinal, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }
            var DocumentosMetadados = new Models.DocumentoMetadados
            {
                Id = _nextId++,
                Name = nameOriginal,
                Extensão = extensao,
                Caminho = caminhoFinal,
                CodigoCliente = codigoCliente
            };
            _documentosMetadados.Add(DocumentosMetadados);

            return Ok(new {mensagem = "Documento anexado com sucesso", arquivoSalvo = novonome });
        }
    }
}
