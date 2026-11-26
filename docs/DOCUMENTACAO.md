# SGC — Processador de NFC-e

Fábrica de Projetos · 4º Termo ADS · UNIMAR · Empresa Parceira: Scaquete Sistemas

## O que faz

Lê os arquivos `.xml` de notas fiscais em `C:\XmlNfce`, filtra de forma otimizada apenas as **NFC-e (modelo 65)** da competência informada, **autorizadas pela SEFAZ (`cStat = 100`)** e emitidas para consumidor final **sem CPF/CNPJ**, gerando o arquivo JSON sequencial com nomenclatura padronizada `mmaaaaxxxxx.json`.

O usuário informa a **competência** no padrão nacional `MM/AAAA` (ex.: `01/2026`).

## Arquitetura

C# com .NET 10 e Windows Forms, dividido em camadas (`UI`, `models` e `service`).

| Arquivo | Camada | Função |
|---|---|---|
| `Form1.cs` | UI (Front-end) | Recebe e valida a competência (`MM/AAAA`), aciona o `ProcessadorNfce` e exibe o log em tela. |
| `models/ChaveNfce.cs` | Model | Mapeia os 44 dígitos da chave de acesso em seus blocos fiscais (`Uf`, `AnoMes`, `Cnpj`, `Modelo`, `Serie`, `NumeroNf`, `TipoEmissao`, `CodigoNumerico`, `DigitoVerificador`, `Competencia`). |
| `models/NotaJson.cs` | Model | Estrutura de saída serializada no JSON final (`ChaveAcesso`, `Modelo`, `Competencia`). |
| `service/ProcessadorNfce.cs` | Service | Orquestra validação de diretório, pré-filtro pela chave, leitura condicional do XML e geração do arquivo. |
| `service/Tratamento.cs` | Service | Valida a competência (`MM/AAAA`), valida os 44 dígitos da chave e decompõe a chave em `ChaveNfce`. |
| `service/LeitorXmlNfce.cs` | Service | Abre o XML sob demanda e localiza tags no namespace NFe (`cStat`, `dest`, `infNFe`). |
| `service/GeradorJson.cs` | Service | Serializa a lista validada em JSON indentado e salva `mmaaaaxxxxx.json`. |

## Pipeline de Filtros e Performance

1. **Validação de Diretório:** verifica se `C:\XmlNfce` existe antes de iniciar (`DirectoryNotFoundException`).
2. **Pré-filtro pela Chave no Nome do Arquivo (0 leituras de disco nas descartadas):**
   - Extrai a chave de 44 dígitos do nome do arquivo (com fallback automático para o atributo `Id` de `<infNFe>` caso o arquivo esteja renomeado).
   - Rejeita imediatamente notas com `Modelo != "65"` (ex.: descarta NF-e `55`).
   - Rejeita imediatamente notas com `Competencia != MM/AAAA`.
3. **Leitura Condicional do XML (1 única abertura em memória por candidata):**
   - **Autorização SEFAZ:** localiza `<cStat>` e exige valor `"100"`.
   - **Consumidor não identificado:** localiza `<dest>` e descarta se existir `<CPF>` ou `<CNPJ>` preenchido.

Arquivos corrompidos ou inválidos são registrados como aviso no log sem interromper o processamento do lote.

## Saída — `mmaaaaxxxxx.json`

Exemplo para competência `01/2026` e emitente `44470771000118` → arquivo `01202644470.json`:

```json
[
  {
    "ChaveAcesso": "35260144470771000118650010000009121060719696",
    "Modelo": "65",
    "Competencia": "01/2026"
  },
  {
    "ChaveAcesso": "35260144470771000118650010000009371060719695",
    "Modelo": "65",
    "Competencia": "01/2026"
  }
]
```
