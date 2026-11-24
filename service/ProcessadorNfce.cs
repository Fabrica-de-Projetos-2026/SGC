using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using SGC.models;

namespace SGC.service
{
    public class ProcessadorNfce
    {
        private readonly string pasta;
        private readonly XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

        private readonly LeitorXmlNfce leitorXml = new LeitorXmlNfce();
        private readonly Tratamento tratamento = new Tratamento();
        private readonly GeradorJson geradorJson = new GeradorJson();

        public ProcessadorNfce(string pasta = @"C:\XmlNfce")
        {
            this.pasta = pasta;
        }

        public string lerNomeArquivo(string arquivo)
        {
            string nome = Path.GetFileNameWithoutExtension(arquivo);
            string chave = nome.Split('-')[0].Replace("NFe", "");
            return chave;
        }

        public string ProcessarNf(string competenciaFiltrar)
        {
            if (!Directory.Exists(pasta))
            {
                throw new DirectoryNotFoundException($"A pasta não foi encontrada: {pasta}.");
            }

            string competenciaFiltro = tratamento.validarCompetencia(competenciaFiltrar);

            string[] arquivos = Directory.GetFiles(pasta, "*.xml");

            List<NotaJson> notasValidadas = new List<NotaJson>();
            StringBuilder log = new StringBuilder();

            string cnpjParaArquivo = "";

            foreach (string arquivo in arquivos)
            {
                try
                {
                    string chave = lerNomeArquivo(arquivo);
                    XDocument xml = null;

                    // Se o nome do arquivo não trouxer a chave de 44 dígitos, busca no atributo Id do XML
                    if (!tratamento.chaveEhValida(chave))
                    {
                        xml = leitorXml.abrirXml(arquivo);
                        chave = leitorXml.coletarChaveNfe(xml);

                        if (!tratamento.chaveEhValida(chave))
                        {
                            log.AppendLine($"Arquivo invalido: {Path.GetFileName(arquivo)}");
                            continue;
                        }
                    }

                    ChaveNfce chaveNfce = tratamento.montarChaveNfce(chave);

                    // Filtro inicial pela chave
                    if (chaveNfce.Modelo != "65")
                    {
                        continue;
                    }

                    if (chaveNfce.Competencia != competenciaFiltro)
                    {
                        continue;
                    }

                    if (xml == null)
                    {
                        xml = leitorXml.abrirXml(arquivo);
                    }

                    // validar cStat = 100
                    XElement cStat = leitorXml.coletarTagXml(xml, "cStat");
                    if (cStat == null || cStat.Value != "100")
                    {
                        continue;
                    }

                    // Rejeitar notas com consumidor identificado CPF ou CNPJ em <dest>
                    XElement dest = leitorXml.coletarTagXml(xml, "dest");
                    if (dest != null)
                    {
                        XElement cpf = dest.Element(ns + "CPF");
                        XElement cnpj = dest.Element(ns + "CNPJ");

                        bool temCpf = cpf != null && !string.IsNullOrWhiteSpace(cpf.Value);
                        bool temCnpj = cnpj != null && !string.IsNullOrWhiteSpace(cnpj.Value);

                        if (temCpf || temCnpj)
                        {
                            continue;
                        }
                    }

                    notasValidadas.Add(new NotaJson
                    {
                        ChaveAcesso = chaveNfce.Chave,
                        Modelo = chaveNfce.Modelo,
                        Competencia = chaveNfce.Competencia
                    });

                    if (string.IsNullOrEmpty(cnpjParaArquivo))
                    {
                        cnpjParaArquivo = chaveNfce.Cnpj.Substring(0, 5);
                    }
                }
                catch (Exception ex)
                {
                    log.AppendLine($"[AVISO] Arquivo ignorado por erro ao ler XML ({Path.GetFileName(arquivo)}): {ex.Message}");
                }
            }

            log.AppendLine($"\n[INFO] {arquivos.Length} XML(s) lido(s), {notasValidadas.Count} aprovado(s).");

            if (notasValidadas.Count > 0)
            {
                string caminhoGerado = geradorJson.gerarArquivo(notasValidadas, competenciaFiltro, cnpjParaArquivo, pasta);
                log.AppendLine($"Arquivo gerado em: {caminhoGerado}");
            }
            else
            {
                log.AppendLine("Nenhuma nota fiscal atendeu aos filtros estabelecidos.");
            }

            return log.ToString();
        }
    }
}
