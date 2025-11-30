using System.Collections.Generic;

namespace SistemaOperacional10._0.SistemaDeArquivosEOutros
{
    public class EntradaDiretorio
    {
        public string Nome { get; set; }
        public Dictionary<string, EntradaArquivo> Arquivos { get; } = new();
        public Dictionary<string, EntradaDiretorio> Subdiretorios { get; } = new();

        public EntradaDiretorio(string nome) => Nome = nome;

        public override string ToString() => $"[DIR] {Nome}/";
    }

    public class EntradaArquivo
    {
        public string Nome { get; set; }
        public int Tamanho { get; set; } = 0;
        public int TempoModificacao { get; set; } = 0;
        public string Conteudo { get; set; } = "";
        public bool Aberto { get; set; } = false;

        public EntradaArquivo(string nome) => Nome = nome;

        public override string ToString() => $"[ARQ] {Nome} ({Tamanho} bytes)";
    }

    public class FCB
    {
        public string Caminho { get; set; }
        public string PIDProcesso { get; set; }
        public int ModoAbertura { get; set; } // 0=leitura, 1=escrita, 2=ambos
        public int PosicaoAtual { get; set; } = 0;

        public FCB(string caminho, string pidProcesso, int modoAbertura)
        {
            Caminho = caminho;
            PIDProcesso = pidProcesso;
            ModoAbertura = modoAbertura;
        }

        public override string ToString()
        {
            string modo = ModoAbertura switch
            {
                0 => "R",
                1 => "W",
                _ => "RW"
            };
            return $"{Caminho} ({modo}) - Processo: {PIDProcesso}";
        }
    }
}
