using SimuladorSO.Enum;
using SimuladorSO.Nucleo;
using SimuladorSO.SistemaDeArquivosEOutros;
using System.Collections.Generic;
using System.Linq;

namespace SimuladorSO.Processos
{
    public class GerenciadorDeThreads
    {
        private readonly Kernel _kernel;
        private readonly Dictionary<int, ThreadSimulada> _threads = new();
        public GerenciadorDeThreads(Kernel kernel) => _kernel = kernel;

        public ThreadSimulada? CriarThread(string pidSimbolico)
        {
            var processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);
            if (processo == null)
            {
                LogAviso($"Não achei o processo '{pidSimbolico}'!");
                return null;
            }

            int tid = GeradorIdentificadores.NovoTID();
            var thread = new ThreadSimulada(tid, processo.PCB.PID, _kernel.Clock.TempoAtual);
            thread.MudarEstado(EstadoThread.Pronto);

            _threads[tid] = thread;
            processo.PCB.ThreadsIDs.Add(tid);

            LogInfo($"Thread gerada: [TID:{tid}] para processo '{pidSimbolico}' (PID:{processo.PCB.PID})");
            return thread;
        }

        public void RemoverThread(int tid)
        {
            if (!_threads.TryGetValue(tid, out var thread))
                return;

            var processo = _kernel.GerenciadorProcessos.ObterProcesso(thread.TCB.PIDProcesso);
            processo?.PCB.ThreadsIDs.Remove(tid);

            _threads.Remove(tid);
            LogInfo($"Thread [TID:{tid}] descartada.");
        }

        public void MudarEstado(int tid, EstadoThread novoEstado)
        {
            if (_threads.TryGetValue(tid, out var thread))
            {
                thread.MudarEstado(novoEstado);
                LogInfo($"Thread [TID:{tid}] agora está: {novoEstado}");
            }
        }

        public ThreadSimulada? ObterThread(int tid) => _threads.TryGetValue(tid, out var thread) ? thread : null;

        public List<ThreadSimulada> ListarThreads() => _threads.Values.ToList();

        public List<ThreadSimulada> ListarThreadsDoProcesso(int pid) => _threads.Values.Where(t => t.TCB.PIDProcesso == pid).ToList();

        public void ExibirTCB(int tid)
        {
            if (!_threads.TryGetValue(tid, out var thread))
            {
                LogAviso($"Thread [TID:{tid}] não localizada!");
                return;
            }

            var tcb = thread.TCB;
            LogInfo("---- DETALHES DO TCB ----");
            LogInfo($"ID da Thread: {tcb.TID}");
            LogInfo($"Processo (PID): {tcb.PIDProcesso}");
            LogInfo($"Estado Atual: {tcb.Estado}");
            LogInfo($"PC: {tcb.ContadorPrograma}");
            LogInfo($"Tempo CPU: {tcb.TempoCPU}");
            LogInfo($"Chegada: {tcb.TempoChegada}");
            LogInfo("--------------------------");
        }

        // Métodos auxiliares para logs personalizados
        private void LogInfo(string mensagem) => Console.WriteLine($"[INFO] >> {mensagem}");
        private void LogAviso(string mensagem) => Console.WriteLine($"[AVISO] !! {mensagem}");
    }
}
