const URL_API = 'https://localhost:7081/api/v1/Documento';

async function enviarDocumento() {
    const codigoCliente = document.getElementById("codigoCliente").value;
    const inputArquivo = document.getElementById("arquivo");
    const arquivo = inputArquivo.files[0];

    if (!codigoCliente || !arquivo) {
        alert("Informe o codigo do cliente e selecione um arquivo");
        return;
    }

    const dadosArquivo = new FormData();
    dadosArquivo.append("arquivo", arquivo);

    const response = await fetch(`${URL_API}/upload/${codigoCliente}`, {
        method: "POST",
        body: dadosArquivo
    });

    if (response.ok) {
        alert("Documento enviado com sucesso!");
        document.getElementById("codigoCliente").value = "";
        document.getElementById("arquivo").value = "";
    } else {
        alert("Erro. Falha ao enviar o documento");
    }
    // HU01 - Busca e Listagem de Documentos
    async function buscarDocumentos() {
        const clienteId = document.getElementById("buscaClienteId").value;

        if (!clienteId) {
            alert("Informe o código do cliente para buscar!");
            return;
        }

        try {
            const response = await fetch(`${URL_API}/cliente/${clienteId}`);

            if (!response.ok) {
                throw new Error("Erro ao buscar documentos do cliente.");
            }

            const documentos = await response.json();
            renderizarTabela(documentos);
        } catch (error) {
            console.error("Erro na busca:", error);
            alert("Erro ao carregar lista de documentos.");
        }
    }

    function renderizarTabela(documentos) {
        const tabelaBody = document.getElementById("tabelaBody");
        tabelaBody.innerHTML = ""; 

        documentos.forEach(doc => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
            <td>${doc.id}</td>
            <td>${doc.nome}</td>
            <td>${doc.extensao}</td>
            <td>
                <!-- Botões de ações das próximas HUs serão inseridos aqui -->
            </td>
        `;
            tabelaBody.appendChild(tr);
        });
    }
}