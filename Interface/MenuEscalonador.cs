using System;
using System.Collections.Generic;
using SistemaOperacional10._0.Interface;
using SistemaOperacional10._0.Nucleo;

public class MenuEscalonador : MenuBase
{
    private List<OpcaoMenu> _opcoes;

    public MenuEscalonador(Kernel kernel) : base(kernel)
    {
        InicializarOpcoes();
    }

    private void InicializarOpcoes()
    {
        _opcoes = new List<OpcaoMenu>
        {
            new OpcaoMenu("🔄", "Alterar Algoritmo",          () => TelaTrocarAlgoritmo()),
            new OpcaoMenu("⏱️", "Ajustar Quantum",            () => TelaDefinirQuantum()),
            new OpcaoMenu("▶️", "Executar 1 Ciclo (Step)",    () => TelaExecutarCiclo()),
            new OpcaoMenu("⏩", "Executar Tudo (Run All)",    () => TelaExecutarAteFim()),
            new OpcaoMenu("📋", "Fila de Prontos",            () => TelaMostrarFilaProntos()),
            new OpcaoMenu("📊", "Estatísticas (Contexto)",    () => TelaExibirTrocasContexto()),
            new OpcaoMenu("⬅️", "Voltar ao Menu Principal",   () => { /* Controlado pelo MenuBase */ })
        };
    }

    public void Executar()
    {
        string asciiArt = @"
   ____ ____  _   _ 
  / ___|  _ \| | | |
 | |   | |_) | | | |
 | |___|  __/| |_| |
  \____|_|    \___/ 
        ";

        ExecutarMenu("Gerenciador de Processos (CPU)", asciiArt, _opcoes);
    }

    private void TelaTrocarAlgoritmo()
    {
        ExibirCabecalhoAcao("Seleção de Algoritmo");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("  Algoritmos disponíveis:");
        Console.WriteLine("  [A] FCFS (First-Come, First-Served)");
        Console.WriteLine("  [B] Round Robin (RR)");
        Console.WriteLine("  [C] Prioridade (Preemptivo)");
        Console.WriteLine("  [D] Prioridade (Não Preemptivo)");
        Console.ResetColor();
        Console.WriteLine();

        // Adicionei '?' e '?? ""' para evitar crash se o usuário der Enter vazio
        var op = LerTexto("Opção desejada")?.ToUpper() ?? "";

        string algoritmo = op switch
        {
            "A" => "FCFS",
            "B" => "RR",
            "C" => "PRIORIDADE_PREEMPTIVO",
            "D" => "PRIORIDADE_NAO_PREEMPTIVO",
            _ => ""
        };

        if (!string.IsNullOrEmpty(algoritmo))
        {
            Kernel.Escalonador.TrocarAlgoritmo(algoritmo);
            MensagemSucesso($"Algoritmo definido para: {algoritmo}");
        } else
        {
            MensagemErro("Opção inválida.");
        }
        Pausa();
    }

    private void TelaDefinirQuantum()
    {
        ExibirCabecalhoAcao("Configuração de Time Slice");

        var q = LerInt("Novo Quantum (ticks)");

        if (q.HasValue && q > 0)
        {
            // --- CORREÇÃO AQUI ---
            // Antes: Kernel.Quantum = q.Value;
            Kernel.Configuracoes.Quantum = q.Value;

            MensagemSucesso($"Quantum atualizado para {q.Value} ticks.");
        } else
        {
            MensagemErro("Valor deve ser maior que zero.");
        }
        Pausa();
    }

    private void TelaExecutarCiclo()
    {
        ExibirCabecalhoAcao("Execução Passo a Passo");
        Kernel.Escalonador.ExecutarCiclo();
        MensagemSucesso("Ciclo de CPU executado.");
        Pausa();
    }

    private void TelaExecutarAteFim()
    {
        ExibirCabecalhoAcao("Execução em Lote");
        Console.WriteLine("  Processando todos os processos ativos...");

        Kernel.Escalonador.ExecutarAteFinalizarTodos();

        MensagemSucesso("Fila de prontos vazia. Todos finalizados.");
        Pausa();
    }

    private void TelaMostrarFilaProntos()
    {
        ExibirCabecalhoAcao("Estado Atual da Fila");
        Kernel.Escalonador.MostrarFilaProntos();
        Pausa();
    }

    private void TelaExibirTrocasContexto()
    {
        ExibirCabecalhoAcao("Métricas de Desempenho");

        Console.WriteLine($"  Total de Trocas:      {Kernel.Escalonador.TrocaContexto.ContadorTrocas}");
        Console.WriteLine($"  Sobrecarga (Overhead): {Kernel.Escalonador.TrocaContexto.SobrecargaTotal} ticks");

        Pausa();
    }
}