using System;
using System.Collections.Generic;
using SistemaOperacional10._0.Interface;
using SistemaOperacional10._0.Nucleo;

public class MenuES : MenuBase
{
    private List<OpcaoMenu> _opcoes;

    public MenuES(Kernel kernel) : base(kernel)
    {
        InicializarOpcoes();
    }

    private void InicializarOpcoes()
    {
        _opcoes = new List<OpcaoMenu>
        {
            new OpcaoMenu("💻", "Listar Dispositivos",         () => TelaListarDispositivos()),
            new OpcaoMenu("⏳", "Nova Requisição (Bloqueante)",() => TelaCriarRequisicao(true)),
            new OpcaoMenu("⚡", "Nova Requisição (Async)",     () => TelaCriarRequisicao(false)),
            new OpcaoMenu("⏱️", "Processar 1 Tick de I/O",     () => TelaProcessarTick()),
            new OpcaoMenu("📋", "Visualizar Filas",            () => TelaMostrarFilas()),
            new OpcaoMenu("🛎️", "Visualizar Interrupções",     () => TelaMostrarInterrupcoes()),
            new OpcaoMenu("⬅️", "Voltar ao Menu Principal",    () => { /* Controlado pelo MenuBase */ })
        };
    }

    public void Executar()
    {
        string asciiArt = @"
   ___  ____   ___   _____  __  _   _ _____ 
  |_ _||  _ \ / _ \ |_   _|/ / | | | |_   _|
   | | | |_) | | | |  | | / /  | | | | | |  
   | | |  __/| |_| |  | |/ /   | |_| | | |  
  |___||_|    \___/   |_/_/     \___/  |_|  
        ";

        ExecutarMenu("Gerenciador de Entrada/Saída", asciiArt, _opcoes);
    }


    private void TelaListarDispositivos()
    {
        ExibirCabecalhoAcao("Dispositivos Conectados");
        Kernel.GerenciadorES.ListarDispositivos();
        Pausa();
    }

    private void TelaCriarRequisicao(bool bloqueante)
    {
        string tipo = bloqueante ? "BLOQUEANTE" : "NÃO BLOQUEANTE (ASYNC)";
        ExibirCabecalhoAcao($"Nova Requisição - {tipo}");

        var pid = LerTexto("PID do Processo");
        var disp = LerTexto("Dispositivo (Disco, Teclado, Impressora)")?.ToUpper();
        var tempo = LerInt("Duração (ticks)");

        if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(disp) && tempo.HasValue)
        {
            try
            {
                Kernel.GerenciadorES.CriarRequisicao(pid, disp, tempo.Value, bloqueante);
                MensagemSucesso("Requisição enviada ao controlador de I/O!");
            } catch (Exception ex)
            {
                MensagemErro($"Erro ao criar requisição: {ex.Message}");
            }
        } else
        {
            MensagemErro("Dados inválidos. Verifique o nome do dispositivo.");
        }
        Pausa();
    }

    private void TelaProcessarTick()
    {
        ExibirCabecalhoAcao("Simulação de Hardware");

        Kernel.GerenciadorES.ProcessarTick();

        MensagemSucesso("Ciclo de I/O processado.");
        Console.WriteLine("  Verifique as interrupções para ver se algum processo foi liberado.");
        Pausa();
    }

    private void TelaMostrarFilas()
    {
        ExibirCabecalhoAcao("Filas de Espera por Dispositivo");
        Kernel.GerenciadorES.MostrarFilasDispositivos();
        Pausa();
    }

    private void TelaMostrarInterrupcoes()
    {
        ExibirCabecalhoAcao("Buffer de Interrupções");
        Kernel.GerenciadorES.MostrarInterrupcoes();
        Pausa();
    }
}