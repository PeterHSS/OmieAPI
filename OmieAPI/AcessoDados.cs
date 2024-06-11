namespace OmieAPI;
using Dapper;
using System.Data;
using System.Data.SqlClient;

public class AcessoDados
{
    private readonly string? _connectionString;

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

            var listaEmpresas = await connection.QueryAsync<Empresa>(query);

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

            var listaApiEndpoint = await connection.QueryAsync<ApiEndpoint>(query);

            return listaApiEndpoint;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task InsereRetornoGeral(long codigoEmpresa, long codigoApiEndpoint, string retornoJson) 
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            const string query = @"INSERT INTO RetornoGeral (CodigoEmpresa, CodigoApiEndpoint, RetornoJson) VALUES (@CodigoEmpresa, @CodigoApiEndpoint, @RetornoJson)";

            connection.Open();

            var parametros = new
            {
                CodigoEmpresa = codigoEmpresa,
                CodigoApiEndpoint = codigoApiEndpoint,
                RetornoJson = retornoJson
            };

            await connection.ExecuteAsync(query, parametros);
        }
        catch (Exception)
        {

            throw;
        }
    }

}
