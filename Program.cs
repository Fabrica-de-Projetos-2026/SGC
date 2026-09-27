using SGC.service;

namespace SGC
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {

            Leitorxml.lerxml();
            Console.ReadKey();
            // linha para aparecer o forms
            //ApplicationConfiguration.Initialize();
            // Application.Run(new Form1());
        }
    }
}