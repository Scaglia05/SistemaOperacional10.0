namespace SimuladorSO.Nucleo
{
    public class Metrica
    {
        // Dispositivo
        public string? NomeDispositivo { get; set; }
        public int TempoOcupado { get; set; }
        public int TempoTotal { get; set; }
        public int NumeroRequisicoes { get; set; }

        // Memória
        public int FaltasPagina { get; set; }
        public int AcessosMemoria { get; set; }
        public int HitsTLB { get; set; }
        public int MissesTLB { get; set; }

        // Processo
        public string? PIDSimbolico { get; set; }
        public int TempoRetorno { get; set; }
        public int TempoEspera { get; set; }
        public int TempoResposta { get; set; }
        public int TempoCPU { get; set; }

        // Propriedades calculadas
        public double Utilizacao => TempoTotal > 0 ? (double)TempoOcupado / TempoTotal * 100 : 0;
        public double TaxaFaltaPagina => AcessosMemoria > 0 ? (double)FaltasPagina / AcessosMemoria * 100 : 0;
        public double TaxaAcertoTLB
        {
            get
            {
                int total = HitsTLB + MissesTLB;
                return total > 0 ? (double)HitsTLB / total * 100 : 0;
            }
        }

        public override string ToString()
        {
            if (NomeDispositivo != null)
                return $"{NomeDispositivo}: Utilização={Utilizacao:F2}%, Requisições={NumeroRequisicoes}";
            if (PIDSimbolico != null)
                return $"{PIDSimbolico}: Retorno={TempoRetorno}, Espera={TempoEspera}, Resposta={TempoResposta}, CPU={TempoCPU}";
            return $"Memória: Faltas={FaltasPagina} ({TaxaFaltaPagina:F2}%), TLB={TaxaAcertoTLB:F2}% acerto";
        }
    }
}
