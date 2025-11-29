using System;
using System.Collections.Generic;

namespace SimuladorSO.Nucleo
{
    public class Pagina
    {
        public int Numero { get; }
        public int Moldura { get; set; } = -1;
        public bool Presente { get; set; } = false;
        public bool Modificada { get; set; } = false;
        public bool Referenciada { get; set; } = false;
        public int TempoAcesso { get; set; } = 0;
        public Pagina(int numero) => Numero = numero;

        public override string ToString() => $"Pág {Numero} -> Moldura {Moldura} | Presente: {Presente}";
    }

    public class TabelaDePaginas
    {
        public int ID { get; }
        public string PID { get; }
        private readonly Dictionary<int, Pagina> _paginas = new();

        public TabelaDePaginas(int id, string pid)
        {
            ID = id;
            PID = pid;
        }

        public Pagina ObterOuCriarPagina(int numero)
        {
            if (!_paginas.ContainsKey(numero))
                _paginas[numero] = new Pagina(numero);
            return _paginas[numero];
        }

        public Pagina? ObterPagina(int numero) => _paginas.TryGetValue(numero, out var pagina) ? pagina : null;

        public IReadOnlyDictionary<int, Pagina> Paginas => _paginas;
    }

}
