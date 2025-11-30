using SistemaOperacional10._0.Enum;
using SistemaOperacional10._0.Nucleo;
using SistemaOperacional10._0.SistemaDeArquivosEOutros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaOperacional10._0.Processos
{
    public class GerenciadorProcessos
    {
        private readonly Kernel _kernel;
        private readonly Dictionary<int, Processo> _processos = new();
        private readonly Dictionary<string, int> _mapeamentoPID = new();

        public GerenciadorProcessos(Kernel kernel) => _kernel = kernel;

        public Processo CriarProcesso(string pidSimbolico, int prioridade)
        {
            int pid = GeradorIdentificadores.NovoPID();
            var processo = new Processo(pid, pidSimbolico, prioridade, _kernel.Clock.TempoAtual);
            processo.MudarEstado(EEstadoProcesso.Pronto);

            _processos[pid] = processo;
            _mapeamentoPID[pidSimbolico] = pid;

            _kernel.RegistrarLog($"Processo criado: {pidSimbolico} (PID={pid}, Prioridade={prioridade})");
            return processo;
        }

        public void RemoverProcesso(int pid)
        {
            if (_processos.Remove(pid, out var processo))
            {
                _mapeamentoPID.Remove(processo.PCB.PIDSimbolico);
                _kernel.RegistrarLog($"Processo removido: PID={pid}");
            }
        }

        public void FinalizarProcesso(string pidSimbolico)
        {
            if (_mapeamentoPID.TryGetValue(pidSimbolico, out int pid) &&
                _processos.TryGetValue(pid, out var processo))
            {
                processo.MudarEstado(EEstadoProcesso.Finalizado);
                processo.PCB.TempoFinalizacao = _kernel.Clock.TempoAtual;
                _kernel.RegistrarLog($"Processo finalizado: {pidSimbolico} (PID={pid})");
            }
        }

        public void MudarEstado(int pid, EEstadoProcesso novoEstado)
        {
            if (_processos.TryGetValue(pid, out var processo))
            {
                processo.MudarEstado(novoEstado);
                _kernel.RegistrarLog($"Processo PID={pid} mudou para estado: {novoEstado}");
            }
        }

        public Processo? ObterProcesso(int pid) => _processos.TryGetValue(pid, out var p) ? p : null;

        public Processo? ObterProcessoPorSimbolico(string pidSimbolico) =>
            _mapeamentoPID.TryGetValue(pidSimbolico, out int pid) ? ObterProcesso(pid) : null;

        public List<Processo> ListarProcessos() => _processos.Values.ToList();

        public List<Processo> ListarProcessosPorEstado(EEstadoProcesso estado) =>
            _processos.Values.Where(p => p.PCB.Estado == estado).ToList();

        public void ExibirPCB(int pid)
        {
            if (!_processos.TryGetValue(pid, out var processo))
            {
                Console.WriteLine($"Processo PID={pid} não encontrado.");
                return;
            }

            PCB pcb = processo.PCB;

            Console.WriteLine("\n========== PCB COMPLETO ==========");

            var propriedades = new Dictionary<string, object>
{
    { "PID", pcb.PID },
    { "PID Simbólico", pcb.PIDSimbolico },
    { "Estado", pcb.Estado },
    { "Prioridade", pcb.Prioridade },
    { "Contador de Programa", pcb.ContadorPrograma },
    { "Tempo de Chegada", pcb.TempoChegada },
    { "Tempo de Início", pcb.TempoInicio },
    { "Tempo de Finalização", pcb.TempoFinalizacao },
    { "Tempo de CPU", pcb.TempoCPU },
    { "Tempo de Espera", pcb.TempoEspera },
    { "Quantum Restante", pcb.QuantumRestante },
    { "Tabela de Páginas ID", pcb.TabelaPaginasID },
    { "Arquivos Abertos", string.Join(", ", pcb.ArquivosAbertos) },
    { "Threads", string.Join(", ", pcb.ThreadsIDs) }
};

            foreach (var (rotulo, valor) in propriedades)
                Console.WriteLine($"{rotulo}: {valor}");

            Console.WriteLine("==================================\n");

        }

    }
}
