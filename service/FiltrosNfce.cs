using System.Xml.Linq;

namespace SGC.service
{
    public class ResultadoFiltro
    {
        public bool Aprovada { get; }
        public string MotivoRejeicao { get; }

        private ResultadoFiltro(bool aprovada, string motivoRejeicao)
        {
            Aprovada = aprovada;
            MotivoRejeicao = motivoRejeicao;
        }

        public static ResultadoFiltro CriarAprovada() => new ResultadoFiltro(true, string.Empty);
        public static ResultadoFiltro CriarRejeitada(string motivo) => new ResultadoFiltro(false, motivo);
    }

    public abstract class FiltroNotaBase
    {
        private FiltroNotaBase? _proximoFiltro;

        public FiltroNotaBase DefinirProximo(FiltroNotaBase proximoFiltro)
        {
            _proximoFiltro = proximoFiltro;
            return _proximoFiltro;
        }

        public ResultadoFiltro Validar(XDocument xml, string competenciaFiltrar)
        {
            ResultadoFiltro resultado = AplicarRegra(xml, competenciaFiltrar);

            if (!resultado.Aprovada)
                return resultado;

            if (_proximoFiltro == null)
                return ResultadoFiltro.CriarAprovada();

            return _proximoFiltro.Validar(xml, competenciaFiltrar);
        }

        protected abstract ResultadoFiltro AplicarRegra(XDocument xml, string competenciaFiltrar);
    }

    public class FiltroEstruturaNota : FiltroNotaBase
    {
        protected override ResultadoFiltro AplicarRegra(XDocument xml, string competenciaFiltrar)
        {
            XElement? infNFe = LeitorElementoXml.ObterElemento(xml, "infNFe");

            if (infNFe == null)
                return ResultadoFiltro.CriarRejeitada("Estrutura inválida: elemento <infNFe> não encontrado.");

            return ResultadoFiltro.CriarAprovada();
        }
    }

    public class FiltroModeloNfce : FiltroNotaBase
    {
        private const string ModeloPermitido = "65";

        protected override ResultadoFiltro AplicarRegra(XDocument xml, string competenciaFiltrar)
        {
            string? modelo = LeitorElementoXml.ObterValor(xml, "mod");

            if (modelo != ModeloPermitido)
                return ResultadoFiltro.CriarRejeitada($"Modelo inválido ({modelo ?? "ausente"}). Esperado: {ModeloPermitido}.");

            return ResultadoFiltro.CriarAprovada();
        }
    }

    public class FiltroSituacaoAutorizada : FiltroNotaBase
    {
        private const string CodigoAutorizado = "100";

        protected override ResultadoFiltro AplicarRegra(XDocument xml, string competenciaFiltrar)
        {
            string? cStat = LeitorElementoXml.ObterValor(xml, "cStat");

            if (cStat != CodigoAutorizado)
                return ResultadoFiltro.CriarRejeitada($"Nota não autorizada (cStat = {cStat ?? "ausente"}).");

            return ResultadoFiltro.CriarAprovada();
        }
    }

    public class FiltroCompetencia : FiltroNotaBase
    {
        protected override ResultadoFiltro AplicarRegra(XDocument xml, string competenciaFiltrar)
        {
            string competenciaXml = LeitorElementoXml.ExtrairCompetencia(xml);
            string competenciaEsperada = LeitorElementoXml.NormalizarCompetencia(competenciaFiltrar);

            if (competenciaXml != competenciaEsperada)
                return ResultadoFiltro.CriarRejeitada($"Competência divergente ({competenciaXml}). Esperada: {competenciaEsperada}.");

            return ResultadoFiltro.CriarAprovada();
        }
    }

    public class FiltroConsumidorFinal : FiltroNotaBase
    {
        protected override ResultadoFiltro AplicarRegra(XDocument xml, string competenciaFiltrar)
        {
            XElement? destinatario = LeitorElementoXml.ObterElemento(xml, "dest");
            if (destinatario == null)
                return ResultadoFiltro.CriarAprovada();

            string? cpf = LeitorElementoXml.ObterValor(destinatario, "CPF");
            string? cnpj = LeitorElementoXml.ObterValor(destinatario, "CNPJ");

            bool possuiIdentificacao = !string.IsNullOrWhiteSpace(cpf) || !string.IsNullOrWhiteSpace(cnpj);
            if (possuiIdentificacao)
                return ResultadoFiltro.CriarRejeitada("Consumidor identificado com CPF ou CNPJ.");

            return ResultadoFiltro.CriarAprovada();
        }
    }
}