const URL_API = 'http://localhost:5139/api/v1/Documento'

async function EnviarDocumento() {
    const codigoCliente = document.getElementById("codigoCLiente").value;
    const inputArquivo = document.getElementById("arquivo")
const arquivo = inputArquivo.files[0];

if (!codigoCLiente || !arquivo) {
    alert("Informe o codig do cliente e selecione um arquivo")
}
const dadosArquivo = new FormData();
dadosArquivo.append("arquivo", arquivo);

fetch('${URL_API}/upload/${codigoCliente}`, {
    method: "POST",
    body: dadosArquivo
});

if (response.ok){
    alert("Documento enviado com sucesso!";
        document.getElementById("codigoCliente").value = "";
        document.getElementById("arquivo").value = "";
    
} else {
    cont erro - await response.json();
    alert("Erro: " + (erro.message || "Falha ao enviar o documento"));

}