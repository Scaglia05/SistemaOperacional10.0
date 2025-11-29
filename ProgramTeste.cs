
using SimuladorSO.Nucleo;

namespace SimuladorSO
{
    internal class ProgramTeste
    {
        public static void Rodar()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();

        var kernel = new Kernel();

            // Mensagem inicial
            Console.WriteLine("SIMULADOR SO - TESTE RÁPIDO\n");

            // Carregar arquivo
            Console.WriteLine("Carregando workload_exemplo.txt...");
            kernel.CarregadorWorkload.CarregarArquivo("workload_exemplo.txt");

            Thread.Sleep(500);

            // Listar processos
            var processos = kernel.GerenciadorProcessos.ListarProcessos();
            Console.WriteLine($"\nProcessos carregados: {processos.Count}\n");

            foreach (var p in processos)
            {
                Console.WriteLine($"Processo {p.PCB.PIDSimbolico} (PID {p.PCB.PID})");
                Console.WriteLine($"  Estado: {p.PCB.Estado}");
                Console.WriteLine($"  Prioridade: {p.PCB.Prioridade}");
                Console.WriteLine($"  CPU: {p.PCB.TempoCPU}");
                Console.WriteLine($"  Chegada: {p.PCB.TempoChegada}");
                Console.WriteLine($"  Início: {p.PCB.TempoInicio}");
                Console.WriteLine($"  Finalização: {p.PCB.TempoFinalizacao}\n");
            }

            // Relógio do sistema
            Console.WriteLine($"Ticks do sistema: {kernel.Clock.TempoAtual}\n");

            Console.WriteLine("Execução finalizada.");
        }
    }

}
