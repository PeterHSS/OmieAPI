using Newtonsoft.Json.Linq;

namespace OmieAPI;

public class Negocio
{
    private readonly AcessoDados _acessoDados;

    public Negocio(AcessoDados acessoDados)
    {
        _acessoDados = acessoDados;
    }

    public async Task ExecutaLogica()
    {
        var listaEmpresas = await _acessoDados.ObterListaEmpresa();

        var tasks = new List<Task>();

        foreach (var empresa in listaEmpresas)
        {
            tasks.Add(Task.Run(() => RealizarChamadasAsync(empresa)));
        }

        Task.WaitAll(tasks.ToArray());

        await Console.Out.WriteLineAsync("Finalizado");
    }

    public async Task RealizarChamadasAsync(Empresa empresa)
    {

        var listaApiEndpoint = await _acessoDados.ObterListaApiEndpoint();

        foreach (var apiEndpoint in listaApiEndpoint)
        {
            int pagina = 1;

            var response = await Requisicao.FazerRequisicao(empresa, apiEndpoint, pagina);

            await _acessoDados.InsereRetornoGeral(empresa.Codigo, apiEndpoint.Codigo, response);

            var objeto = JObject.Parse(response);

            int totalPaginas = objeto[apiEndpoint.TipoPagina]!.Value<int>();

            while (pagina < totalPaginas)
            {
                pagina++;

                response = await Requisicao.FazerRequisicao(empresa, apiEndpoint, pagina);

                await _acessoDados.InsereRetornoGeral(empresa.Codigo, apiEndpoint.Codigo, response);
            }

            await Console.Out.WriteLineAsync();

        }
    }
}
