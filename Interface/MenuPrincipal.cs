using SimuladorSO.Nucleo;

public class MenuPrincipal
{
    private Kernel _kernel;
    public MenuPrincipal(Kernel kernel)
    {
        _kernel = kernel;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

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
                    new MenuArquivo(_kernel).Executar();
                    break;
                case "2":
                    new MenuMemoria(_kernel).Executar();
                    break;
                case "3":
                    new MenuEscalonador(_kernel).Executar();
                    break;
                case "4":
                    new MenuES(_kernel).Executar();
                    break;
                case "5":
                    new MenuConfiguracoes(_kernel).Executar();
                    break;
                case "0":
                    ativo = false;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n⚠ Opção inválida! Tente novamente...");
                    Console.ResetColor();
                    Thread.Sleep(1200);
                    break;
            }
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Simulador encerrado. Até a próxima!");
        Console.ResetColor();
    }

    private void ExibirCabecalho()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║        SIMULADOR DE SISTEMA OPERACIONAL  ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private void ExibirOpcoes()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n┌─────────────────────── MENU ────────────────────────┐");
        Console.WriteLine("│ 1) 📂 Sistema de Arquivos                            │");
        Console.WriteLine("│ 2) 🧠 Memória                                        │");
        Console.WriteLine("│ 3) ⚡ Escalonador                                    │");
        Console.WriteLine("│ 4) 💻 Dispositivos I/O                               │");
        Console.WriteLine("│ 5) ⚙️ Configurações                                  │");
        Console.WriteLine("│ 0) ❌ Sair                                          │");
        Console.WriteLine("└─────────────────────────────────────────────────────┘");
        Console.ResetColor();
    }

}
