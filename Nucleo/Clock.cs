using System;
using System.Collections.Generic;
using System.IO;

namespace SimuladorSO.Nucleo
{
    public class Simulador
    {
        // ===== Relógio =====
        public int TempoAtual { get; private set; } = 0;
        public void Tick(int quantidade = 1) => TempoAtual += quantidade;
        public void ResetarClock() => TempoAtual = 0;

        // ===== Registrador de Eventos =====
        private readonly List<string> _eventos = new();
        public void RegistrarEvento(string evento)
        {
            string eventoComTempo = $"[T={TempoAtual}] {evento}";
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

        // ===== Configurações =====
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

        // ===== Workload =====
        public void CarregarWorkload(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"Arquivo não encontrado: {caminhoArquivo}");
                return;
            }

            Console.WriteLine($"\n=== Carregando workload: {caminhoArquivo} ===\n");

            foreach (var linha in File.ReadAllLines(caminhoArquivo))
            {
                string linhaLimpa = linha.Trim();
                if (string.IsNullOrEmpty(linhaLimpa) || linhaLimpa.StartsWith("#"))
                    continue;

                ProcessarComando(linhaLimpa);
            }

            Console.WriteLine("\n=== Workload carregado com sucesso ===\n");
        }

        private void ProcessarComando(string comando)
        {
            var partes = comando.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0)
                return;

            string cmd = partes[0].ToUpper();

            try
            {
                switch (cmd)
                {
                    case "SET_SEED":
                        if (partes.Length >= 2)
                            Seed = int.Parse(partes[1]);
                        break;
                    case "SET_QUANTUM":
                        if (partes.Length >= 2)
                            Quantum = int.Parse(partes[1]);
                        break;
                    case "SET_ESCALONADOR":
                        if (partes.Length >= 2)
                            AlgoritmoEscalonamento = partes[1];
                        break;
                    case "SET_TAMANHO_PAGINA":
                        if (partes.Length >= 2)
                            TamanhoPagina = int.Parse(partes[1]);
                        break;
                    case "SET_FRAMES":
                        if (partes.Length >= 2)
                            NumeroMolduras = int.Parse(partes[1]);
                        break;
                    default:
                        Console.WriteLine($"Comando desconhecido: {cmd}");
                        break;
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar comando '{comando}': {ex.Message}");
            }
        }
    }
}
