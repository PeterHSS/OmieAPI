using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace OmieAPI;
public class AcessoDados : IDisposable
{
    private readonly string? _connectionString;
    private bool _disposed = false;


    public AcessoDados(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PRD");
    }

    public async Task<IEnumerable<Empresa>> ObterListaEmpresa()
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            const string query = @"SELECT * FROM Empresas";

            connection.Open();

            var listaEmpresas = await connection.QueryAsync<Empresa>(query, commandTimeout: 9999);

            return listaEmpresas;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<ApiEndpoint>> ObterListaApiEndpoint()
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            const string query = @"SELECT * FROM ApiEndpoint";

            connection.Open();

            var listaApiEndpoint = await connection.QueryAsync<ApiEndpoint>(query, commandTimeout: 9999);

            return listaApiEndpoint;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<IEnumerable<ConfiguracaoJson>> ObterListaConfiguracaoJson()
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            const string query = @"SELECT * FROM ConfiguracaoJson";

            connection.Open();

            var listaConfiguracaoJson = await connection.QueryAsync<ConfiguracaoJson>(query, commandTimeout: 9999);

            return listaConfiguracaoJson;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task InsereRetornoGeral(IEnumerable<RetornoGeral> listaRetornoGeral) 
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("@tipoRetornoGeral", ObterTabelaRetornoGeralDeListaRetornoGeral(listaRetornoGeral), DbType.Object, ParameterDirection.Input);

            connection.Open();

            await connection.ExecuteAsync("InsereRetornoGeral", parametros, commandType: CommandType.StoredProcedure, commandTimeout: 9999);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task InsereRequisicaoComFalha(Empresa empresa, ApiEndpoint apiEndpoint, ConfiguracaoJson configuracaoJson, int pagina)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            const string query = @"INSERT INTO ErroRequisicao (CodigoEmpresa, CodigoApiEndpoint, CodigoConfiguracaoJson, Pagina)
                                    VALUES (@CodigoEmpresa, @CodigoApiEndpoint, @CodigoConfiguracaoJson, @Pagina)";

            var parametros = new
            { 
                CodigoEmpresa = empresa.Codigo,
                CodigoApiEndpoint = apiEndpoint.Codigo,
                CodigoConfiguracaoJson = configuracaoJson.Codigo,
                Pagina = pagina
            };

            connection.Open();

            await connection.ExecuteAsync(query, parametros, commandTimeout: 9999);
        }
        catch (Exception)
        {

            throw;
        }
    }

    private DataTable ObterTabelaRetornoGeralDeListaRetornoGeral(IEnumerable<RetornoGeral> listaRetornoGeral)
    {
        DataTable tabela = new DataTable();

        tabela.Columns.Add("CodigoEmpresa", typeof(long));
        tabela.Columns.Add("CodigoApiEndpoint", typeof(long));
        tabela.Columns.Add("Pagina", typeof(string));
        tabela.Columns.Add("TotalDePaginas", typeof(string));
        tabela.Columns.Add("Registros", typeof(string));
        tabela.Columns.Add("TotalDeRegistros", typeof(string));
        tabela.Columns.Add("PosicaoRetorno", typeof(int));
        tabela.Columns.Add("ChaveRetorno", typeof(string));
        tabela.Columns.Add("ValorRetorno", typeof(string));

        foreach(var retornoGeral in listaRetornoGeral) 
        { 
            tabela.Rows.Add(
                retornoGeral.CodigoEmpresa,
                retornoGeral.CodigoApiEndpoint,
                retornoGeral.Pagina,
                retornoGeral.TotalDePaginas,
                retornoGeral.Registros,
                retornoGeral.TotalDeRegistros,
                retornoGeral.PosicaoRetorno,
                retornoGeral.ChaveRetorno,
                retornoGeral.ValorRetorno
                );
        }

        return tabela;
    }

    public async Task ExecutaProcedureAsync(string nomeProcedure)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            await connection.ExecuteAsync(nomeProcedure, commandType: CommandType.StoredProcedure, commandTimeout: 9999);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<DataTable> ObterRequisicaoParaReprocessamento()
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            const string query = @"SELECT TOP 1 * FROM ErroRequisicao ORDER BY Codigo ASC";

            connection.Open();

            var leitor = await connection.ExecuteReaderAsync(query, commandTimeout: 9999);

            var tabela = new DataTable();

            tabela.Load(leitor);

            return tabela;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task DeletarRequisicaoParaReprocessamento(int codigoRequisicao)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            const string query = @"DELETE FROM ErroRequisicao WHERE Codigo = @Codigo";

            var parametros = new { Codigo = codigoRequisicao };

            connection.Open();

            await connection.ExecuteAsync(query, parametros, commandTimeout: 9999);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            // Libere recursos gerenciados, se houver
            // No momento, não há recursos gerenciados persistentes para liberar.
        }

        // Libere recursos não gerenciados, se houver

        _disposed = true;
    }

  

    ~AcessoDados()
    {
        Dispose(false);
    }
}
