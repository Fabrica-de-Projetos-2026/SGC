using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SGC.models;

namespace SGC.service
{
    public class GeradorJson
    {
        internal string gerarArquivo(List<NotaJson> notasValidadas, string competenciaPadraoNacional, string cnpjParaArquivo, string pasta)
        {
            var opcoesJson = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(notasValidadas, opcoesJson);

            //  mmaaaaxxxxx.json
            string nomeArquivoFinal = $"{competenciaPadraoNacional.Replace("/", "")}{cnpjParaArquivo}.json";
            string caminhoCompletoSaida = Path.Combine(pasta, nomeArquivoFinal);

            File.WriteAllText(caminhoCompletoSaida, jsonString);

            return caminhoCompletoSaida;
        }
    }
}
