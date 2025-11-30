using System;
using System.Collections.Generic;
using System.IO;

namespace SistemaOperacional10._0.Nucleo
{
    public class Clock
    {
        public int TempoAtual { get; private set; } = 0;
        private readonly List<string> _eventos = new();

        public void Tick(int quantidade = 1)
        {
            TempoAtual += quantidade;
        }

        public void ResetarClock()
        {
            TempoAtual = 0;
            LimparEventos();
        }

        public void RegistrarEvento(string evento)
        {
            string eventoComTempo = $"[T={TempoAtual}] {evento}";
            _eventos.Add(eventoComTempo);
            Console.WriteLine(eventoComTempo);
        }

        public List<string> ObterEventos() => new(_eventos);

        public void LimparEventos() => _eventos.Clear();

        public void ExportarLog(string caminhoArquivo)
        {
            try
            {
                File.WriteAllLines(caminhoArquivo, _eventos);
                Console.WriteLine($"[LOG] Exportado para: {caminhoArquivo}");
            } catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao exportar log: {ex.Message}");
            }
        }
    }
}