using SimuladorSO.Nucleo;
using SimuladorSO.Processos;

namespace SimuladorSO.Nucleo.Interface
{
    public interface IEscalonamento
    {
        Processo? SelecionarProximoProcesso(FilaProntos fila);
        string Nome { get; }
    }
}
