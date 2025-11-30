using SistemaOperacional10._0.Enum;
using SistemaOperacional10._0.Nucleo.Interface;
using SistemaOperacional10._0.Processos;

namespace SistemaOperacional10._0.Nucleo
{
    public class Escalonador
    {
        private Kernel _kernel;
        private FilaProntos _filaProntos;
        private IEscalonamento _algoritmo;
        private TrocaDeContexto _trocaContexto;
        private Processo? _processoAtual;

        public FilaProntos FilaProntos => _filaProntos;
        public TrocaDeContexto TrocaContexto => _trocaContexto;
        public Processo? ProcessoAtual => _processoAtual;

        public Escalonador(Kernel kernel)
        {
            _kernel = kernel;
            _filaProntos = new FilaProntos();
            _algoritmo = new RoundRobin(); // padrão
            _trocaContexto = new TrocaDeContexto();
        }

        public void TrocarAlgoritmo(string nomeAlgoritmo)
        {
            _algoritmo = nomeAlgoritmo.ToUpper() switch
            {
                "FCFS" => new FCFS(),
                "RR" => new RoundRobin(),
                "PRIORIDADE_PREEMPTIVO" => new PrioridadePreemptivo(),
                "PRIORIDADE_NAO_PREEMPTIVO" => new PrioridadeNaoPreemptivo(),
                _ => _algoritmo
            };

            _kernel.RegistrarLog($"Algoritmo de escalonamento agora: {_algoritmo.Nome}");
        }

        public void AdicionarProcessoNaFila(Processo processo)
        {
            if (processo.PCB.Estado == EEstadoProcesso.Pronto)
                _filaProntos.Adicionar(processo);
        }

        public void ExecutarCiclo()
        {
            // Adiciona todos os processos prontos à fila
            foreach (var p in _kernel.GerenciadorProcessos.ListarProcessosPorEstado(EEstadoProcesso.Pronto))
                AdicionarProcessoNaFila(p);

            // Seleciona próximo processo se necessário
            if (_processoAtual == null || _processoAtual.PCB.Estado != EEstadoProcesso.Executando)
                SelecionarProximoProcesso();

            if (_processoAtual != null && _processoAtual.PCB.Estado == EEstadoProcesso.Executando)
            {
                _processoAtual.ExecutarCiclo();

                if (_processoAtual.PCB.TempoInicio == -1)
                    _processoAtual.PCB.TempoInicio = _kernel.Clock.TempoAtual;

                _kernel.Clock.Tick();

                // Round Robin
                if (_algoritmo is RoundRobin)
                {
                    _processoAtual.PCB.QuantumRestante--;
                    if (_processoAtual.PCB.QuantumRestante <= 0)
                    {
                        _processoAtual.MudarEstado(EEstadoProcesso.Pronto);
                        _filaProntos.Adicionar(_processoAtual);
                        _processoAtual = null;
                    }
                }

                // Prioridade preemptiva
                if (_algoritmo is PrioridadePreemptivo)
                {
                    var candidato = _algoritmo.SelecionarProximoProcesso(_filaProntos);
                    if (candidato != null && candidato.PCB.Prioridade < _processoAtual.PCB.Prioridade)
                    {
                        _processoAtual.MudarEstado(EEstadoProcesso.Pronto);
                        _filaProntos.Adicionar(_processoAtual);
                        _processoAtual = null;
                    }
                }
            } else
            {
                _kernel.Clock.Tick();
            }
        }

        public void ExecutarCiclos(int quantidade)
        {
            for (int i = 0; i < quantidade; i++)
                ExecutarCiclo();
        }

        public void ExecutarAteFinalizarTodos()
        {
            const int maxIteracoes = 10000;
            int iteracoes = 0;

            while (iteracoes < maxIteracoes)
            {
                List<Processo> todosProcessos = _kernel.GerenciadorProcessos.ListarProcessos();
                var ativos = todosProcessos
                    .Where(p => p.PCB.Estado != EEstadoProcesso.Finalizado)
                    .ToList();

                if (ativos.Count == 0)
                    break;

                ExecutarCiclo();
                iteracoes++;
            }

            if (iteracoes >= maxIteracoes)
                Console.WriteLine("Atenção: limite de iterações atingido.");
        }

        private void SelecionarProximoProcesso()
        {
            var proximo = _algoritmo.SelecionarProximoProcesso(_filaProntos);

            if (proximo != null)
            {
                _filaProntos.Remover(proximo);

                if (_processoAtual != null && _processoAtual.PCB.PID != proximo.PCB.PID)
                    _trocaContexto.RegistrarTroca();

                _processoAtual = proximo;
                _processoAtual.MudarEstado(EEstadoProcesso.Executando);
                _processoAtual.PCB.QuantumRestante = _kernel.Configuracoes.Quantum;

                _kernel.RegistrarLog(
                    $"Processo selecionado: PID={_processoAtual.PCB.PID} ({_processoAtual.PCB.PIDSimbolico})"
                );
            }
        }

        public void LimparProcessoAtual() => _processoAtual = null;

        public void MostrarFilaProntos()
        {
            Console.WriteLine("\n>>> FILA DE PRONTOS <<<");
            var processos = _filaProntos.ObterTodos();
            if (processos.Count == 0)
                Console.WriteLine("Fila vazia.");
            else
                processos.ForEach(p => Console.WriteLine(p.PCB.ToString()));
            Console.WriteLine(">>> FIM DA FILA <<<\n");
        }
    }

    public class FilaProntos
    {
        private List<Processo> _fila = new List<Processo>();

        public void Adicionar(Processo p)
        {
            if (!_fila.Contains(p))
                _fila.Add(p);
        }

        public void Remover(Processo p) => _fila.Remove(p);

        public Processo? ObterPrimeiro() => _fila.Count > 0 ? _fila[0] : null;

        public List<Processo> ObterTodos() => new List<Processo>(_fila);

        public bool EstaVazia() => _fila.Count == 0;
    }

    public class TrocaDeContexto
    {
        private int _contadorTrocas = 0;
        private int _sobrecargaTotal = 0;
        private const int CUSTO_TROCA = 1;

        public int ContadorTrocas => _contadorTrocas;
        public int SobrecargaTotal => _sobrecargaTotal;

        public void RegistrarTroca()
        {
            _contadorTrocas++;
            _sobrecargaTotal += CUSTO_TROCA;
        }

        public void Resetar()
        {
            _contadorTrocas = 0;
            _sobrecargaTotal = 0;
        }
    }

    public class FCFS : IEscalonamento
    {
        public string Nome => "FCFS (First-Come, First-Served)";
        public Processo? SelecionarProximoProcesso(FilaProntos fila) => fila.ObterPrimeiro();
    }

    public class RoundRobin : IEscalonamento
    {
        public string Nome => "Round Robin";
        public Processo? SelecionarProximoProcesso(FilaProntos fila) => fila.ObterPrimeiro();
    }

    public class PrioridadeNaoPreemptivo : IEscalonamento
    {
        public string Nome => "Prioridade (Não Preemptivo)";
        public Processo? SelecionarProximoProcesso(FilaProntos fila)
        {
            return fila.ObterTodos()
                       .OrderBy(p => p.PCB.Prioridade)
                       .ThenBy(p => p.PCB.TempoChegada)
                       .FirstOrDefault();
        }
    }

    public class PrioridadePreemptivo : IEscalonamento
    {
        public string Nome => "Prioridade (Preemptivo)";
        public Processo? SelecionarProximoProcesso(FilaProntos fila)
        {
            return fila.ObterTodos()
                       .OrderBy(p => p.PCB.Prioridade)
                       .ThenBy(p => p.PCB.TempoChegada)
                       .FirstOrDefault();
        }
    }
}
