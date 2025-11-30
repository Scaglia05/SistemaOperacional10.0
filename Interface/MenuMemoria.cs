using System;
using System.Collections.Generic;
using SistemaOperacional10._0.Interface;
using SistemaOperacional10._0.Nucleo;

public class MenuMemoria : MenuBase
{
    private List<OpcaoMenu> _opcoes;

    public MenuMemoria(Kernel kernel) : base(kernel)
    {
        InicializarOpcoes();
    }

    private void InicializarOpcoes()
    {
        _opcoes = new List<OpcaoMenu>
        {
            new OpcaoMenu("📄", "Tabela de Páginas/Segmentos", () => TelaMostrarPaginas()),
            new OpcaoMenu("🟢", "Alocar Memória",              () => TelaAlocar()),
            new OpcaoMenu("🔴", "Liberar Memória",             () => TelaLiberar()),
            new OpcaoMenu("🗂️", "Mapa de Molduras (Frames)",    () => TelaMostrarMapa()),
            new OpcaoMenu("🔄", "Alternar TLB (On/Off)",       () => TelaAlternarTLB()),
            new OpcaoMenu("📊", "Estatísticas da TLB",         () => TelaEstatisticasTLB()),
            new OpcaoMenu("⬅️", "Voltar ao Menu Principal",    () => { })
        };
    }

    public void Executar()
    {
        string asciiArt = @"
   __  __  _____  __  __   ___   ____  ___    _    
  |  \/  || ____||  \/  | / _ \ |  _ \|_ _|  / \   
  | |\/| ||  _|  | |\/| || | | || |_) || |  / _ \  
  | |  | || |___ | |  | || |_| ||  _ < | | / ___ \ 
  |_|  |_||_____||_|  |_| \___/ |_| \_\___/_/   \_\
        ";

        ExecutarMenu("Gerenciamento de Memória (MMU)", asciiArt, _opcoes);
    }

    private void TelaMostrarPaginas()
    {
        ExibirCabecalhoAcao("Inspeção de Memória Virtual");

        var pid = LerTexto("PID do Processo");
        if (!string.IsNullOrEmpty(pid))
        {
            Kernel.GerenciadorMemoria.MostrarTabelaPaginas(pid);
        } else
            MensagemErro("PID inválido.");

        Pausa();
    }

    private void TelaAlocar()
    {
        ExibirCabecalhoAcao("Alocação de Memória"); 

        var pid = LerTexto("PID do Processo");
        var quantidade = LerInt("Quantidade (bytes)");

        if (!string.IsNullOrEmpty(pid) && quantidade.HasValue && quantidade > 0)
        {
            try
            {
                Kernel.GerenciadorMemoria.AlocarMemoria(pid, quantidade.Value);
                MensagemSucesso($"Alocados {quantidade.Value} bytes para o processo {pid}.");
            } catch (Exception ex)
            {
                MensagemErro($"Falha na alocação: {ex.Message}");
            }
        } else
            MensagemErro("Dados inválidos.");

        Pausa();
    }

    private void TelaLiberar()
    {
        ExibirCabecalhoAcao("Desalocação de Memória"); 

        var pid = LerTexto("PID do Processo");

        if (!string.IsNullOrEmpty(pid))
        {
            Kernel.GerenciadorMemoria.LiberarMemoria(pid);
            MensagemSucesso($"Memória do processo {pid} liberada.");
        } else
            MensagemErro("PID inválido.");

        Pausa();
    }

    private void TelaMostrarMapa()
    {
        ExibirCabecalhoAcao("Mapa de Memória Física");
        Kernel.GerenciadorMemoria.MostrarMapaMolduras();
        Pausa();
    }

    private void TelaAlternarTLB()
    {
        ExibirCabecalhoAcao("Configuração da TLB"); 

        Kernel.Configuracoes.TLBAtivada = !Kernel.Configuracoes.TLBAtivada;

        string status = Kernel.Configuracoes.TLBAtivada ? "ATIVADA" : "DESATIVADA";

        MensagemSucesso($"TLB (Translation Lookaside Buffer) agora está: {status}");
        Pausa();
    }

    private void TelaEstatisticasTLB()
    {
        ExibirCabecalhoAcao("Performance da TLB");
        Kernel.GerenciadorMemoria.MostrarEstatisticasTLB();
        Pausa();
    }
}