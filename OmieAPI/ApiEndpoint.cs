namespace OmieAPI
{
    public class ApiEndpoint
    {
        public long Codigo { get; set; }

        public required string UrlBase { get; set; }

        public required string CaminhoRelativo { get; set; }

        public required string MetodoChamada { get; set; }

        public required int CodigoConfiguracaoJson { get; set; }

        public required string ChaveRetorno { get; set; }

        public DateTime Criacao { get; set; }
    }
}