namespace OmieAPI.Entidades
{
    public class ConfiguracaoJson
    {
        public required int Codigo { get; set; }
        public required string Pagina { get; set; }
        public required string TotalDePaginas { get; set; }
        public required string Registros { get; set; }
        public required string TotalDeRegistros { get; set; }
        public required string RegistrosPorPagina { get; set; }
        public required int ValorRegistrosPorPagina { get; set; }
        public required string ApenasImportadoApi { get; set; }
        public required string ValorApenasImportadoApi { get; set; }
        public required string TemplateParametro { get; set; }
        public required DateTime Criacao { get; set; }
    }
}
