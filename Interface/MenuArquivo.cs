using System;
using System.Collections.Generic;
using System.Threading;
using SistemaOperacional10._0.Nucleo;

namespace SistemaOperacional10._0.Interface
{
    public class MenuArquivo
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

        public MenuArquivo(Kernel kernel)
        {
            _kernel = kernel;
            _indiceSelecionado = 0;
            InicializarOpcoes();
        }

        private void InicializarOpcoes()
        {
            _opcoes = new List<OpcaoMenu>
            {
                new OpcaoMenu("📂", "Listar Diretório Atual",  () => TelaListarDiretorio()),
                new OpcaoMenu("📝", "Criar Arquivo",           () => TelaCriarArquivo()),
                new OpcaoMenu("📁", "Criar Diretório",         () => TelaCriarDiretorio()),
                new OpcaoMenu("🔓", "Abrir Arquivo",           () => TelaAbrirArquivo()),
                new OpcaoMenu("📖", "Ler Arquivo",             () => TelaLerArquivo()),
                new OpcaoMenu("✍️", "Escrever em Arquivo",     () => TelaEscreverArquivo()),
                new OpcaoMenu("🔒", "Fechar Arquivo",          () => TelaFecharArquivo()),
                new OpcaoMenu("🗑️", "Apagar Arquivo",          () => TelaApagarArquivo()),
                new OpcaoMenu("🚀", "Mudar Diretório (cd)",    () => TelaMudarDiretorio()),
                new OpcaoMenu("⬅️", "Voltar ao Menu Principal",() => {  })
            };
        }

        public void Executar()
        {
            Console.CursorVisible = false;
            bool emMenu = true;

            while (emMenu)
            {
                DesenharTela();
                var tecla = Console.ReadKey(true);

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
                        if (_indiceSelecionado == _opcoes.Count - 1)
                        {
                            emMenu = false;
                        } else
                        {
                            ExecutarAnimacaoSelecao();
                            Console.Clear();
                            _opcoes[_indiceSelecionado].Acao.Invoke();
                        }
                        break;

                    case ConsoleKey.Escape:
                        emMenu = false;
                        break;
                }
            }
        }

        private void DesenharTela()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(@"
   _____ ___ _     ____  __   __ ____  _____ 
  |  ___|_ _| |   |  _ \ \ \ / // ___||_   _|
  | |_   | || |   | |_) | \ V / \___ \  | |  
  |  _|  | || |___|  __/   | |   ___) | | |  
  |_|   |___|_____|_|      |_|  |____/  |_|  
            ");

            Console.WriteLine("\n  GERENCIADOR DE ARQUIVOS :: v1.0");
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
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("  [↑/↓] Navegar   [ENTER] Executar   [ESC] Voltar");
            Console.ResetColor();
        }

        private void ExecutarAnimacaoSelecao()
        {
            Console.Beep();
        }

        private void ExibirCabecalhoAcao(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  :: {titulo.ToUpper()} ::");
            Console.WriteLine("  " + new string('-', 30) + "\n");
            Console.ResetColor();
        }

        private string? LerTexto(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  {label}: ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        private int? LerInt(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  {label}: ");
            Console.ResetColor();
            if (int.TryParse(Console.ReadLine(), out int valor))
                return valor;
            return null;
        }

        private void Pausa()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Pressione qualquer tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        private void MensagemSucesso(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✅ {msg}");
            Console.ResetColor();
        }

        private void MensagemErro(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  ❌ {msg}");
            Console.ResetColor();
        }

        private void TelaListarDiretorio()
        {
            ExibirCabecalhoAcao("Conteúdo do Diretório");
            _kernel.SistemaArquivos.ListarDiretorio();
            Pausa();
        }

        private void TelaCriarArquivo()
        {
            ExibirCabecalhoAcao("Novo Arquivo");
            var caminho = LerTexto("Caminho (ex: /docs/nota.txt)");

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.CriarArquivo(caminho);
                MensagemSucesso("Comando enviado ao Kernel.");
            } else
                MensagemErro("Caminho inválido.");
            Pausa();
        }

        private void TelaCriarDiretorio()
        {
            ExibirCabecalhoAcao("Novo Diretório");
            var nome = LerTexto("Nome do diretório");

            if (!string.IsNullOrEmpty(nome))
            {
                _kernel.SistemaArquivos.CriarDiretorio(nome);
                MensagemSucesso("Comando enviado ao Kernel.");
            } else
                MensagemErro("Nome inválido.");
            Pausa();
        }

        private void TelaAbrirArquivo()
        {
            ExibirCabecalhoAcao("Abrir Arquivo");
            var pid = LerTexto("PID do Processo");
            var caminho = LerTexto("Caminho do Arquivo");

            if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.AbrirArquivo(pid, caminho);
                MensagemSucesso("Tentativa de abertura iniciada.");
            } else
                MensagemErro("Dados incompletos.");
            Pausa();
        }

        private void TelaLerArquivo()
        {
            ExibirCabecalhoAcao("Ler Arquivo");
            var pid = LerTexto("PID do Processo");
            var caminho = LerTexto("Caminho do Arquivo");
            var tamanho = LerInt("Bytes para ler");

            if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho) && tamanho.HasValue)
            {
                _kernel.SistemaArquivos.LerArquivo(pid, caminho, tamanho.Value);
            } else
                MensagemErro("Dados inválidos.");
            Pausa();
        }

        private void TelaEscreverArquivo()
        {
            ExibirCabecalhoAcao("Escrever em Arquivo");
            var pid = LerTexto("PID do Processo");
            var caminho = LerTexto("Caminho do Arquivo");
            var tamanho = LerInt("Bytes para escrever");

            if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho) && tamanho.HasValue)
            {
                _kernel.SistemaArquivos.EscreverArquivo(pid, caminho, tamanho.Value);
            } else
                MensagemErro("Dados inválidos.");
            Pausa();
        }

        private void TelaFecharArquivo()
        {
            ExibirCabecalhoAcao("Fechar Arquivo");
            var pid = LerTexto("PID do Processo");
            var caminho = LerTexto("Caminho do Arquivo");

            if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.FecharArquivo(pid, caminho);
                MensagemSucesso("Solicitação de fechamento enviada.");
            } else
                MensagemErro("Dados incompletos.");
            Pausa();
        }

        private void TelaApagarArquivo()
        {
            ExibirCabecalhoAcao("Apagar Arquivo");
            var caminho = LerTexto("Caminho do arquivo");

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.ApagarArquivo(caminho);
                MensagemSucesso("Solicitação de exclusão enviada.");
            } else
                MensagemErro("Caminho inválido.");
            Pausa();
        }

        private void TelaMudarDiretorio()
        {
            ExibirCabecalhoAcao("Navegar (CD)");
            var nome = LerTexto("Diretório (ou '..')");

            if (!string.IsNullOrEmpty(nome))
            {
                _kernel.SistemaArquivos.MudarDiretorio(nome);
                MensagemSucesso("Comando de navegação enviado.");
            } else
                MensagemErro("Caminho inválido.");
            Pausa();
        }
    }
}