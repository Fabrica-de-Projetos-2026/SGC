using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SGC.service
{
    public class ProcessadorNfce
    {
        private const string PastaPadrao = @"C:\XmlNfce";
        private const string ArquivoJsonPadrao = @"C:\XmlNfce\envio.json";

        private readonly IExportadorNotas _exportador;

        public ProcessadorNfce()
            : this(new ExportadorJson())
        {
        }

        public ProcessadorNfce(IExportadorNotas exportador)
        {
            _exportador = exportador;
        }

        public string Processar(string competenciaFiltrar)
        {
            string pasta = PastaPadrao;
            string arquivoJson = ArquivoJsonPadrao;

            ValidarDiretorio(pasta);

            StringBuilder logBuilder = new StringBuilder();
            string[] arquivos = Directory.GetFiles(pasta, "*.xml");

            logBuilder.AppendLine("[SUCESSO] Diretório encontrado!");
            logBuilder.AppendLine($"[INFO] Total de arquivos XML encontrados: {arquivos.Length}");

            FiltroNotaBase cadeiaDeFiltros = MontarCadeiaDeFiltros();
            List<NotaParaEnvio> notasValidadas = new List<NotaParaEnvio>();

            foreach (string arquivo in arquivos)
            {
                ProcessarArquivo(arquivo, competenciaFiltrar, cadeiaDeFiltros, notasValidadas, logBuilder);
            }

            SalvarResultado(notasValidadas, arquivoJson, logBuilder);

            return logBuilder.ToString();
        }

        private void ValidarDiretorio(string pasta)
        {
            if (!Directory.Exists(pasta))
            {
                throw new DirectoryNotFoundException($"A pasta não foi encontrada: {pasta}.");
            }
        }

        private FiltroNotaBase MontarCadeiaDeFiltros()
        {
            FiltroNotaBase primeiroFiltro = new FiltroEstruturaNota();

            primeiroFiltro
                .DefinirProximo(new FiltroModeloNfce())
                .DefinirProximo(new FiltroSituacaoAutorizada())
                .DefinirProximo(new FiltroCompetencia())
                .DefinirProximo(new FiltroConsumidorFinal());

            return primeiroFiltro;
        }

        private void ProcessarArquivo(
            string arquivo,
            string competenciaFiltrar,
            FiltroNotaBase cadeiaDeFiltros,
            List<NotaParaEnvio> notasValidadas,
            StringBuilder logBuilder)
        {
            string nomeArquivo = Path.GetFileName(arquivo);

            try
            {
                XDocument xml = XDocument.Load(arquivo);
                logBuilder.AppendLine($"[INFO] Lendo arquivo: {nomeArquivo}");

                ResultadoFiltro resultado = cadeiaDeFiltros.Validar(xml, competenciaFiltrar);

                if (!resultado.Aprovada)
                {
                    logBuilder.AppendLine($"[DESCARTADA] {nomeArquivo} - Motivo: {resultado.MotivoRejeicao}");
                    return;
                }

                NotaParaEnvio nota = FabricaNotaParaEnvio.Criar(xml);
                notasValidadas.Add(nota);

                logBuilder.AppendLine($"[APROVADA] Arquivo atende aos requisitos: {nomeArquivo}");
            }
            catch (Exception ex)
            {
                logBuilder.AppendLine($"[AVISO] Erro ao analisar {nomeArquivo}: {ex.Message}");
            }
        }

        private void SalvarResultado(
            List<NotaParaEnvio> notasValidadas,
            string arquivoJson,
            StringBuilder logBuilder)
        {
            if (notasValidadas.Count == 0)
            {
                logBuilder.AppendLine("\n[AVISO] Nenhuma nota fiscal atendeu aos filtros estabelecidos.");
                return;
            }

            _exportador.Exportar(notasValidadas, arquivoJson);
            logBuilder.AppendLine($"\n[SUCESSO] {notasValidadas.Count} notas passaram nos filtros.");
            logBuilder.AppendLine($"[SUCESSO] Arquivo '{arquivoJson}' gerado com a lista das notas.");
        }
    }
}