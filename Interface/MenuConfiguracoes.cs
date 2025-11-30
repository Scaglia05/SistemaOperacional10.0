using SistemaOperacional10._0.Interface; // Ajuste para seu namespace real
using SistemaOperacional10._0.Nucleo;
using SistemaOperacional10._0.SistemaDeArquivosEOutros; // Onde está o Randomizador

public class MenuConfiguracoes : MenuBase
{
    private List<OpcaoMenu> _opcoes;

    public MenuConfiguracoes(Kernel kernel) : base(kernel)
    {
        InicializarOpcoes();
    }

    private void InicializarOpcoes()
    {
        _opcoes = new List<OpcaoMenu>
        {
            new OpcaoMenu("🎲", "Definir Semente (Seed)",       () => TelaConfigurarSemente()),
            new OpcaoMenu("📏", "Tamanho da Página",            () => TelaConfigurarTamanhoPagina()),
            new OpcaoMenu("🖼️", "Número de Molduras",           () => TelaConfigurarNumeroMolduras()),
            new OpcaoMenu("⏱️", "Tempos de Dispositivos (I/O)", () => TelaConfigurarTemposDispositivos()),
            new OpcaoMenu("📂", "Carregar Workload",            () => TelaCarregarWorkload()),
            new OpcaoMenu("⬅️", "Voltar ao Menu Principal",     () => { }) // MenuBase trata o retorno
        };
    }

    public void Executar()
    {
        string asciiArt = @"
   _____ ___  _   _ _____ ___ ____ 
  / ____/ _ \| \ | |  ___|_ _/ ___|
 | |   | | | |  \| | |_   | | |  _ 
 | |___| |_| | |\  |  _|  | | |_| |
  \_____\___/|_| \_|_|   |___\____|
        ";

        ExecutarMenu("Configurações do Sistema", asciiArt, _opcoes);
    }

    private void TelaConfigurarSemente()
    {
        ExibirCabecalhoAcao("Configuração de Randomização");

        var semente = LerInt("Nova semente (Seed)");

        if (semente.HasValue)
        {
            // CORREÇÃO AQUI: Acessando via .Configuracoes
            Kernel.Configuracoes.Seed = semente.Value;

            // Supondo que Randomizador seja uma classe estática utilitária
            Randomizador.Inicializar(semente.Value);

            MensagemSucesso($"Semente definida: {semente.Value}");
        } else
            MensagemErro("Valor inválido mantido.");

        Pausa();
    }

    private void TelaConfigurarTamanhoPagina()
    {
        ExibirCabecalhoAcao("Memória - Paginação");

        var tamanho = LerInt("Tamanho da página (bytes)");

        if (tamanho.HasValue && tamanho > 0)
        {
            // CORREÇÃO AQUI
            Kernel.Configuracoes.TamanhoPagina = tamanho.Value;
            MensagemSucesso($"Novo tamanho de página: {tamanho.Value} bytes");
        } else
            MensagemErro("Valor deve ser maior que zero.");

        Pausa();
    }

    private void TelaConfigurarNumeroMolduras()
    {
        ExibirCabecalhoAcao("Memória - Molduras (Frames)");

        var numero = LerInt("Quantidade de molduras");

        if (numero.HasValue && numero > 0)
        {
            // CORREÇÃO AQUI
            Kernel.Configuracoes.NumeroMolduras = numero.Value;
            MensagemSucesso($"Total de molduras: {numero.Value}");
        } else
            MensagemErro("Valor deve ser maior que zero.");

        Pausa();
    }

    private void TelaConfigurarTemposDispositivos()
    {
        ExibirCabecalhoAcao("Latência de I/O (Ticks)");

        Console.WriteLine("  Informe os novos tempos de espera:\n");

        // CORREÇÕES NOS ACESSOS ABAIXO
        var disco = LerInt("💽 DISCO");
        if (disco.HasValue)
            Kernel.Configuracoes.TempoDisco = disco.Value;

        var teclado = LerInt("⌨️ TECLADO");
        if (teclado.HasValue)
            Kernel.Configuracoes.TempoTeclado = teclado.Value;

        var impressora = LerInt("🖨️ IMPRESSORA");
        if (impressora.HasValue)
            Kernel.Configuracoes.TempoImpressora = impressora.Value;

        MensagemSucesso("Tempos de dispositivos atualizados!");
        Pausa();
    }

    private void TelaCarregarWorkload()
    {
        ExibirCabecalhoAcao("Carregar Carga de Trabalho");

        var caminho = LerTexto("Caminho do arquivo");

        if (!string.IsNullOrEmpty(caminho))
        {
            try
            {
                Kernel.CarregadorWorkload.CarregarArquivo(caminho);
                MensagemSucesso("Workload processado e carregado.");
            } catch (Exception ex)
            {
                MensagemErro($"Falha ao ler arquivo: {ex.Message}");
            }
        } else
            MensagemErro("Caminho inválido.");

        Pausa();
    }
}