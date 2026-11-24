using System.Linq;
using System.Xml.Linq;

namespace SGC.service
{
    public class LeitorXmlNfce
    {
        private readonly XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

        public XDocument abrirXml(string arquivo)
        {
            return XDocument.Load(arquivo);
        }

        public XElement coletarTagXml(XDocument xml, string tag)
        {
            return xml.Descendants(ns + tag).FirstOrDefault();
        }

        public string coletarChaveNfe(XDocument xml)
        {
            XElement infNFe = xml.Descendants(ns + "infNFe").FirstOrDefault();

            if (infNFe == null || infNFe.Attribute("Id") == null)
            {
                return "";
            }

            string id = infNFe.Attribute("Id").Value;
            return id.Replace("NFe", "");
        }
    }
}
