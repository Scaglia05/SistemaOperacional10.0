using SimuladorSO.Nucleo;
using System.Threading;

public class MenuMemoria
{
    private Kernel _kernel;
    public MenuMemoria(Kernel kernel) => _kernel = kernel;

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
                    MostrarPaginas();
                    break;
                case "2":
                    Alocar();
                    break;
                case "3":
                    Liberar();
                    break;
                case "4":
                    _kernel.GerenciadorMemoria.MostrarMapaMolduras();
                    Pausa();
                    break;
                case "5":
                    AlternarTLB();
                    break;
                case "6":
                    _kernel.GerenciadorMemoria.MostrarEstatisticasTLB();
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
        Console.WriteLine("║                MEMÓRIA                 ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private void ExibirOpcoes()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n┌───────────────────── MENU ──────────────────────┐");
        Console.WriteLine("│ 1) 📄 Tabela de páginas/segmentos                 │");
        Console.WriteLine("│ 2) 🟢 Alocar memória                               │");
        Console.WriteLine("│ 3) 🔴 Liberar memória                               │");
        Console.WriteLine("│ 4) 🗂️ Mapa de molduras                               │");
        Console.WriteLine("│ 5) 🔄 Ativar/Desativar TLB                           │");
        Console.WriteLine("│ 6) 📊 Estatísticas TLB                               │");
        Console.WriteLine("│ 0) ❌ Voltar                                      │");
        Console.WriteLine("└─────────────────────────────────────────────────┘");
        Console.ResetColor();
    }

    private void MostrarPaginas()
    {
        var pid = LerEntrada("🆔 PID do processo: ");
        if (!string.IsNullOrEmpty(pid))
        {
            _kernel.GerenciadorMemoria.MostrarTabelaPaginas(pid);
            Pausa();
        }
    }

    private void Alocar()
    {
        var pid = LerEntrada("🆔 PID do processo: ");
        var quantidade = LerInteiro("📏 Quantidade (bytes): ");
        if (!string.IsNullOrEmpty(pid) && quantidade.HasValue)
        {
            _kernel.GerenciadorMemoria.AlocarMemoria(pid, quantidade.Value);
            Aviso("✅ Memória alocada com sucesso!");
            Pausa();
        }
    }

    private void Liberar()
    {
        var pid = LerEntrada("🆔 PID do processo: ");
        if (!string.IsNullOrEmpty(pid))
        {
            _kernel.GerenciadorMemoria.LiberarMemoria(pid);
            Aviso("✅ Memória liberada!");
            Pausa();
        }
    }

    private void AlternarTLB()
    {
        _kernel.TLBAtivada = !_kernel.TLBAtivada;
        Aviso($"🔄 TLB agora {_kernel.TLBAtivada}");
        Pausa();
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

    private string? LerEntrada(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim();
    }

    private int? LerInteiro(string prompt)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int valor))
            return valor;
        return null;
    }

}
