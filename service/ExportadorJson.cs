using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace SGC.service
{
    public class NotaParaEnvio
    {
        public string chave { get; set; } = string.Empty;
        public string serie { get; set; } = string.Empty;
        public string competencia { get; set; } = string.Empty;
    }

    // Fábrica Simples: centraliza a montagem do objeto final de saída
    public static class FabricaNotaParaEnvio
    {
        public static NotaParaEnvio Criar(XDocument xml)
        {
            XElement? infNFe = LeitorElementoXml.ObterElemento(xml, "infNFe");
            string atributoId = infNFe?.Attribute("Id")?.Value ?? string.Empty;
            string chaveLimpa = atributoId.Replace("NFe", string.Empty).Trim();

            return new NotaParaEnvio
            {
                chave = chaveLimpa,
                serie = LeitorElementoXml.ObterValor(xml, "mod") ?? string.Empty,
                competencia = LeitorElementoXml.ExtrairCompetencia(xml)
            };
        }
    }

    // Estratégia: desacoplamos o formato de geração do arquivo de saída
    public interface IExportadorNotas
    {
        void Exportar(List<NotaParaEnvio> notasValidadas, string caminhoDestino);
    }

    public class ExportadorJson : IExportadorNotas
    {
        private static readonly JsonSerializerOptions OpcoesJson = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void Exportar(List<NotaParaEnvio> notasValidadas, string caminhoDestino)
        {
            string conteudoJson = JsonSerializer.Serialize(notasValidadas, OpcoesJson);
            File.WriteAllText(caminhoDestino, conteudoJson, Encoding.UTF8);
        }
    }
}