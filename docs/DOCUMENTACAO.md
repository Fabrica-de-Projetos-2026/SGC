# SGC — Processador de NFC-e

Fábrica de Projetos · 4º Termo ADS · UNIMAR

## O que faz

Lê os XMLs de notas fiscais em `C:\XmlNfce`, seleciona só as NFC-e (modelo 65) emitidas para
consumidor final **sem CPF/CNPJ** e gera o arquivo `envio.json`.

O usuário informa a **série** (ex.: `1`) e a **competência** no formato `AAAA-MM` (ex.: `2026-01`).

## Arquitetura

C# com .NET 10 e Windows Forms.

| Arquivo | Função |
|---|---|
| `Form1.cs` | Tela: recebe série e competência, chama o leitor e mostra o log. |
| `service/Leitorxml.cs` | Lê os XMLs, aplica os filtros e grava o `envio.json`. |

```
Form1 ──(série, competência)──► Leitorxml ──lê──► C:\XmlNfce\*.xml
  ▲                                 │
  └────────────(log)────────────────┴──grava──► C:\XmlNfce\envio.json
```

## Como funciona

Para cada `.xml` da pasta, o leitor aplica quatro filtros. A nota só é aprovada se passar em todos.

| Filtro | Regra | Tag |
|---|---|---|
| Modelo | Deve ser `65` (NFC-e), não `55` (NF-e) | `<mod>` |
| Série | Igual à informada | `<serie>` |
| Competência | `AAAA-MM` da data de emissão igual ao informado | `<dhEmi>` |
| Consumidor final | Sem `<CPF>` nem `<CNPJ>` | `<dest>` |

Se um XML estiver corrompido, o erro vai para o log como aviso e a leitura continua.

## Saída — `envio.json`

```json
[
  {
    "chave": "35260144470771000118650010000009121060719696",
    "competencia": "2026-01",
    "valor": 75.00
  }
]
```

| Campo | Origem |
|---|---|
| `chave` | Atributo `Id` de `<infNFe>`, sem o prefixo `NFe` |
| `competencia` | `AAAA-MM` de `<dhEmi>` |
| `valor` | `<vNF>` como número decimal |

## Casos de teste

Parâmetros: série `1`, competência `2026-01`.

**XML válido** — `...650010000009121060719696-nfe.xml`
Modelo 65, série 1, emitido em 03/01/2026 e sem `<dest>`.
Passa em todos os filtros e entra no `envio.json` com valor `75.00`.

**XML inválido** — `...650010000009211060719695-nfe.xml`
Modelo 65, série 1, emitido em 05/01/2026, mas com `<CPF>` em `<dest>`.
Descartado porque o comprador está identificado.

Resultado do conjunto de exemplo: 6 lidos, 2 aprovados, 4 descartados.

## Como executar

1. Coloque os XMLs em `C:\XmlNfce`.
2. Abra `SGC.slnx` no Visual Studio e execute (F5).
3. Informe série e competência e clique em processar.
4. Confira o log e o arquivo `C:\XmlNfce\envio.json`.
