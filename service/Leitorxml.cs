using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace SGC.service
{
    public class Leitorxml
    {
        public static string lerxml(string competenciaFiltrar)
        {
            ProcessadorNfce processador = new ProcessadorNfce();
            return processador.Processar(competenciaFiltrar);
        }
    }

    public static class LeitorElementoXml
    {
        public static XElement? ObterElemento(XContainer? documento, string nomeElemento)
        {
            return documento?.Descendants()
                             .FirstOrDefault(x => x.Name.LocalName == nomeElemento);
        }

        public static string? ObterValor(XContainer? documento, string nomeElemento)
        {
            return ObterElemento(documento, nomeElemento)?.Value?.Trim();
        }

        public static string ExtrairCompetencia(XDocument xml)
        {
            string? dhEmi = ObterValor(xml, "dhEmi");

            bool dataValida = DateTimeOffset.TryParse(
                dhEmi,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTimeOffset dataEmissao);

            return dataValida ? dataEmissao.ToString("MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
        }

        public static string NormalizarCompetencia(string? competencia)
        {
            if (string.IsNullOrWhiteSpace(competencia))
                return string.Empty;

            string texto = competencia.Trim();
            string[] formatosAceitos = { "MM/yyyy", "yyyy-MM", "MM-yyyy", "yyyy/MM" };

            bool formatoValido = DateTime.TryParseExact(
                texto,
                formatosAceitos,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime data);

            return formatoValido ? data.ToString("MM/yyyy", CultureInfo.InvariantCulture) : texto;
        }
    }
}