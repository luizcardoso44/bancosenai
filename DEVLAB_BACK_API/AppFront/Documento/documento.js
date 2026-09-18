const URL_API = "https://localhost:7081"

async function enviarDocumento() {

    const codigoCliente = document.getElementById("codigoCliente").value;
    const inputArquivo = document.getElementById("arquivo");
    const arquivo = inputArquivo.files[0];

    if (!codigoCliente || !arquivo) {
        alert("Informe o código do cliente e selecione o arquivo");
        return;
    }

    const dadosArquivo = new FormData();
    dadosArquivo.append("arquivo", arquivo);

    const response = await fetch(${ URL_API } / upload / ${ codigoCliente }, {
        method: "POST",
        body: dadosArquivo
    });

    if (response.ok) {
        alert("Documento enviado com sucesso !");
        document.getElementById("codigoCliente").value = "";
        document.getElementById("arquivo").value = "";
    } else {

        alert("Erro: " + ("Falha ao enviar o documento"));
    }

}

async function buscarDocumentos() {
    const codigoCliente = document.getElementById("codigoClienteBusca").value;

    if (!codigoCliente) {
        alert("Informe o código do cliente : ");
        return;
    }
    try {
        const response = await fetch(${ URL_API } / listar / ${ codigoCliente });

        if (response.ok) {
            const documentos = await response.json();
            renderizarTabela(documentos);
        } else {
            alert("Erro ao buscar documentos do cliente.");
        }
    } catch (error) {
        console.error("Erro ao buscar documentos:", error);
        alert("Erro ao conectar com o servidor.");
    }

    function renderizarTabela(documentos) {
        const corpoTabela = document.getElementById("corpoTabela");
        corpoTabela.innerHTML = "";

        if (!documentos || documentos.length === 0) {
            corpoTabela.innerHTML = <tr><td colspan="4" style="text-align:center;">Nenhum documento encontrado.</td></tr>;
            return;
        }

        documentos.forEach(doc => {
            const tr = document.createElement("tr");

            tr.innerHTML = `
            <td>${doc.id}</td>
            <td>${doc.nome}</td>
            <td>${doc.extensao}</td>
            <td>
                <div class="acoes-cell">
                    <button class="btn-amarelo" onclick="baixarDocumento(${doc.id})">Baixar</button>
                    <button class="btn-vermelho" onclick="excluirDocumento(${doc.id})">Excluir</button>
                </div>
            </td>
        `;

            corpoTabela.appendChild(tr);
        });
    }


}

async function baixarDocumento(id) {
    try {
        window.open(${ URL_API } / documento / download / ${ id }, "_blank");
    } catch (error) {
        alert("Erro ao baixar o documento.");
    }
}

async function excluirDocumento(id) {
    if (!confirm("Tem certeza que deseja excluir este documento?")) {
        return;
    }

    try {
        const response = await fetch(${ URL_API } / excluir / ${ id }, {
            method: "DELETE"
        });

        if (response.ok) {
            alert("Documento excluído com sucesso!");
            buscarDocumentos();
        } else {
            alert("Erro ao excluir o documento.");
        }
    } catch (error) {
        console.error("Erro ao excluir:", error);
        alert("Erro ao conectar com o servidor.");
    }
}