using System;
using System.IO;

namespace SistemaOperacional10._0.Nucleo
{
    public class CarregadorWorkload
    {
        private readonly Configuracoes _config;

        public CarregadorWorkload(Configuracoes config)
        {
            _config = config;
        }

        public void CarregarArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"[ERRO] Arquivo não encontrado: {caminhoArquivo}");
                return;
            }

            Console.WriteLine($"\n=== Carregando workload: {caminhoArquivo} ===\n");

            var linhas = File.ReadAllLines(caminhoArquivo);
            foreach (var linha in linhas)
            {
                ProcessarLinha(linha);
            }

            Console.WriteLine("\n=== Workload carregado com sucesso ===\n");
        }

        private void ProcessarLinha(string linha)
        {
            string linhaLimpa = linha.Trim();
            if (string.IsNullOrEmpty(linhaLimpa) || linhaLimpa.StartsWith("#"))
                return;

            var partes = linhaLimpa.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length < 2)
                return;

            string comando = partes[0].ToUpper();
            string valor = partes[1];

            try
            {
                switch (comando)
                {
                    case "SET_SEED":
                        _config.Seed = int.Parse(valor);
                        break;
                    case "SET_QUANTUM":
                        _config.Quantum = int.Parse(valor);
                        break;
                    case "SET_ESCALONADOR":
                        _config.AlgoritmoEscalonamento = valor;
                        break;
                    case "SET_TAMANHO_PAGINA":
                        _config.TamanhoPagina = int.Parse(valor);
                        break;
                    case "SET_FRAMES":
                        _config.NumeroMolduras = int.Parse(valor);
                        break;
                    default:
                        Console.WriteLine($"[AVISO] Comando desconhecido no workload: {comando}");
                        break;
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao processar comando '{linhaLimpa}': {ex.Message}");
            }
        }
    }
}