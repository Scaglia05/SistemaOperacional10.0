using SistemaOperacional10._0.Nucleo;
using SistemaOperacional10._0.Processos;

namespace SistemaOperacional10._0.Nucleo.Interface
{
    public interface IEscalonamento
    {
        Processo? SelecionarProximoProcesso(FilaProntos fila);
        string Nome { get; }
    }
}
