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

        private readonly LeitorXmlNfce leitorXml = new LeitorXmlNfce();
        private readonly Tratamento tratamento = new Tratamento();
        private readonly GeradorJson geradorJson = new GeradorJson();

        public ProcessadorNfce(string pasta = @"C:\XmlNfce")
        {
            this.pasta = pasta;
        }

        public string ProcessarNf(string competenciaFiltrar)
        {
            if (!Directory.Exists(pasta))
            {
                throw new DirectoryNotFoundException($"A pasta não foi encontrada: {pasta}.");
            }

            string[] arquivos = Directory.GetFiles(pasta, "*.xml");

            string competenciaFiltro = tratamento.validarCompetencia(competenciaFiltrar);

            List<NotaJson> notasValidadas = new List<NotaJson>();
            StringBuilder log = new StringBuilder();

            string cnpjParaArquivo = "";

            foreach (string arquivo in arquivos)
            {
                try
                {
                    string chave = tratamento.extrairChave(arquivo);
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

                    if (xml == null)
                    {
                        xml = leitorXml.abrirXml(arquivo);
                    }

                    if (!tratamento.validarNF(chaveNfce, xml, competenciaFiltro))
                    {
                        continue;
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
                    log.AppendLine($"Arquivo ignorado por erro ao ler XML ({Path.GetFileName(arquivo)}): {ex.Message}");
                }
            }

            log.AppendLine($"{arquivos.Length} XMLs lidos, {notasValidadas.Count} aprovados");

            if (notasValidadas.Count > 0)
            {
                string caminhoGerado = geradorJson.gerarArquivo(notasValidadas, competenciaFiltro, cnpjParaArquivo, pasta);
                log.AppendLine($"Arquivo gerado em: {caminhoGerado}");
            }
            else
            {
                log.AppendLine("Nenhuma nota fiscal atendeu aos filtros");
            }

            return log.ToString();
        }
    }
}
