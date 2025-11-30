using SistemaOperacional10._0.Enum;
using System.Collections.Generic;

namespace SistemaOperacional10._0.Processos
{
    public class PCB
    {
        public int PID { get; set; }
        public string PIDSimbolico { get; set; }
        public EEstadoProcesso Estado { get; set; }
        public int Prioridade { get; set; }
        public int ContadorPrograma { get; set; }
        public int TempoChegada { get; set; }
        public int TempoInicio { get; set; } = -1;
        public int TempoFinalizacao { get; set; } = -1;
        public int TempoCPU { get; set; }
        public int TempoEspera { get; set; }
        public int QuantumRestante { get; set; }
        public int TabelaPaginasID { get; set; } = -1;
        public List<string> ArquivosAbertos { get; set; } = new();
        public List<int> ThreadsIDs { get; set; } = new();

        public PCB(int pid, string pidSimbolico, int prioridade, int tempoChegada)
        {
            PID = pid;
            PIDSimbolico = pidSimbolico;
            Estado = EEstadoProcesso.Novo;
            Prioridade = prioridade;
            ContadorPrograma = 0;
            TempoChegada = tempoChegada;
            TempoCPU = 0;
            TempoEspera = 0;
            QuantumRestante = 0;
        }

        public override string ToString() =>
            $"PID: {PID} ({PIDSimbolico}) | Estado: {Estado} | Prioridade: {Prioridade} | CPU: {TempoCPU}";
    }

    public class Processo
    {
        public PCB PCB { get; private set; }

        public Processo(int pid, string pidSimbolico, int prioridade, int tempoChegada)
        {
            PCB = new PCB(pid, pidSimbolico, prioridade, tempoChegada);
        }

        public void MudarEstado(EEstadoProcesso novoEstado) => PCB.Estado = novoEstado;

        public void ExecutarCiclo()
        {
            PCB.TempoCPU++;
            PCB.ContadorPrograma++;
        }

        public void AbrirArquivo(string caminho)
        {
            if (!PCB.ArquivosAbertos.Contains(caminho))
                PCB.ArquivosAbertos.Add(caminho);
        }

        public void FecharArquivo(string caminho) => PCB.ArquivosAbertos.Remove(caminho);
    }
}
