using System.Text.Json.Serialization;

namespace SGC.models
{
    internal class NotaFiscal
    {
        public string ChaveAcesso { get; set; }

        public DateTime Competencia { get; set; }
    }
}