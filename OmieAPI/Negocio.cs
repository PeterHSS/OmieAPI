using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace OmieAPI;

public class Negocio : IDisposable
{
    private readonly AcessoDados _acessoDados;
    private bool _disposed = false; // Flag para detectar chamadas redundantes

    public Negocio(AcessoDados acessoDados)
    {
        _acessoDados = acessoDados;
    }

    public async Task ExecutaLogica()
    {
        var listaEmpresas = await _acessoDados.ObterListaEmpresa();

        //var listaApiEndpoint = await _acessoDados.ObterListaApiEndpoint();
        //var listaConfiguracaoJson = await _acessoDados.ObterListaConfiguracaoJson();

        //var listaReprocessar = await _acessoDados.ObterListaParaReprocessamento();


        var tasks = new List<Task>();

        foreach (var empresa in listaEmpresas)
        {
            tasks.Add(Task.Run(() => RealizarChamadasAsync(empresa)));
        }

        Task.WaitAll(tasks.ToArray());

        //await _acessoDados.ExecutaProcedureAlimentarTabelas();
    }

    public async Task RealizarChamadasAsync(Empresa empresa)
    {

        var listaApiEndpoint = await _acessoDados.ObterListaApiEndpoint();

        var listaConfiguracaoJson = await _acessoDados.ObterListaConfiguracaoJson();

        foreach (var apiEndpoint in listaApiEndpoint)
        {

            List<string> retornosJson = new();

            ConfiguracaoJson configuracaoJson = listaConfiguracaoJson.FirstOrDefault(c => c.Codigo == apiEndpoint.CodigoConfiguracaoJson)!;

            int pagina = 1;

            var response = await Requisicao.FazerRequisicao(empresa, apiEndpoint, configuracaoJson, pagina);

            if (response.IsSuccessful)
            {
                string responseString = response.Content!.ToString()!;

                retornosJson.Add(responseString);

                var objeto = JObject.Parse(responseString);

                int totalPaginas = objeto[configuracaoJson.TotalDePaginas]!.Value<int>();

                while (pagina < totalPaginas)
                {
                    response = await Requisicao.FazerRequisicao(empresa, apiEndpoint, configuracaoJson, ++pagina);
                    responseString = response.Content!.ToString()!;

                    if (response.IsSuccessful)
                        retornosJson.Add(responseString);
                    else
                        await _acessoDados.InsereRequisicaoComFalha(empresa, apiEndpoint, configuracaoJson, ++pagina);

                }

                IEnumerable<RetornoGeral> retornoGeral = ObterListaDeRetornoGeralPorListaDeJson(empresa, apiEndpoint, configuracaoJson, retornosJson);

                await _acessoDados.InsereRetornoGeral(retornoGeral);
            }
            else
            {
                await _acessoDados.InsereRequisicaoComFalha(empresa, apiEndpoint, configuracaoJson, pagina);
            }

        }
    }

    private IEnumerable<RetornoGeral> ObterListaDeRetornoGeralPorListaDeJson(Empresa empresa, ApiEndpoint apiEndpoint, ConfiguracaoJson configuracaoJson, List<string> retornosJson)
    {
        List<RetornoGeral> listaRetornoGeral = new List<RetornoGeral>();

        foreach (var json in retornosJson)
        {
            var objeto = JObject.Parse(json);
            var pagina = objeto[configuracaoJson.Pagina]!.ToString();
            var totalDePaginas = objeto[configuracaoJson.TotalDePaginas]!.ToString();
            var registros = objeto[configuracaoJson.Registros]!.ToString();
            var totalDeRegistros = objeto[configuracaoJson.TotalDeRegistros]!.ToString();
            var retornosDaChave = objeto[apiEndpoint.ChaveRetorno]!.Children().Select(item => item.ToString()).ToList();

            //foreach (var retorno in retornosDaChave)
            //{
            //    listaRetornoGeral. Add(new RetornoGeral(empresa.Codigo, apiEndpoint.Codigo, pagina, totalDePaginas, registros, totalDeRegistros, retornosDaChave.IndexOf(retorno), apiEndpoint.ChaveRetorno, retorno));
            //}

            listaRetornoGeral.AddRange(retornosDaChave.Select((retorno, index) =>
              new RetornoGeral(
                  empresa.Codigo,
                  apiEndpoint.Codigo,
                  pagina,
                  totalDePaginas,
                  registros,
                  totalDeRegistros,
                  ++index,
                  apiEndpoint.ChaveRetorno,
                  retorno)));
        }

        return listaRetornoGeral;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Suprima a finalização
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Libere recursos gerenciados
            if (_acessoDados != null)
            {
                _acessoDados.Dispose();
            }
        }

        // Libere recursos não gerenciados (se houver)

        _disposed = true;
    }

    ~Negocio()
    {
        Dispose(false);
    }
}
