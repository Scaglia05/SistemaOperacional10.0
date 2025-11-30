using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaOperacional10._0.Nucleo
{
    public class Moldura
    {
        public int Numero { get; }
        public bool Livre { get; private set; } = true;
        public string? PID { get; private set; } = null;
        public int Pagina { get; private set; } = -1;
        public Moldura(int numero) => Numero = numero;

        public void Alocar(string pid, int pagina)
        {
            Livre = false;
            PID = pid;
            Pagina = pagina;
        }

        public void Liberar()
        {
            Livre = true;
            PID = null;
            Pagina = -1;
        }

        public override string ToString() => Livre ? $"Moldura {Numero}: LIVRE" : $"Moldura {Numero}: {PID} (Pág {Pagina})";
    }

    public class TabelaDeMolduras
    {
        private readonly Dictionary<int, Moldura> _molduras;

        public TabelaDeMolduras(int quantidade)
        {
            _molduras = Enumerable.Range(0, quantidade).ToDictionary(i => i, i => new Moldura(i));
        }

        public Moldura? Alocar(string pid, int pagina, Enum.Enum politica)
        {
            // Apenas First Fit por enquanto
            var livre = _molduras.Values.FirstOrDefault(m => m.Livre);
            if (livre != null)
                livre.Alocar(pid, pagina);
            return livre;
        }

        public void Liberar(int numero)
        {
            if (_molduras.TryGetValue(numero, out var moldura))
                moldura.Liberar();
        }

        public void LiberarProcesso(string pid)
        {
            foreach (var moldura in _molduras.Values.Where(m => m.PID == pid))
                moldura.Liberar();
        }

        public IReadOnlyList<Moldura> ObterTodas() => _molduras.Values.ToList();

        public int ContarLivres() => _molduras.Values.Count(m => m.Livre);
    }
}
