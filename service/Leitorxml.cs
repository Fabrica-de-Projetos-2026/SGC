using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace SGC.service
{
    public class NotaParaEnvio
    {
        public string chave { get; set; }
        public string competencia { get; set; }
        public decimal valor { get; set; }
    }

    public class Leitorxml
    {
        public static string lerxml(string serieFiltrar, string competenciaFiltrar)
        {
            StringBuilder logBuilder = new StringBuilder();
            string pasta = @"C:\XmlNfce";
            string arquivoJson = @"C:\XmlNfce\envio.json";

            if (!Directory.Exists(pasta))
            {
                throw new Exception($"A pasta não foi encontrada: {pasta}.");
            }

            string[] arquivos = Directory.GetFiles(pasta, "*.xml");
            logBuilder.AppendLine("[SUCESSO] Diretório encontrado!");
            logBuilder.AppendLine($"[INFO] Total de arquivos XML encontrados: {arquivos.Length}");

            List<NotaParaEnvio> notasValidadas = new List<NotaParaEnvio>();
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

            foreach (string arquivo in arquivos)
            {
                try
                {
                    XDocument xml = XDocument.Load(arquivo);
                    logBuilder.AppendLine($"[INFO] Lendo arquivo: {Path.GetFileName(arquivo)}");

                    XElement infNFe = xml.Descendants(ns + "infNFe").FirstOrDefault();
                    if (infNFe == null) continue;

                    string modelo = infNFe.Descendants(ns + "mod").FirstOrDefault()?.Value;
                    if (modelo != "65") continue;

                    string serieXml = infNFe.Descendants(ns + "serie").FirstOrDefault()?.Value;
                    if (serieXml != serieFiltrar) continue;

                    string dhEmi = infNFe.Descendants(ns + "dhEmi").FirstOrDefault()?.Value;
                    string competenciaXml = string.IsNullOrEmpty(dhEmi) ? "" : dhEmi.Substring(0, 7);
                    if (competenciaXml != competenciaFiltrar) continue;

                    XElement dest = infNFe.Descendants(ns + "dest").FirstOrDefault();
                    bool temCpf = dest?.Element(ns + "CPF") != null;
                    bool temCnpj = dest?.Element(ns + "CNPJ") != null;
                    if (temCpf || temCnpj) continue;

                    string chave = infNFe.Attribute("Id")?.Value.Replace("NFe", "");
                    string valorString = infNFe.Descendants(ns + "vNF").FirstOrDefault()?.Value;
                    decimal valorConvertido = decimal.Parse(valorString, CultureInfo.InvariantCulture);

                    notasValidadas.Add(new NotaParaEnvio
                    {
                        chave = chave,
                        competencia = competenciaXml,
                        valor = valorConvertido
                    });

                    logBuilder.AppendLine($"[APROVADA] Arquivo atende aos requisitos: {Path.GetFileName(arquivo)}");
                }
                catch (Exception ex)
                {
                    logBuilder.AppendLine($"[AVISO] Erro ao analisar {Path.GetFileName(arquivo)}: {ex.Message}");
                }
            }

            if (notasValidadas.Count > 0)
            {
                var opcoesJson = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(notasValidadas, opcoesJson);
                File.WriteAllText(arquivoJson, jsonString);
                logBuilder.AppendLine($"\n[SUCESSO] {notasValidadas.Count} notas passaram nos filtros.");
                logBuilder.AppendLine($"[SUCESSO] Arquivo '{arquivoJson}' gerado com a lista das notas.");
            }
            else
            {
                logBuilder.AppendLine("\n[AVISO] Nenhuma nota fiscal atendeu aos 4 filtros estabelecidos.");
            }

            return logBuilder.ToString();
        }
        }
}
