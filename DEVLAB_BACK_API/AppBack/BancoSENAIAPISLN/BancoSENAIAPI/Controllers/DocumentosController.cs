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
        }
    }
}
