using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaOperacional10._0.Nucleo
{
    public class TLB
    {
        private readonly List<EntradaTLB> _entradas = new();
        private readonly int _capacidade;
        private int _hits;
        private int _misses;
        public int Hits => _hits;
        public int Misses => _misses;

        public TLB(int capacidade)
        {
            _capacidade = capacidade;
        }

        public int? Consultar(string pid, int pagina)
        {
            var entrada = _entradas.FirstOrDefault(e => e.PID == pid && e.Pagina == pagina);
            if (entrada != null)
            {
                _hits++;
                return entrada.Moldura;
            }
            _misses++;
            return null;
        }

        public void Inserir(string pid, int pagina, int moldura, int tempoAtual)
        {
            // Remove duplicata
            _entradas.RemoveAll(e => e.PID == pid && e.Pagina == pagina);

            // Se cheio, remove o mais antigo (LRU)
            if (_entradas.Count >= _capacidade)
            {
                var lru = _entradas.OrderBy(e => e.TempoAcesso).First();
                _entradas.Remove(lru);
            }

            _entradas.Add(new EntradaTLB(pid, pagina, moldura, tempoAtual));
        }

        public void LimparTudo() => _entradas.Clear();

        public void LimparProcesso(string pid) => _entradas.RemoveAll(e => e.PID == pid);

        public double TaxaAcerto() => _hits + _misses > 0 ? _hits / (double)(_hits + _misses) * 100 : 0;

        public IReadOnlyList<EntradaTLB> Entradas => _entradas.AsReadOnly();
    }

    public class EntradaTLB
    {
        public string PID { get; }
        public int Pagina { get; }
        public int Moldura { get; }
        public int TempoAcesso { get; }

        public EntradaTLB(string pid, int pagina, int moldura, int tempoAcesso)
        {
            PID = pid;
            Pagina = pagina;
            Moldura = moldura;
            TempoAcesso = tempoAcesso;
        }

        public override string ToString() => $"[{PID}] Pág {Pagina} → Moldura {Moldura}";
    }
}
