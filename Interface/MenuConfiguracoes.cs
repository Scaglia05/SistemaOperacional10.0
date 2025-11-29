using SimuladorSO.Interface;
using SimuladorSO.Nucleo;
using SimuladorSO.SistemaDeArquivosEOutros;
using System.Threading;

public class MenuConfiguracoes : MenuBase
{
    public MenuConfiguracoes(Kernel kernel) : base(kernel) { }
    private void LogSucesso(string mensagem) => System.Console.WriteLine($"[OK] ✔ {mensagem}");

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
                    ConfigurarSemente();
                    break;
                case "2":
                    ConfigurarTamanhoPagina();
                    break;
                case "3":
                    ConfigurarNumeroMolduras();
                    break;
                case "4":
                    ConfigurarTemposDispositivos();
                    break;
                case "5":
                    CarregarWorkload();
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
        Console.WriteLine("║           CONFIGURAÇÕES DO SO          ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private void ExibirOpcoes()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n┌───────────────────── MENU ──────────────────────┐");
        Console.WriteLine("│ 1) 🎲 Definir semente                              │");
        Console.WriteLine("│ 2) 📏 Tamanho de página                            │");
        Console.WriteLine("│ 3) 🖼️ Número de molduras                             │");
        Console.WriteLine("│ 4) ⏱️ Tempos de dispositivos                         │");
        Console.WriteLine("│ 5) 📂 Carregar workload                              │");
        Console.WriteLine("│ 0) ❌ Voltar                                      │");
        Console.WriteLine("└─────────────────────────────────────────────────┘");
        Console.ResetColor();
    }

    private void Pausa()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey(true);
        Console.ResetColor();
    }

    private void ConfigurarSemente()
    {
        var semente = LerInteiro("🎲 Informe a semente: ");
        if (semente.HasValue)
        {
            Kernel.Seed = semente.Value;
            Randomizador.Inicializar(semente.Value);
            LogSucesso($"Semente configurada com sucesso: {semente.Value}");
            Pausa();
        }
    }


    private void ConfigurarTamanhoPagina()
    {
        var tamanho = LerInteiro("📏 Tamanho da página (bytes): ");
        if (tamanho.HasValue && tamanho > 0)
        {
            Kernel.TamanhoPagina = tamanho.Value;
            Aviso($"✅ Tamanho de página definido: {tamanho.Value} bytes");
            Pausa();
        }
    }

    private void ConfigurarNumeroMolduras()
    {
        var numero = LerInteiro("🖼️ Número de molduras: ");
        if (numero.HasValue && numero > 0)
        {
            Kernel.NumeroMolduras = numero.Value;
            Aviso($"✅ Número de molduras definido: {numero.Value}");
            Pausa();
        }
    }

    private void ConfigurarTemposDispositivos()
    {
        Console.WriteLine("⏱️ Tempos de dispositivos (ticks):");
        var disco = LerInteiro("💽 DISCO: ");
        if (disco.HasValue)
            Kernel.TempoDisco = disco.Value;
        var teclado = LerInteiro("⌨️ TECLADO: ");
        if (teclado.HasValue)
            Kernel.TempoTeclado = teclado.Value;
        var impressora = LerInteiro("🖨️ IMPRESSORA: ");
        if (impressora.HasValue)
            Kernel.TempoImpressora = impressora.Value;
        Aviso("✅ Tempos configurados!");
        Pausa();
    }

    private void CarregarWorkload()
    {
        var caminho = LerEntrada("📂 Caminho do arquivo de workload: ");
        if (!string.IsNullOrEmpty(caminho))
        {
            Kernel.CarregadorWorkload.CarregarArquivo(caminho);
            Aviso("✅ Workload carregado com sucesso!");
            Pausa();
        }
    }

}
