using SistemaOperacional10._0.Nucleo;
using System;

namespace SistemaOperacional10._0
{
    internal class Entrada
    {
        private static void Main(string[] parametros)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool modoTeste = parametros.Length > 0 && parametros[0].Equals("teste", StringComparison.OrdinalIgnoreCase);

            if (modoTeste)
            {
                ProgramTeste.Rodar();
            } else
            {
                var nucleo = new Kernel();
                var interfaceMenu = new MenuPrincipal(nucleo);

                interfaceMenu.Executar();
            }
        }
    }
}
