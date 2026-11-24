using System;
using System.Linq;
using SGC.models;

namespace SGC.service
{
    internal class Tratamento
    {
        public string validarCompetencia(string dataPadraoNacional)
        {
            string[] partesData = dataPadraoNacional.Trim().Split('/');

            if (partesData.Length != 2 || partesData[0].Length != 2 || partesData[1].Length != 4)
            {
                throw new Exception("Formato de data inválido. Use MM/AAAA.");
            }

            return $"{partesData[0]}/{partesData[1]}";
        }

        public bool chaveEhValida(string chave)
        {
            return !string.IsNullOrEmpty(chave) && chave.Length == 44 && chave.All(char.IsDigit);
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
