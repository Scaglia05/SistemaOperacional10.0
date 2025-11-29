using SimuladorSO.Interface;
using SimuladorSO.Nucleo;
using System.Threading;

public class MenuES : MenuBase
{
    public MenuES(Kernel kernel) : base(kernel) { }
    public void Executar()
    {
        bool ativo = true;
        while (ativo)
        {
            Console.Clear();
            ExibirCabecalho();
            ExibirOpcoes();

            Console.Write("\n🔹 Escolha uma opção: ");
            string? opcao = Console.ReadLine()?.Trim();

            switch (opcao)
            {
                case "1":
                    Kernel.GerenciadorES.ListarDispositivos();
                    Pausa();
                    break;
                case "2":
                    CriarRequisicao(true);
                    break;
                case "3":
                    CriarRequisicao(false);
                    break;
                case "4":
                    Kernel.GerenciadorES.ProcessarTick();
                    Aviso("✅ 1 tick processado.");
                    Pausa();
                    break;
                case "5":
                    Kernel.GerenciadorES.MostrarFilasDispositivos();
                    Pausa();
                    break;
                case "6":
                    Kernel.GerenciadorES.MostrarInterrupcoes();
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
        Console.WriteLine("║          DISPOSITIVOS DE I/O           ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private void ExibirOpcoes()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n┌───────────────────── MENU ──────────────────────┐");
        Console.WriteLine("│ 1) 💻 Listar dispositivos                         │");
        Console.WriteLine("│ 2) ⏳ Requisição bloqueante                        │");
        Console.WriteLine("│ 3) ⚡ Requisição não bloqueante                     │");
        Console.WriteLine("│ 4) ⏱️ Processar 1 tick                               │");
        Console.WriteLine("│ 5) 📋 Ver filas de dispositivos                     │");
        Console.WriteLine("│ 6) 🛎️ Ver interrupções                               │");
        Console.WriteLine("│ 0) ❌ Voltar                                      │");
        Console.WriteLine("└─────────────────────────────────────────────────┘");
        Console.ResetColor();
    }

    private void CriarRequisicao(bool bloqueante)
    {
        var pid = LerEntrada("🆔 PID simbólico: ");
        var disp = LerEntrada("💻 Dispositivo (DISCO, TECLADO, IMPRESSORA): ")?.ToUpper();
        var tempo = LerInteiro("⏱️ Tempo (ticks): ");

        if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(disp) && tempo.HasValue)
        {
            Kernel.GerenciadorES.CriarRequisicao(pid, disp, tempo.Value, bloqueante);
            Aviso($"✅ {(bloqueante ? "Bloqueante" : "Não bloqueante")} criada!");
            Pausa();
        }
    }

    private void Pausa()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey(true);
        Console.ResetColor();
    }
}
