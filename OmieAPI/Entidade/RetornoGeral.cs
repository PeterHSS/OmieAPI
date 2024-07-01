namespace OmieAPI.Entidades;

public class RetornoGeral(long codigoEmpresa, long codigoApiEndpoint, string pagina, string totalDePaginas, string registros, string totalDeRegistros, int posicaoRetorno, string chaveRetorno, string valorRetorno)
{
    public long CodigoEmpresa { get; set; } = codigoEmpresa;
    public long CodigoApiEndpoint { get; set; } = codigoApiEndpoint;
    public string Pagina { get; set; } = pagina;
    public string TotalDePaginas { get; set; } = totalDePaginas;
    public string Registros { get; set; } = registros;
    public string TotalDeRegistros { get; set; } = totalDeRegistros;
    public int PosicaoRetorno { get; set; } = posicaoRetorno;
    public string ChaveRetorno { get; set; } = chaveRetorno;
    public string ValorRetorno { get; set; } = valorRetorno;
}