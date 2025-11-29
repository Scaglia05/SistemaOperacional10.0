using SimuladorSO.Nucleo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorSO.Interface
{
    public abstract class MenuBase
    {
        protected Kernel Kernel { get; }

        protected MenuBase(Kernel kernel)
        {
            Kernel = kernel;
        }

        protected string? LerEntrada(string mensagem)
        {
            Console.Write(mensagem);
            return Console.ReadLine();
        }

        protected int? LerInteiro(string mensagem)
        {
            if (int.TryParse(LerEntrada(mensagem), out int valor))
                return valor;
            return null;
        }

        protected void Aviso(string mensagem) => Console.WriteLine(mensagem);
    }
}
