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
    }

    public class Leitorxml
    {
        public static string lerxml(string competenciaFiltrar)
        {
            StringBuilder logBuilder = new StringBuilder();
            string pasta = @"C:\XmlNfce";

            if (!Directory.Exists(pasta))
            {
                throw new Exception($"A pasta não foi encontrada: {pasta}.");
            }

            // Converte a Competência do padrão nacional (MM/AAAA) para o formato das tags XML (AAAA-MM)
            string[] partesData = competenciaFiltrar.Split('/');
            if (partesData.Length != 2) throw new Exception("Formato de data inválido.");
            string mes = partesData[0];
            string ano = partesData[1];
            string competenciaXmlFiltro = $"{ano}-{mes}";

            string[] arquivos = Directory.GetFiles(pasta, "*.xml");
            logBuilder.AppendLine("[SUCESSO] Diretório encontrado!");
            logBuilder.AppendLine($"[INFO] Total de ficheiros XML encontrados: {arquivos.Length}");

            List<NotaParaEnvio> notasValidadas = new List<NotaParaEnvio>();
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
            string cnpjParaArquivo = "";

            foreach (string arquivo in arquivos)
            {
                try
                {
                    //  carrega o XML fisicamente sem usar a Chave de Acesso do título
                    XDocument xml = XDocument.Load(arquivo);
                    logBuilder.AppendLine($"[INFO] Lendo arquivo: {Path.GetFileName(arquivo)}");

                    XElement infNFe = xml.Descendants(ns + "infNFe").FirstOrDefault();
                    if (infNFe == null) continue;

                    string modelo = infNFe.Descendants(ns + "mod").FirstOrDefault()?.Value;
                    if (modelo != "65") continue;

                    string dhEmi = infNFe.Descendants(ns + "dhEmi").FirstOrDefault()?.Value;
                    string competenciaXml = string.IsNullOrEmpty(dhEmi) ? "" : dhEmi.Substring(0, 7);
                    if (competenciaXml != competenciaXmlFiltro) continue;

                    XElement dest = infNFe.Descendants(ns + "dest").FirstOrDefault();
                    bool temCpf = dest?.Element(ns + "CPF") != null;
                    bool temCnpj = dest?.Element(ns + "CNPJ") != null;
                    if (temCpf || temCnpj) continue;

                    string chave = infNFe.Attribute("Id")?.Value.Replace("NFe", "");
                    string valorString = infNFe.Descendants(ns + "vNF").FirstOrDefault()?.Value;
                    decimal valorConvertido = valorString != null ? decimal.Parse(valorString, CultureInfo.InvariantCulture) : 0;

                    notasValidadas.Add(new NotaParaEnvio
                    {
                        chave = chave,
                        competencia = competenciaXml
                    });

                    // Captura o CNPJ dentro do XML (tag <emit>) para dar nome ao ficheiro final
                    if (string.IsNullOrEmpty(cnpjParaArquivo))
                    {
                        XElement emit = infNFe.Descendants(ns + "emit").FirstOrDefault();
                        cnpjParaArquivo = emit?.Element(ns + "CNPJ")?.Value ?? "00000";
                    }

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

                // Garante que o CNPJ tem tamanho suficiente para evitar erros
                string xxxxx = cnpjParaArquivo.Length >= 5 ? cnpjParaArquivo.Substring(0, 5) : cnpjParaArquivo.PadRight(5, '0');
                string nomeArquivoFinal = $"{mes}{ano}{xxxxx}.txt";
                string caminhoCompletoSaida = Path.Combine(pasta, nomeArquivoFinal);

                File.WriteAllText(caminhoCompletoSaida, jsonString);
                logBuilder.AppendLine($"\n[SUCESSO] {notasValidadas.Count} notas passaram nos filtros.");
                logBuilder.AppendLine($"[SUCESSO] Arquivo '{nomeArquivoFinal}' gerado com a lista das notas.");
            }
            else
            {
                logBuilder.AppendLine("\n[AVISO] Nenhuma nota fiscal atendeu aos filtros estabelecidos.");
            }

            return logBuilder.ToString();
        }
    }
}
