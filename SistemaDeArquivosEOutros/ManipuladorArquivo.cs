using SistemaOperacional10._0.Nucleo;

namespace SistemaOperacional10._0.SistemaDeArquivosEOutros
{
    public class ManipuladorArquivo
    {
        private readonly Dictionary<string, List<FCB>> _arquivosAbertos = new();
        public void AbrirArquivo(string caminho, string pidProcesso, int modoAbertura)
        {
            if (!_arquivosAbertos.ContainsKey(caminho))
                _arquivosAbertos[caminho] = new List<FCB>();

            _arquivosAbertos[caminho].Add(new FCB(caminho, pidProcesso, modoAbertura));
        }

        public void FecharArquivo(string caminho, string pidProcesso)
        {
            if (_arquivosAbertos.TryGetValue(caminho, out var lista))
            {
                lista.RemoveAll(fcb => fcb.PIDProcesso == pidProcesso);
                if (lista.Count == 0)
                    _arquivosAbertos.Remove(caminho);
            }
        }

        public bool EstaAberto(string caminho) => _arquivosAbertos.ContainsKey(caminho) && _arquivosAbertos[caminho].Count > 0;

        public List<FCB> ObterArquivosAbertos() => _arquivosAbertos.Values.SelectMany(l => l).ToList();
    }

    public class SistemaArquivos
    {
        private readonly Kernel _kernel;
        private readonly EntradaDiretorio _raiz;
        private EntradaDiretorio _diretorioAtual;
        private readonly ManipuladorArquivo _manipuladorArquivo = new();
        private readonly TabelaDeAlocacao _tabelaAlocacao = new();

        public SistemaArquivos(Kernel kernel)
        {
            _kernel = kernel;
            _raiz = new EntradaDiretorio("/");
            _diretorioAtual = _raiz;

            // Diretórios padrão
            _raiz.Subdiretorios["docs"] = new EntradaDiretorio("docs");
            _raiz.Subdiretorios["bin"] = new EntradaDiretorio("bin");
        }

        public void CriarArquivo(string caminho)
        {
            var diretorio = ObterDiretorio(caminho);
            string nomeArquivo = ObterNome(caminho);

            if (diretorio == null)
                return;

            if (!diretorio.Arquivos.ContainsKey(nomeArquivo))
            {
                diretorio.Arquivos[nomeArquivo] = new EntradaArquivo(nomeArquivo);
                _kernel.RegistrarLog($"Arquivo criado: {caminho}");
            } else
            {
                Console.WriteLine($"Arquivo {caminho} já existe.");
            }
        }

        public void CriarDiretorio(string caminho)
        {
            string nome = ObterNome(caminho);
            if (!_diretorioAtual.Subdiretorios.ContainsKey(nome))
            {
                _diretorioAtual.Subdiretorios[nome] = new EntradaDiretorio(nome);
                _kernel.RegistrarLog($"Diretório criado: {nome}");
            } else
                Console.WriteLine($"Diretório {nome} já existe.");
        }

        public void AbrirArquivo(string pidSimbolico, string caminho) => ManipularArquivo(pidSimbolico, caminho, true);

        public void FecharArquivo(string pidSimbolico, string caminho) => ManipularArquivo(pidSimbolico, caminho, false);

        private void ManipularArquivo(string pidSimbolico, string caminho, bool abrir)
        {
            var processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);
            if (processo == null)
            { Console.WriteLine($"Processo {pidSimbolico} não encontrado."); return; }

            var arquivo = ObterArquivo(caminho);
            if (arquivo == null)
            { Console.WriteLine($"Arquivo {caminho} não encontrado."); return; }

            if (abrir)
            {
                _manipuladorArquivo.AbrirArquivo(caminho, pidSimbolico, 2);
                arquivo.Aberto = true;
                processo.AbrirArquivo(caminho);
                _kernel.RegistrarLog($"Arquivo aberto: {caminho} por {pidSimbolico}");
            } else
            {
                _manipuladorArquivo.FecharArquivo(caminho, pidSimbolico);
                processo.FecharArquivo(caminho);
                if (!_manipuladorArquivo.EstaAberto(caminho))
                    arquivo.Aberto = false;
                _kernel.RegistrarLog($"Arquivo fechado: {caminho} por {pidSimbolico}");
            }
        }

        public void LerArquivo(string pidSimbolico, string caminho, int tamanho) => OperacaoArquivo(pidSimbolico, caminho, "Leitura", tamanho);

        public void EscreverArquivo(string pidSimbolico, string caminho, int tamanho)
        {
            var arquivo = ObterArquivo(caminho);
            if (arquivo == null)
            { Console.WriteLine($"Arquivo {caminho} não encontrado."); return; }

            arquivo.Tamanho = tamanho;
            arquivo.TempoModificacao = _kernel.Clock.TempoAtual;
            _tabelaAlocacao.AlocarBlocos(caminho, tamanho / 512 + 1);

            _kernel.RegistrarLog($"Escrita em arquivo: {caminho} ({tamanho} bytes) por {pidSimbolico}");
        }

        private void OperacaoArquivo(string pidSimbolico, string caminho, string operacao, int tamanho)
        {
            if (ObterArquivo(caminho) != null)
                _kernel.RegistrarLog($"{operacao} de arquivo: {caminho} ({tamanho} bytes) por {pidSimbolico}");
            else
                Console.WriteLine($"Arquivo {caminho} não encontrado.");
        }

        public void ApagarArquivo(string caminho)
        {
            var diretorio = ObterDiretorio(caminho);
            string nomeArquivo = ObterNome(caminho);

            if (diretorio != null && diretorio.Arquivos.Remove(nomeArquivo))
            {
                _tabelaAlocacao.LiberarBlocos(caminho);
                _kernel.RegistrarLog($"Arquivo apagado: {caminho}");
            } else
                Console.WriteLine($"Arquivo {caminho} não encontrado.");
        }

        public void ListarDiretorio()
        {
            Console.WriteLine($"\n===== Diretório: {_diretorioAtual.Nome} =====");
            foreach (var subdir in _diretorioAtual.Subdiretorios.Values)
                Console.WriteLine(subdir);
            foreach (var arquivo in _diretorioAtual.Arquivos.Values)
                Console.WriteLine(arquivo);
            Console.WriteLine("===============================\n");
        }

        public void MudarDiretorio(string nome)
        {
            if (nome == "..")
                _diretorioAtual = _raiz;
            else if (_diretorioAtual.Subdiretorios.ContainsKey(nome))
                _diretorioAtual = _diretorioAtual.Subdiretorios[nome];
            else
                Console.WriteLine($"Diretório {nome} não encontrado.");
        }

        private EntradaDiretorio? ObterDiretorio(string caminho)
        {
            var partes = caminho.Split('/', StringSplitOptions.RemoveEmptyEntries);
            EntradaDiretorio atual = _raiz;
            foreach (var parte in partes.Take(partes.Length - 1))
                if (!atual.Subdiretorios.TryGetValue(parte, out var sub))
                    return null;
                else
                    atual = sub;
            return atual;
        }

        private EntradaArquivo? ObterArquivo(string caminho)
        {
            var diretorio = ObterDiretorio(caminho);
            string nome = ObterNome(caminho);
            return diretorio != null && diretorio.Arquivos.TryGetValue(nome, out var arquivo) ? arquivo : null;
        }

        private static string ObterNome(string caminho) => caminho.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
    }

    public class TabelaDeAlocacao
    {
        private readonly Dictionary<string, List<int>> _blocos = new();

        public void AlocarBlocos(string caminho, int quantidadeBlocos)
        {
            if (!_blocos.ContainsKey(caminho))
                _blocos[caminho] = new List<int>();
            for (int i = 0; i < quantidadeBlocos; i++)
                _blocos[caminho].Add(_blocos[caminho].Count);
        }

        public void LiberarBlocos(string caminho) => _blocos.Remove(caminho);

        public int ObterQuantidadeBlocos(string caminho) => _blocos.TryGetValue(caminho, out var lista) ? lista.Count : 0;
    }
}
