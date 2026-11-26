using SGC.models;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SGC.service
{
    internal class Tratamento
    {
        private readonly LeitorXmlNfce leitorXml = new LeitorXmlNfce();
        private readonly XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
        public string validarCompetencia(string dataPadraoNacional)
        {
            string[] partesData = dataPadraoNacional.Trim().Split('/');

            if (partesData.Length != 2 || partesData[0].Length != 2 || partesData[1].Length != 4)
            {
                throw new Exception("Formato de data inválido. Use MM/AAAA.");
            }

            return $"{partesData[0]}/{partesData[1]}";
        }

        public string extrairChave(string arquivo)
        {
            string nome = Path.GetFileNameWithoutExtension(arquivo);
            string chave = nome.Split('-')[0].Replace("NFe", "");
            return chave;
        }

        public bool chaveEhValida(string chave)
        {
            return !string.IsNullOrEmpty(chave) && chave.Length == 44 && chave.All(char.IsDigit);
        }

        public bool validarNF(ChaveNfce chave, XDocument xml, string competenciaFiltro)
        {
            if (chave.Modelo != "65")
            {
                return false;
            }

            if (chave.Competencia != competenciaFiltro)
            {
                return false;
            }

            // validar cStat = 100
            XElement cStat = leitorXml.coletarTagXml(xml, "cStat");
            if (cStat == null || cStat.Value != "100")
            {
                return false;
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
                    return false;
                }
            }

            return true;
        }
        public ChaveNfce montarChaveNfce(string chave)
        {
            return new ChaveNfce
            {
                Chave = chave,
                Uf = chave.Substring(0, 2),
                AnoMes = chave.Substring(2, 4),
                Cnpj = chave.Substring(6, 14),
                Modelo = chave.Substring(20, 2),
                Serie = chave.Substring(22, 3),
                NumeroNf = chave.Substring(25, 9),
                TipoEmissao = chave.Substring(34, 1),
                CodigoNumerico = chave.Substring(35, 8),
                DigitoVerificador = chave.Substring(43, 1),
                Competencia = $"{chave.Substring(4, 2)}/20{chave.Substring(2, 2)}"
            };
        }
    }
}
