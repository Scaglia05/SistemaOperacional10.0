namespace SistemaOperacional10._0.Nucleo.Interface
{
    public interface IDispositivo
    {
        string Nome { get; }
        int TempoOperacao { get; set; }
        bool Ocupado { get; }
        void IniciarOperacao(RequisicaoES requisicao);
        void ProcessarTick();
        RequisicaoES? FinalizarOperacao();
    }
}
