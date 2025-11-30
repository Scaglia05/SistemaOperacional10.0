using SistemaOperacional10._0.Processos;
using SistemaOperacional10._0.SistemaDeArquivosEOutros;

namespace SistemaOperacional10._0.Nucleo
{
    public class Kernel
    {
        public Clock Clock { get; private set; }
        public Configuracoes Configuracoes { get; private set; }
        public CarregadorWorkload CarregadorWorkload { get; private set; }

        public GerenciadorProcessos GerenciadorProcessos { get; private set; }
        public GerenciadorThreads GerenciadorThreads { get; private set; }
        public Escalonador Escalonador { get; private set; }
        public GerenciadorMemoria GerenciadorMemoria { get; private set; }
        public GerenciadorES GerenciadorES { get; private set; }
        public SistemaArquivos SistemaArquivos { get; private set; }
        public GerenciadorMetricas GerenciadorMetricas { get; private set; }

        public Kernel()
        {
            Configuracoes = new Configuracoes();
            Clock = new Clock();

            CarregadorWorkload = new CarregadorWorkload(Configuracoes);


            GerenciadorMemoria = new GerenciadorMemoria(this);
            Escalonador = new Escalonador(this);
            GerenciadorProcessos = new GerenciadorProcessos(this);
            GerenciadorThreads = new GerenciadorThreads(this);
            GerenciadorES = new GerenciadorES(this);
            SistemaArquivos = new SistemaArquivos(this);
            GerenciadorMetricas = new GerenciadorMetricas(this);
        }


        public void Tick(int quantidade = 1)
        {
            Clock.Tick(quantidade);
            GerenciadorES.ProcessarTick();
        }

        public void RegistrarLog(string mensagem)
        {
            Clock.RegistrarEvento(mensagem);
        }

        public void ExportarLog(string caminhoArquivo)
        {
            Clock.ExportarLog(caminhoArquivo);
        }
    }
}