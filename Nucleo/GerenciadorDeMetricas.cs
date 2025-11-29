using System;
using System.Collections.Generic;
using System.Linq;
using SimuladorSO.Enum;
using SimuladorSO.Processos;

namespace SimuladorSO.Nucleo
{
    public class GerenciadorDeMetricas
    {
        private readonly Kernel _kernel;
        private readonly Dictionary<string, Metrica> _metricasProcessos = new();
        private readonly Dictionary<string, Metrica> _metricasDispositivos = new();
        private readonly Metrica _metricasMemoria = new();

        public GerenciadorDeMetricas(Kernel kernel) => _kernel = kernel;

        public void ColetarMetricas()
        {
            foreach (var processo in _kernel.GerenciadorProcessos.ListarProcessos())
            {
                string pid = processo.PCB.PIDSimbolico;
                if (!_metricasProcessos.ContainsKey(pid))
                    _metricasProcessos[pid] = new Metrica { PIDSimbolico = pid };

                var m = _metricasProcessos[pid];

                if (processo.PCB.TempoFinalizacao > 0)
                    m.TempoRetorno = processo.PCB.TempoFinalizacao - processo.PCB.TempoChegada;

                if (m.TempoRetorno > 0)
                    m.TempoEspera = Math.Max(0, m.TempoRetorno - processo.PCB.TempoCPU);

                if (processo.PCB.TempoInicio > 0)
                    m.TempoResposta = processo.PCB.TempoInicio - processo.PCB.TempoChegada;

                m.TempoCPU = processo.PCB.TempoCPU;
            }

            // Memória
            _metricasMemoria.FaltasPagina = _kernel.GerenciadorMemoria.FaltasPagina;
            _metricasMemoria.HitsTLB = _kernel.GerenciadorMemoria.TLB.Hits;
            _metricasMemoria.MissesTLB = _kernel.GerenciadorMemoria.TLB.Misses;

            // CPU
            int tempoTotal = _kernel.Clock.TempoAtual;
            List<Processo> todosProcessos = _kernel.GerenciadorProcessos.ListarProcessos();
            int tempoOcupado = todosProcessos.Sum(p => p.PCB.TempoCPU);
            _metricasMemoria.TempoTotal = tempoTotal;
            _metricasMemoria.TempoOcupado = tempoOcupado;
        }

        private void ExibirMetricasProcessos(Func<Metrica, int> seletor, string titulo, string unidade)
        {
            ColetarMetricas();
            Console.WriteLine($"\n===== {titulo} =====");
            foreach (var m in _metricasProcessos.Values)
                Console.WriteLine($"{m.PIDSimbolico}: {seletor(m)} {unidade}");

            if (_metricasProcessos.Count > 0)
            {
                double media = _metricasProcessos.Values.Average(seletor);
                Console.WriteLine($"\nMédia: {media:F2} {unidade}");
            }
            Console.WriteLine(new string('=', 40));
        }

        public void ExibirTempoRetorno() => ExibirMetricasProcessos(m => m.TempoRetorno, "TEMPO DE RETORNO POR PROCESSO", "ticks");
        public void ExibirTempoEspera() => ExibirMetricasProcessos(m => m.TempoEspera, "TEMPO DE ESPERA EM PRONTO", "ticks");
        public void ExibirTempoResposta() => ExibirMetricasProcessos(m => m.TempoResposta, "TEMPO DE RESPOSTA", "ticks");

        public void ExibirUtilizacaoCPU()
        {
            ColetarMetricas();
            Console.WriteLine("\n===== UTILIZAÇÃO DA CPU =====");
            Console.WriteLine($"Tempo Total: {_metricasMemoria.TempoTotal} ticks");
            Console.WriteLine($"Tempo Ocupado: {_metricasMemoria.TempoOcupado} ticks");
            Console.WriteLine($"Utilização: {_metricasMemoria.Utilizacao:F2}%");
            Console.WriteLine(new string('=', 40));
        }

        public void ExibirThroughput()
        {
            ColetarMetricas();

            int tempoTotal = _kernel.Clock.TempoAtual;
            int processosFinalizados = _kernel.GerenciadorProcessos.ListarProcessosPorEstado(EEstadoProcesso.Finalizado).Count;
            double throughput = tempoTotal > 0 ? (double)processosFinalizados / tempoTotal : 0;

            Console.WriteLine("\n===== THROUGHPUT =====");
            Console.WriteLine($"Processos Finalizados: {processosFinalizados}");
            Console.WriteLine($"Tempo Total: {tempoTotal} ticks");
            Console.WriteLine($"Throughput: {throughput:F4} processos/tick");
            Console.WriteLine(new string('=', 40));
        }

        public void ExibirTrocasContexto()
        {
            Console.WriteLine("\n===== TROCAS DE CONTEXTO =====");
            Console.WriteLine($"Número de Trocas: {_kernel.Escalonador.TrocaContexto.ContadorTrocas}");
            Console.WriteLine($"Sobrecarga Total: {_kernel.Escalonador.TrocaContexto.SobrecargaTotal} ticks");
            Console.WriteLine(new string('=', 40));
        }

        public void ExibirMetricasMemoria()
        {
            ColetarMetricas();
            Console.WriteLine("\n===== MÉTRICAS DE MEMÓRIA =====");
            Console.WriteLine($"Faltas de Página: {_metricasMemoria.FaltasPagina} ({_metricasMemoria.TaxaFaltaPagina:F2}%)");
            Console.WriteLine($"Hits TLB: {_metricasMemoria.HitsTLB}");
            Console.WriteLine($"Misses TLB: {_metricasMemoria.MissesTLB}");
            Console.WriteLine($"Taxa de Acerto TLB: {_metricasMemoria.TaxaAcertoTLB:F2}%");
            Console.WriteLine(new string('=', 40));
        }

        public void ExibirTodasMetricas()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════╗");
            Console.WriteLine("║        RELATÓRIO COMPLETO DE MÉTRICAS          ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");

            ExibirTempoRetorno();
            ExibirTempoEspera();
            ExibirTempoResposta();
            ExibirUtilizacaoCPU();
            ExibirThroughput();
            ExibirTrocasContexto();
            ExibirMetricasMemoria();
        }

        public void ExportarLog(string caminhoArquivo) => _kernel.ExportarLog(caminhoArquivo);
    }

}
