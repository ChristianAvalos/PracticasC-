namespace BivliotecaAPI.DTOs
{
    public record PaginacionDTO(int Pagina, int RecordPorPagina = 10)
    {
        private const int MaxRecordPorPagina = 50;

        public int Pagina { get; init; } = Math.Max(1,Pagina);
        public int RecordPorPagina { get; init; } = Math.Clamp(RecordPorPagina, 1, MaxRecordPorPagina);
    }
}
