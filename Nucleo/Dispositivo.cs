using SistemaOperacional10._0.Nucleo.Interface;

namespace SistemaOperacional10._0.Nucleo
{
    public class Dispositivo : IDispositivo
    {
        public string Nome { get; private set; }
        public int TempoOperacao { get; set; }
        public bool Ocupado { get; private set; }
        private RequisicaoES? _requisicaoAtual;

        public Dispositivo(string nome, int tempoOperacao)
        {
            Nome = nome;
            TempoOperacao = tempoOperacao;
            Ocupado = false;
        }

        public void IniciarOperacao(RequisicaoES requisicao)
        {
            _requisicaoAtual = requisicao;
            Ocupado = true;
        }

        public void ProcessarTick()
        {
            if (_requisicaoAtual != null && Ocupado)
                _requisicaoAtual.TempoRestante--;
        }

        public RequisicaoES? FinalizarOperacao()
        {
            if (_requisicaoAtual != null && _requisicaoAtual.TempoRestante <= 0)
            {
                var requisicaoFinalizada = _requisicaoAtual;
                _requisicaoAtual = null;
                Ocupado = false;
                return requisicaoFinalizada;
            }
            return null;
        }
    }
}