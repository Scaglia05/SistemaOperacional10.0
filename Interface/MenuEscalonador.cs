using SimuladorSO.Nucleo;
using System.Threading;

public class MenuEscalonador
{
    private Kernel _kernel;
    public MenuEscalonador(Kernel kernel) => _kernel = kernel;

    public void Executar()
    {
        bool ativo = true;
        while (ativo)
        {
            Console.Clear();
            ExibirCabecalho();
            ExibirOpcoes();

            Console.Write("\n🔹 Escolha uma opção: ");
            string? escolha = Console.ReadLine()?.Trim();

            switch (escolha)
            {
                case "1":
                    TrocarMetodoEscalonamento();
                    break;
                case "2":
                    DefinirQuantum();
                    break;
                case "3":
                    _kernel.Escalonador.ExecutarCiclo();
                    Aviso("✅ Ciclo de CPU concluído.");
                    Pausa();
                    break;
                case "4":
                    _kernel.Escalonador.ExecutarAteFinalizarTodos();
                    Aviso("✅ Todos os processos finalizados.");
                    Pausa();
                    break;
                case "5":
                    _kernel.Escalonador.MostrarFilaProntos();
                    Pausa();
                    break;
                case "6":
                    ExibirTrocasContexto();
                    Pausa();
                    break;
                case "0":
                    ativo = false;
                    break;
                default:
                    Aviso("⚠ Opção inválida! Tente novamente...");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    private void ExibirCabecalho()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║           ESCALONADOR DE CPU           ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private void ExibirOpcoes()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n┌───────────────────── MENU ──────────────────────┐");
        Console.WriteLine("│ 1) 🔄 Alterar algoritmo                            │");
        Console.WriteLine("│ 2) ⏱️ Ajustar quantum                               │");
        Console.WriteLine("│ 3) ▶️ Executar ciclo único                           │");
        Console.WriteLine("│ 4) ⏩ Executar até finalização                       │");
        Console.WriteLine("│ 5) 📋 Exibir fila de prontos                         │");
        Console.WriteLine("│ 6) 📊 Estatísticas de troca de contexto             │");
        Console.WriteLine("│ 0) ❌ Retornar                                     │");
        Console.WriteLine("└─────────────────────────────────────────────────┘");
        Console.ResetColor();
    }

    private void TrocarMetodoEscalonamento()
    {
        Console.WriteLine("\nEscolha o algoritmo:");
        Console.WriteLine("a) FCFS");
        Console.WriteLine("b) Round Robin");
        Console.WriteLine("c) Prioridade Preemptiva");
        Console.WriteLine("d) Prioridade Não Preemptiva");
        Console.Write("Opção: ");

        string? op = Console.ReadLine()?.Trim().ToLower();
        string algoritmo = op switch
        {
            "a" => "FCFS",
            "b" => "RR",
            "c" => "PRIORIDADE_PREEMPTIVO",
            "d" => "PRIORIDADE_NAO_PREEMPTIVO",
            _ => ""
        };

        if (!string.IsNullOrEmpty(algoritmo))
        {
            _kernel.Escalonador.TrocarAlgoritmo(algoritmo);
            Aviso($"✅ Algoritmo alterado para {algoritmo}");
            Pausa();
        } else
        {
            Aviso("⚠ Escolha inválida!");
            Pausa();
        }
    }

    private void DefinirQuantum()
    {
        Console.Write("\n⏱️ Informe o quantum: ");
        if (int.TryParse(Console.ReadLine(), out int q) && q > 0)
        {
            _kernel.Quantum = q;
            Aviso($"✅ Quantum atualizado para {q}.");
            Pausa();
        } else
        {
            Aviso("⚠ Valor inválido!");
            Pausa();
        }
    }

    private void ExibirTrocasContexto()
    {
        Console.WriteLine("\n-- Trocas de Contexto --");
        Console.WriteLine($"Total de trocas: {_kernel.Escalonador.TrocaContexto.ContadorTrocas}");
        Console.WriteLine($"Sobrecarga acumulada: {_kernel.Escalonador.TrocaContexto.SobrecargaTotal} ticks");
        Console.WriteLine("------------------------\n");
    }

    private void Aviso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n{mensagem}");
        Console.ResetColor();
    }

    private void Pausa()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey(true);
        Console.ResetColor();
    }
}
