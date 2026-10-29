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

        string pasta = @"C:\XmlNfce";
        XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
        public string conversorDataParaXml(string dataPadraoNacional)
        {
            string[] partesData = dataPadraoNacional.Split('/');

            if (partesData.Length != 2)
            {
                throw new Exception("Formato de data inválido.");
            }

            //fiz dessa forma para não alocar memoria desnecessariamente com mais variaveis
            // partesData[1] é o ano
            //e partesData[0] é o mes
            return $"{partesData[1]}-{partesData[0]}";
        }

        public XElement coletarTagXml(string arquivo, string tag)
        {
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
            XDocument xml = XDocument.Load(arquivo);

            XElement infNFe = xml.Descendants(ns + "infNFe").FirstOrDefault();

            XElement valorTag = infNFe.Descendants(ns + $"{tag}").FirstOrDefault();

            return valorTag;
        }
        public string coletarChaveNfe(string arquivo)
        {
            
            XDocument xml = XDocument.Load(arquivo);

            XElement infNFe = xml.Descendants(ns + "infNFe").FirstOrDefault();
            string chave = (string)infNFe.Attribute("Id");

            return chave;

        }

        public string ProcessarNf(string competenciaFiltrar)
        {
           
            if (!Directory.Exists(pasta))
            {
                throw new Exception($"A pasta não foi encontrada: {pasta}.");
            }

            string competenciaXmlFiltro = conversorDataParaXml(competenciaFiltrar);

            string[] arquivos = Directory.GetFiles(pasta, "*.xml");

            List<NotaParaEnvio> notasValidadas = new List<NotaParaEnvio>();
      
            string cnpjParaArquivo = "";

            foreach (string arquivo in arquivos)
            {
                try
                {
                   
                    string chave = coletarChaveNfe(arquivo);

                    string modelo = chave.Substring(21, 22);
                    if (modelo != "65") continue;


                    string competenciaXml = $"{chave.Substring(4, 2)}/20{chave.Substring(2, 2)}";
                    if (competenciaXml != competenciaXmlFiltro) continue;

                    XElement dest = coletarTagXml(arquivo, "dest");
                     
                    bool temCpf = dest?.Element(ns + "CPF") != null;
                    bool temCnpj = dest?.Element(ns + "CNPJ") != null;
                    if (temCpf || temCnpj) continue;

                    notasValidadas.Add(new NotaParaEnvio
                    {
                        chave = chave,
                        competencia = competenciaXml
                    });

                    // 
                    if (string.IsNullOrEmpty(cnpjParaArquivo))
                    {
                        XElement emit = coletarTagXml(arquivo,"emit");
                        cnpjParaArquivo = (emit?.Element(ns + "CNPJ")?.Value ?? "00000").Substring(0, 5);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao tentar abrir o XML {0}", ex);
                }
            }

            if (notasValidadas.Count > 0)
            {
                var opcoesJson = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(notasValidadas, opcoesJson);

                string nomeArquivoFinal = $"{competenciaXmlFiltro}{cnpjParaArquivo}.txt";
                string caminhoCompletoSaida = Path.Combine(pasta, nomeArquivoFinal);

                File.WriteAllText(caminhoCompletoSaida, jsonString);
            }
            else
            {
                Console.Write("\n[AVISO] Nenhuma nota fiscal atendeu aos filtros estabelecidos.");
            }
            return "sucesso";
        }
    }
}
