namespace SimuladorSO.Enum
{
    public enum Enum : sbyte
    {
        FirstFit,
        BestFit
    }
    public enum EEstadoProcesso : sbyte
    {
        Novo,
        Pronto,
        Executando,
        Bloqueado,
        Finalizado
    }
    public enum EstadoThread : sbyte
    {
        Novo,
        Pronto,
        Executando,
        Bloqueado,
        Finalizado
    }
}
