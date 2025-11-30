using SistemaOperacional10._0.Nucleo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SistemaOperacional10._0.Interface
{
    public abstract class MenuBase
    {
        protected Kernel Kernel { get; }
        protected class OpcaoMenu
        {
            public string Icone { get; }
            public string Titulo { get; }
            public Action Acao { get; }

            public OpcaoMenu(string icone, string titulo, Action acao)
            {
                Icone = icone;
                Titulo = titulo;
                Acao = acao;
            }
        }

        protected MenuBase(Kernel kernel)
        {
            Kernel = kernel;
            Console.OutputEncoding = Encoding.UTF8;
        }
        protected void ExecutarMenu(string tituloMenu, string asciiArt, List<OpcaoMenu> opcoes)
        {
            int indiceSelecionado = 0;
            bool emMenu = true;
            Console.CursorVisible = false;

            while (emMenu)
            {
                DesenharTela(tituloMenu, asciiArt, opcoes, indiceSelecionado);

                var tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.UpArrow:
                        indiceSelecionado--;
                        if (indiceSelecionado < 0)
                            indiceSelecionado = opcoes.Count - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        indiceSelecionado++;
                        if (indiceSelecionado >= opcoes.Count)
                            indiceSelecionado = 0;
                        break;

                    case ConsoleKey.Enter:
                        opcoes[indiceSelecionado].Acao.Invoke();

                        if (opcoes[indiceSelecionado].Titulo.Contains("Voltar") ||
                            opcoes[indiceSelecionado].Titulo.Contains("Sair"))
                        {
                            emMenu = false;
                        }
                        break;

                    case ConsoleKey.Escape:
                        emMenu = false;
                        break;
                }
            }
        }

        private void DesenharTela(string titulo, string asciiArt, List<OpcaoMenu> opcoes, int indice)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;

            if (!string.IsNullOrWhiteSpace(asciiArt))
            {
                Console.WriteLine(asciiArt);
            }

            Console.WriteLine($"\n  {titulo.ToUpper()}");
            Console.WriteLine("  " + new string('=', 40));
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < opcoes.Count; i++)
            {
                if (i == indice)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"  >> {opcoes[i].Icone}  {opcoes[i].Titulo.PadRight(30)} <<  ");
                    Console.ResetColor();
                } else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"     {opcoes[i].Icone}  {opcoes[i].Titulo}");
                    Console.ResetColor();
                }
            }

            ExibirRodape();
        }

        private void ExibirRodape()
        {
            Console.WriteLine("\n  " + new string('=', 40));
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("  [↑/↓] Navegar   [ENTER] Selecionar   [ESC] Voltar");
            Console.ResetColor();
        }

        protected void ExibirCabecalhoAcao(string tituloAcao)
        {
            Console.Clear(); // Limpa o menu para focar na ação
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  :: {tituloAcao.ToUpper()} ::");
            Console.WriteLine("  " + new string('-', 30) + "\n");
            Console.ResetColor();
        }

        protected string? LerTexto(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  {label}: ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        protected int? LerInt(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  {label}: ");
            Console.ResetColor();

            string? entrada = Console.ReadLine();
            if (int.TryParse(entrada, out int valor))
                return valor;

            return null;
        }

        protected void MensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✅ {mensagem}");
            Console.ResetColor();
        }

        protected void MensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  ❌ {mensagem}");
            Console.ResetColor();
        }

        protected void Pausa()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Pressione qualquer tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        protected string? LerEntrada(string mensagem) => LerTexto(mensagem.Replace(": ", ""));
        protected int? LerInteiro(string mensagem) => LerInt(mensagem.Replace(": ", ""));
        protected void Aviso(string mensagem) => MensagemErro(mensagem);
    }
}