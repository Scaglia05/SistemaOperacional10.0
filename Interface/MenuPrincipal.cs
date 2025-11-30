using System;
using System.Collections.Generic;
using System.Threading;
using SistemaOperacional10._0.Interface;
using SistemaOperacional10._0.Nucleo;
public class MenuPrincipal
{
    private readonly Kernel _kernel;
    private int _indiceSelecionado;
    private List<OpcaoMenu> _opcoes;
    private class OpcaoMenu
    {
        public string Titulo { get; }
        public string Icone { get; }
        public Action Acao { get; }

        public OpcaoMenu(string icone, string titulo, Action acao)
        {
            Icone = icone;
            Titulo = titulo;
            Acao = acao;
        }
    }

    public MenuPrincipal(Kernel kernel)
    {
        _kernel = kernel;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        _indiceSelecionado = 0;
        InicializarOpcoes();
    }

    private void InicializarOpcoes()
    {
        _opcoes = new List<OpcaoMenu>
        {
            new OpcaoMenu("📂", "Gerenciador de Arquivos", () => new MenuArquivo(_kernel).Executar()),
            new OpcaoMenu("🧠", "Gerenciador de Memória",  () => new MenuMemoria(_kernel).Executar()),
            new OpcaoMenu("⚡", "Escalonador de CPU",      () => new MenuEscalonador(_kernel).Executar()),
            new OpcaoMenu("💻", "Dispositivos de I/O",     () => new MenuES(_kernel).Executar()),
            new OpcaoMenu("⚙️", "Configurações do Sistema",() => new MenuConfiguracoes(_kernel).Executar()),
            new OpcaoMenu("❌", "Encerrar Sistema",        () => Environment.Exit(0))
        };
    }

    public void Executar()
    {
        Console.CursorVisible = false;
        ExibirIntro(); 

        bool rodando = true;
        while (rodando)
        {
            DesenharTela();
            LerEntradaUsuario();
        }
    }

    private void LerEntradaUsuario()
    {
        ConsoleKeyInfo tecla = Console.ReadKey(true);

        switch (tecla.Key)
        {
            case ConsoleKey.UpArrow:
                _indiceSelecionado--;
                if (_indiceSelecionado < 0)
                    _indiceSelecionado = _opcoes.Count - 1;
                break;

            case ConsoleKey.DownArrow:
                _indiceSelecionado++;
                if (_indiceSelecionado >= _opcoes.Count)
                    _indiceSelecionado = 0;
                break;

            case ConsoleKey.Enter:
                ExecutarAnimacaoSelecao();
                _opcoes[_indiceSelecionado].Acao.Invoke();
                break;

            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }

    private void DesenharTela()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        // Logo ASCII
        Console.WriteLine(@"
   _____ ____  __  __     ____  _____ 
  / ____/ __ \|  \/  |   / __ \|  __ \
 | (___| |  | | \  / |  | |  | | |__) |
  \___ \ |  | | |\/| |  | |  | |  ___/
  ____) | |__| | |  | |__| |__| | |    
 |_____/ \____/|_|  |_(_) \____/|_|    
        ");

        Console.WriteLine("\n  KERNEL v1.0.0 :: STATUS [ONLINE]");
        Console.WriteLine("  " + new string('=', 40));
        Console.ResetColor();
        Console.WriteLine();

        for (int i = 0; i < _opcoes.Count; i++)
        {
            if (i == _indiceSelecionado)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"  >> {_opcoes[i].Icone}  {_opcoes[i].Titulo.PadRight(30)} <<  ");
                Console.ResetColor();
            } else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"     {_opcoes[i].Icone}  {_opcoes[i].Titulo}");
                Console.ResetColor();
            }
        }

        Console.WriteLine("\n  " + new string('=', 40));
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("  [↑/↓] Navegar   [ENTER] Selecionar   [ESC] Sair");
        Console.ResetColor();
    }

    private void ExecutarAnimacaoSelecao()
    {
        Console.Beep();
        Thread.Sleep(100);
    }

    private void ExibirIntro()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Inicializando Kernel");
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(300);
            Console.Write(".");
        }
        Thread.Sleep(500);
        Console.ResetColor();
    }
}