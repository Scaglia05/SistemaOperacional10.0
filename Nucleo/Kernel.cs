using System;
using System.Collections.Generic;
using System.IO;

namespace SimuladorSO.Nucleo
{
    public class Kernel
    {
        // Core
        public Simulador Clock { get; private set; }  // substituído Clock por Clock
        private readonly List<string> _eventos = new();

        // Configurações
        public int Seed { get; set; } = 0;
        public int Quantum { get; set; } = 4;
        public int TamanhoPagina { get; set; } = 1024;
        public int NumeroMolduras { get; set; } = 16;
        public string AlgoritmoEscalonamento { get; set; } = "RR";
        public int TempoDisco { get; set; } = 30;
        public int TempoTeclado { get; set; } = 10;
        public int TempoImpressora { get; set; } = 40;
        public bool TLBAtivada { get; set; } = true;
        public int TamanhoTLB { get; set; } = 8;

        // Gerenciadores (stubs)
        public dynamic GerenciadorProcessos { get; private set; }
        public dynamic GerenciadorThreads { get; private set; }
        public dynamic Escalonador { get; private set; }
        public dynamic GerenciadorMemoria { get; private set; }
        public dynamic GerenciadorES { get; private set; }
        public dynamic SistemaArquivos { get; private set; }
        public dynamic GerenciadorMetricas { get; private set; }
        public dynamic CarregadorWorkload { get; private set; }


        public Kernel()
        {
            Clock = new Simulador();  // instância do Clock

            // Inicializar gerenciadores (substitua pelos reais)
            GerenciadorProcessos = new object();
            GerenciadorThreads = new object();
            Escalonador = new object();
            GerenciadorMemoria = new object();
            GerenciadorES = new object();
            SistemaArquivos = new object();
            GerenciadorMetricas = new object();
            CarregadorWorkload = new object();
        }

        // Eventos usando Clock
        public void RegistrarEvento(string evento)
        {
            string eventoComTempo = $"[T={Clock.TempoAtual}] {evento}";
            _eventos.Add(eventoComTempo);
            Console.WriteLine(eventoComTempo);
        }

        public void ExportarLog(string caminhoArquivo)
        {
            try
            {
                File.WriteAllLines(caminhoArquivo, _eventos);
                Console.WriteLine($"Log exportado para: {caminhoArquivo}");
            } catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exportar log: {ex.Message}");
            }
        }

        public List<string> ObterEventos() => new(_eventos);
        public void LimparEventos() => _eventos.Clear();

        // Tick
        public void Tick(int quantidade = 1) => Clock.Tick(quantidade);
        public void ResetarTempo() => Clock.ResetarClock();
    }

}
