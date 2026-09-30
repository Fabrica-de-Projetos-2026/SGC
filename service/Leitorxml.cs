using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SGC.service
{
    public class Leitorxml
    {
        public static void lerxml()
        {
            string pasta = @"C:\Users\Administrator\source\repos\SGC\notas-ficais";

            if (!Directory.Exists(pasta))
            {
                Console.WriteLine($"Pasta não encontrada: {pasta}");
                return;
            }

            string[] arquivos = Directory.GetFiles(pasta, "*.xml");
            Console.WriteLine($"Total de arquivos encontrados: {arquivos.Length}");

            foreach (string arquivo in arquivos)
            {
                XDocument documento = XDocument.Load(arquivo);
                Console.WriteLine($"Arquivo: {Path.GetFileName(arquivo)}");
            }
        }
    }
}