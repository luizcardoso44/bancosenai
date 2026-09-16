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
            if (arquivo == null || arquivo.Length == 0)
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

            long limitebytes = 2 * 1024 * 1024;

            string[] extensaopermitida = { ".jpg", ".pfd", ".png" };

            if (arquivo.Length > limitebytes)
            {
                return BadRequest("Arquivo maior que o limite");
            }

            if (!extensaopermitida.Contains(extensao.ToLower()))
            {
                return BadRequest("Extensão não suportada");
            }



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

            return Ok(new { mensagem = "Documento anexado com sucesso", arquivoSalvo = novonome });
        }
        [HttpGet("/api/v1/documento/listar/{codigoCliente}")]
        public IActionResult ListarDocumentos(int codigoCliente)
        {
            var documentos = _documentosMetadados.Where(d => d.CodigoCliente == codigoCliente).ToList();

            if (!documentos.Any())
            {
                return NotFound("Nenhum documento encontrado.");
            }

            return Ok(documentos);
        }
        [HttpGet("/api/v1/documento/download/{id}")]
        public IActionResult DownloadDocumento(int id)
        {
            var documento = _documentosMetadados.FirstOrDefault(d => d.Id == id);

            if (documento == null)
            {
                return NotFound("Documento não encontrado.");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(documento.Caminho);
            string nomeArquivo = $"{documento.Name}{documento.Extensão}";

            return File(fileBytes, "application/octet-stream", nomeArquivo);
        }
        [HttpDelete("/api/v1/documento/excluir/{id}")]
        public IActionResult ExcluirDocumento(int id)
        {
            var documento = _documentosMetadados.FirstOrDefault(d => d.Id == id);

            if (documento == null)
            {
                return NotFound("Documento não encontrado.");
            }

            if (System.IO.File.Exists(documento.Caminho))
            {
                System.IO.File.Delete(documento.Caminho);
            }

            _documentosMetadados.Remove(documento);

            return Ok(new { mensagem = "Documento excluído com sucesso." });
        } 
    }
}

