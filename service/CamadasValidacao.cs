using System;
using System.Collections.Generic;
using System.Text;

namespace SGC.service
{
    internal class CamadasValidacao
    {
        /// Primeira camada de validação: Validação de Protocolo de Autorização SEFAZ.
        public void ProtocoloSEFAZ(models.NotaFiscal notaFiscal)
        {
            if (notaFiscal.chaveAcesso <= 0)
            {
                throw new ArgumentException("Chave de acesso inválida.");
            }
            if (notaFiscal.competencia == default(DateTime))
            {
                throw new ArgumentException("Data de competência inválida.");
            }
        }
    }
}
