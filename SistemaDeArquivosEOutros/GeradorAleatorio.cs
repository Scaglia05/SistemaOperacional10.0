using System;

namespace SistemaOperacional10._0.SistemaDeArquivosEOutros
{
    public static class Randomizador
    {
        private static Random? _rnd;

        public static void Inicializar(int semente)
        {
            _rnd = new Random(semente);
        }

        private static Random ObterRandom()
        {
            if (_rnd == null)
                _rnd = new Random();
            return _rnd;
        }

        public static int IntEntre(int minimo, int maximo) => ObterRandom().Next(minimo, maximo);

        public static double DoubleAleatorio() => ObterRandom().NextDouble();
    }

    public static class GeradorIdentificadores
    {
        private static int _pidAtual = 1;
        private static int _tidAtual = 1;
        private static int _tabelaAtual = 1;

        public static int NovoPID() => _pidAtual++;
        public static int NovoTID() => _tidAtual++;
        public static int NovaTabelaID() => _tabelaAtual++;

        public static void ResetarContadores()
        {
            _pidAtual = 1;
            _tidAtual = 1;
            _tabelaAtual = 1;
        }
    }

}
