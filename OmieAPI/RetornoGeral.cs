namespace OmieAPI;

public class RetornoGeral
{
    public long CodigoEmpresa { get; set; }
    public long CodigoApiEndpoint { get; set; }
    public string Pagina { get; set; }
    public string TotalDePaginas { get; set; }
    public string Registros { get; set; }
    public string TotalDeRegistros { get; set; }
    public int PosicaoRetorno { get; set; }
    public string ChaveRetorno { get; set; }
    public string ValorRetorno { get; set; }

    public RetornoGeral(long codigoEmpresa, long codigoApiEndpoint, string pagina, string totalDePaginas, string registros, string totalDeRegistros, int posicaoRetorno, string chaveRetorno, string valorRetorno)
    {
        CodigoEmpresa = codigoEmpresa;
        CodigoApiEndpoint = codigoApiEndpoint;
        Pagina = pagina;
        TotalDePaginas = totalDePaginas;
        Registros = registros;
        TotalDeRegistros = totalDeRegistros;
        PosicaoRetorno = posicaoRetorno;
        ChaveRetorno = chaveRetorno;
        ValorRetorno = valorRetorno;
    }
}