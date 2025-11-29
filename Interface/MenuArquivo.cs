using SimuladorSO.Interface;
using SimuladorSO.Nucleo;


public class MenuArquivo : MenuBase
{
    public MenuArquivo(Kernel kernel) : base(kernel) { }

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
                    Kernel.SistemaArquivos.ListarDiretorio();
                    Pausa();
                    break;
                case "2":
                    CriarArquivo();
                    break;
                case "3":
                    CriarDiretorio();
                    break;
                case "4":
                    AbrirArquivo();
                    break;
                case "5":
                    LerArquivo();
                    break;
                case "6":
                    EscreverArquivo();
                    break;
                case "7":
                    FecharArquivo();
                    break;
                case "8":
                    ApagarArquivo();
                    break;
                case "9":
                    MudarDiretorio();
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
        Console.WriteLine("║         SISTEMA DE ARQUIVOS            ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private void ExibirOpcoes()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n┌───────────────────── MENU ──────────────────────┐");
        Console.WriteLine("│ 1) 📂 Listar diretório                            │");
        Console.WriteLine("│ 2) 📝 Criar arquivo                               │");
        Console.WriteLine("│ 3) 📁 Criar diretório                              │");
        Console.WriteLine("│ 4) 🔓 Abrir arquivo                                │");
        Console.WriteLine("│ 5) 📖 Ler arquivo                                  │");
        Console.WriteLine("│ 6) ✍️ Escrever arquivo                             │");
        Console.WriteLine("│ 7) 🔒 Fechar arquivo                               │");
        Console.WriteLine("│ 8) 🗑️ Apagar arquivo                               │");
        Console.WriteLine("│ 9) 📂 Mudar diretório                               │");
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

    private void CriarArquivo()
    {
        var caminho = LerEntrada("📄 Caminho do arquivo: ");
        if (!string.IsNullOrEmpty(caminho))
        {
            Kernel.SistemaArquivos.CriarArquivo(caminho);
            Aviso("✅ Arquivo criado com sucesso!");
            Pausa();
        }
    }

    private void CriarDiretorio()
    {
        var nome = LerEntrada("📁 Nome do diretório: ");
        if (!string.IsNullOrEmpty(nome))
        {
            Kernel.SistemaArquivos.CriarDiretorio(nome);
            Aviso("✅ Diretório criado com sucesso!");
            Pausa();
        }
    }

    private void AbrirArquivo()
    {
        var pid = LerEntrada("🆔 PID simbólico: ");
        var caminho = LerEntrada("📄 Caminho do arquivo: ");
        if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
        {
            Kernel.SistemaArquivos.AbrirArquivo(pid, caminho);
            Aviso("🔓 Arquivo aberto!");
            Pausa();
        }
    }

    private void LerArquivo()
    {
        var pid = LerEntrada("🆔 PID simbólico: ");
        var caminho = LerEntrada("📄 Caminho do arquivo: ");
        var tamanho = LerInteiro("📏 Tamanho a ler (bytes): ");
        if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho) && tamanho.HasValue)
        {
            Kernel.SistemaArquivos.LerArquivo(pid, caminho, tamanho.Value);
            Pausa();
        }
    }

    private void EscreverArquivo()
    {
        var pid = LerEntrada("🆔 PID simbólico: ");
        var caminho = LerEntrada("📄 Caminho do arquivo: ");
        var tamanho = LerInteiro("📏 Tamanho a escrever (bytes): ");
        if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho) && tamanho.HasValue)
        {
            Kernel.SistemaArquivos.EscreverArquivo(pid, caminho, tamanho.Value);
            Aviso("✍️ Escrita concluída!");
            Pausa();
        }
    }

    private void FecharArquivo()
    {
        var pid = LerEntrada("🆔 PID simbólico: ");
        var caminho = LerEntrada("📄 Caminho do arquivo: ");
        if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
        {
            Kernel.SistemaArquivos.FecharArquivo(pid, caminho);
            Aviso("🔒 Arquivo fechado!");
            Pausa();
        }
    }

    private void ApagarArquivo()
    {
        var caminho = LerEntrada("🗑️ Caminho do arquivo: ");
        if (!string.IsNullOrEmpty(caminho))
        {
            Kernel.SistemaArquivos.ApagarArquivo(caminho);
            Aviso("🗑️ Arquivo apagado!");
            Pausa();
        }
    }

    private void MudarDiretorio()
    {
        var nome = LerEntrada("📂 Nome do diretório ('..' para voltar): ");
        if (!string.IsNullOrEmpty(nome))
        {
            Kernel.SistemaArquivos.MudarDiretorio(nome);
            Aviso("📁 Diretório alterado!");
            Pausa();
        }
    }
}
