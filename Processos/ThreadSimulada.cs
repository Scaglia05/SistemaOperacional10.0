using SimuladorSO.Enum;

namespace SimuladorSO.Processos
{
    public class ThreadSimulada
    {
        public TCB TCB { get; }

        public ThreadSimulada(int tid, int pidProcesso, int tempoChegada)
        {
            TCB = new TCB(tid, pidProcesso, tempoChegada);
        }

        public void MudarEstado(EstadoThread novoEstado) => TCB.Estado = novoEstado;

        public void ExecutarCiclo()
        {
            TCB.TempoCPU++;
            TCB.ContadorPrograma++;
        }
    }

    public class TCB
    {
        public int TID { get; set; }
        public int PIDProcesso { get; set; }
        public EstadoThread Estado { get; set; } = EstadoThread.Novo;
        public int ContadorPrograma { get; set; } = 0;
        public int TempoCPU { get; set; } = 0;
        public int TempoChegada { get; set; }

        public TCB(int tid, int pidProcesso, int tempoChegada)
        {
            TID = tid;
            PIDProcesso = pidProcesso;
            TempoChegada = tempoChegada;
        }

        public override string ToString() =>
            $"TID: {TID} | Processo: {PIDProcesso} | Estado: {Estado} | CPU: {TempoCPU}";
    }
}
