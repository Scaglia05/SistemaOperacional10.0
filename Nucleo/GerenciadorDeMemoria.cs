using SimuladorSO.SistemaDeArquivosEOutros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimuladorSO.Nucleo
{
    public class GerenciadorDeMemoria
    {
        private readonly Kernel _kernel;
        private readonly TabelaDeMolduras _tabelaMolduras;
        private readonly Dictionary<string, TabelaDePaginas> _tabelasPaginas = new();
        private readonly TLB _tlb;
        private int _faltasPagina;
        public int FaltasPagina => _faltasPagina;
        public TLB TLB => _tlb;

        public GerenciadorDeMemoria(Kernel kernel)
        {
            _kernel = kernel;
            _tabelaMolduras = new TabelaDeMolduras(_kernel.NumeroMolduras);
            _tlb = new TLB(_kernel.TamanhoTLB);
        }

        public void AlocarMemoria(string pid, int tamanhoBytes)
        {
            var processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pid);
            if (processo == null)
            {
                Console.WriteLine($"Processo {pid} não encontrado.");
                return;
            }

            int tamanhoPagina = _kernel.TamanhoPagina;
            int numeroPaginas = (int)Math.Ceiling(tamanhoBytes / (double)tamanhoPagina);

            if (!_tabelasPaginas.ContainsKey(pid))
            {
                int tabelaID = GeradorIdentificadores.NovaTabelaID();
                _tabelasPaginas[pid] = new TabelaDePaginas(tabelaID, pid);
                processo.PCB.TabelaPaginasID = tabelaID;
            }

            _kernel.RegistrarEvento(
                $"Memória alocada para {pid}: {tamanhoBytes} bytes ({numeroPaginas} páginas)"
            );
        }

        public void LiberarMemoria(string pid)
        {
            if (!_tabelasPaginas.ContainsKey(pid))
                return;

            _tabelaMolduras.LiberarProcesso(pid);
            _tabelasPaginas.Remove(pid);
            _tlb.LimparProcesso(pid);

            _kernel.RegistrarEvento($"Memória liberada para {pid}");
        }

        public void AcessarMemoria(string pid, int enderecoLogico)
        {
            if (!_tabelasPaginas.ContainsKey(pid))
            {
                Console.WriteLine($"Processo {pid} não possui tabela de páginas.");
                return;
            }

            int tamanhoPagina = _kernel.TamanhoPagina;
            int numeroPagina = enderecoLogico / tamanhoPagina;
            int deslocamento = enderecoLogico % tamanhoPagina;
            int? moldura = null;

            if (_kernel.TLBAtivada)
                moldura = _tlb.Consultar(pid, numeroPagina);

            if (moldura == null)
            {
                var tabela = _tabelasPaginas[pid];
                var pagina = tabela.ObterOuCriarPagina(numeroPagina);

                if (!pagina.Presente)
                {
                    _faltasPagina++;
                    _kernel.RegistrarEvento($"Falta de página: {pid} - Página {numeroPagina}");

                    var molduraAlocada = _tabelaMolduras.Alocar(pid, numeroPagina, Enum.Enum.FirstFit);
                    if (molduraAlocada == null)
                    {
                        Console.WriteLine("Erro: Sem molduras livres disponíveis!");
                        return;
                    }

                    pagina.Moldura = molduraAlocada.Numero;
                    pagina.Presente = true;
                    moldura = molduraAlocada.Numero;
                } else
                {
                    moldura = pagina.Moldura;
                }

                if (_kernel.TLBAtivada)
                    _tlb.Inserir(pid, numeroPagina, moldura.Value, _kernel.Clock.TempoAtual);

                pagina.Referenciada = true;
                pagina.TempoAcesso = _kernel.Clock.TempoAtual;
            }

            int enderecoFisico = moldura.Value * tamanhoPagina + deslocamento;
            _kernel.RegistrarEvento(
                $"Acesso à memória: {pid} - End. Lógico: 0x{enderecoLogico:X4} -> End. Físico: 0x{enderecoFisico:X4}"
            );
        }

        public void MostrarTabelaPaginas(string pid)
        {
            if (!_tabelasPaginas.ContainsKey(pid))
            {
                Console.WriteLine($"Processo {pid} não possui tabela de páginas.");
                return;
            }

            Console.WriteLine($"\n===== TABELA DE PÁGINAS - {pid} =====");
            foreach (var pagina in _tabelasPaginas[pid].Paginas.Values)
                Console.WriteLine(pagina);
            Console.WriteLine("====================================\n");
        }

        public void MostrarMapaMolduras()
        {
            Console.WriteLine("\n===== MAPA DE MOLDURAS =====");
            foreach (var moldura in _tabelaMolduras.ObterTodas())
                Console.WriteLine(moldura);
            Console.WriteLine($"\nMolduras livres: {_tabelaMolduras.ContarLivres()}");
            Console.WriteLine("============================\n");
        }

        public void MostrarEstatisticasTLB()
        {
            Console.WriteLine("\n===== ESTATÍSTICAS TLB =====");
            Console.WriteLine($"Hits: {_tlb.Hits}");
            Console.WriteLine($"Misses: {_tlb.Misses}");
            Console.WriteLine($"Taxa de Acerto: {_tlb.TaxaAcerto():F2}%");
            Console.WriteLine("============================\n");
        }
    }
}
