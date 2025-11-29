using SimuladorSO.Enum;
using SimuladorSO.Nucleo;
using SimuladorSO.Nucleo.Interface;

public class GerenciadorES
{
    private Kernel _kernel;
    private Dictionary<string, IDispositivo> _dispositivos;
    private Dictionary<string, Queue<RequisicaoES>> _filasDispositivos;
    private List<Interrupcao> _interrupcoes;

    public GerenciadorES(Kernel kernel)
    {
        _kernel = kernel;
        _dispositivos = new Dictionary<string, IDispositivo>();
        _filasDispositivos = new Dictionary<string, Queue<RequisicaoES>>();
        _interrupcoes = new List<Interrupcao>();

        // Criar dispositivos padrão
        CriarDispositivo("DISCO", 30);
        CriarDispositivo("TECLADO", 10);
        CriarDispositivo("IMPRESSORA", 40);
    }

    public void CriarDispositivo(string nome, int tempoOperacao)
    {
        var dispositivo = new Dispositivo(nome, tempoOperacao);
        _dispositivos[nome] = dispositivo;
        _filasDispositivos[nome] = new Queue<RequisicaoES>();

        _kernel.RegistrarEvento($"Novo dispositivo registrado: {nome}");
    }

    public void CriarRequisicao(string pidSimbolico, string nomeDispositivo, int tempo, bool bloqueante = true)
    {
        if (!_dispositivos.ContainsKey(nomeDispositivo))
        {
            Console.WriteLine($"Aviso: dispositivo {nomeDispositivo} não existe.");
            return;
        }

        var processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);
        if (processo == null)
        {
            Console.WriteLine($"Erro: processo {pidSimbolico} não localizado.");
            return;
        }

        var requisicao = new RequisicaoES(pidSimbolico, nomeDispositivo, tempo, bloqueante);
        _filasDispositivos[nomeDispositivo].Enqueue(requisicao);

        if (bloqueante)
            processo.MudarEstado(EEstadoProcesso.Bloqueado);

        _kernel.RegistrarEvento(
            $"Solicitação de I/O criada: {pidSimbolico} -> {nomeDispositivo} ({tempo} ticks)"
        );
    }

    public void ProcessarTick()
    {
        foreach (var dispositivo in _dispositivos.Values)
        {
            if (dispositivo.Ocupado)
            {
                dispositivo.ProcessarTick();
                var requisicaoFinalizada = dispositivo.FinalizarOperacao();

                if (requisicaoFinalizada != null)
                {
                    requisicaoFinalizada.TempoFim = _kernel.Clock.TempoAtual;
                    GerarInterrupcao("I/O", dispositivo.Nome,
                        $"Finalizada operação do processo {requisicaoFinalizada.PIDProcesso}");

                    if (requisicaoFinalizada.Bloqueante)
                    {
                        var processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(
                            requisicaoFinalizada.PIDProcesso);
                        if (processo != null && processo.PCB.Estado == EEstadoProcesso.Bloqueado)
                            processo.MudarEstado(EEstadoProcesso.Pronto);
                    }
                }
            } else if (_filasDispositivos[dispositivo.Nome].Count > 0)
            {
                var proximaRequisicao = _filasDispositivos[dispositivo.Nome].Dequeue();
                proximaRequisicao.TempoInicio = _kernel.Clock.TempoAtual;
                dispositivo.IniciarOperacao(proximaRequisicao);

                _kernel.RegistrarEvento(
                    $"Início de operação I/O: {proximaRequisicao.PIDProcesso} em {dispositivo.Nome}"
                );
            }
        }
    }

    public void ProcessarTicks(int quantidade)
    {
        for (int i = 0; i < quantidade; i++)
        {
            ProcessarTick();
            _kernel.Clock.Tick();
        }
    }

    private void GerarInterrupcao(string tipo, string origem, string mensagem)
    {
        var interrupcao = new Interrupcao(tipo, origem, mensagem, _kernel.Clock.TempoAtual);
        _interrupcoes.Add(interrupcao);
        _kernel.RegistrarEvento($"Interrupção registrada: {mensagem}");
    }

    public void ListarDispositivos()
    {
        Console.WriteLine("\n>>> STATUS DOS DISPOSITIVOS <<<");
        foreach (var dispositivo in _dispositivos.Values)
        {
            string status = dispositivo.Ocupado ? "EM USO" : "DISPONÍVEL";
            int filaSize = _filasDispositivos[dispositivo.Nome].Count;
            Console.WriteLine($"Dispositivo: {dispositivo.Nome} | Status: {status} | Fila: {filaSize}");
        }
        Console.WriteLine(">>> FIM DO STATUS <<<\n");
    }

    public void MostrarFilasDispositivos()
    {
        Console.WriteLine("\n>>> FILAS DE DISPOSITIVOS <<<");
        foreach (var kvp in _filasDispositivos)
        {
            Console.WriteLine($"\nDispositivo: {kvp.Key}");
            if (kvp.Value.Count == 0)
                Console.WriteLine("  Nenhuma requisição pendente.");
            else
                foreach (var req in kvp.Value)
                    Console.WriteLine($"  {req}");
        }
        Console.WriteLine(">>> FIM DAS FILAS <<<\n");
    }

    public void MostrarInterrupcoes()
    {
        Console.WriteLine("\n>>> INTERRUPÇÕES REGISTRADAS <<<");
        if (_interrupcoes.Count == 0)
            Console.WriteLine("Sem interrupções no momento.");
        else
            foreach (var i in _interrupcoes)
                Console.WriteLine($"  {i}");
        Console.WriteLine(">>> FIM DAS INTERRUPÇÕES <<<\n");
    }
}

public class Interrupcao
{
    public string Tipo { get; set; }
    public string Origem { get; set; }
    public string Mensagem { get; set; }
    public int Tempo { get; set; }

    public Interrupcao(string tipo, string origem, string mensagem, int tempo)
    {
        Tipo = tipo;
        Origem = origem;
        Mensagem = mensagem;
        Tempo = tempo;
    }

    public override string ToString() => $"[T={Tempo}] {Tipo} de {Origem}: {Mensagem}";
}

public class RequisicaoES
{
    public string PIDProcesso { get; set; }
    public string NomeDispositivo { get; set; }
    public int TempoRequisitado { get; set; }
    public int TempoRestante { get; set; }
    public bool Bloqueante { get; set; }
    public int TempoInicio { get; set; }
    public int TempoFim { get; set; }

    public RequisicaoES(string pidProcesso, string nomeDispositivo, int tempoRequisitado, bool bloqueante)
    {
        PIDProcesso = pidProcesso;
        NomeDispositivo = nomeDispositivo;
        TempoRequisitado = tempoRequisitado;
        TempoRestante = tempoRequisitado;
        Bloqueante = bloqueante;
        TempoInicio = -1;
        TempoFim = -1;
    }

    public override string ToString() => $"Req: {PIDProcesso} -> {NomeDispositivo} ({TempoRestante}/{TempoRequisitado} ticks)";
}
