using Newtonsoft.Json.Linq;
using OmieAPI.Entidades;
using RestSharp;
using System.Text.Json;

namespace OmieAPI.AgenteExterno;

public static class Requisicao
{

    public static async Task<RestResponse> FazerRequisicao(Empresa empresa, ApiEndpoint endpoint, ConfiguracaoJson configuracao, int pagina = 1)
    {
        var options = new RestClientOptions(endpoint.UrlBase);

        var client = new RestClient(options);

        var request = new RestRequest(endpoint.CaminhoRelativo, Method.Post);

        request.AddHeader("Content-Type", "application/json");

        var parametrosJson = JObject.Parse(configuracao.TemplateParametro.Replace("$$$pagina$$$", pagina.ToString()).Replace("$$$total$$$", configuracao.ValorRegistrosPorPagina.ToString()));

        var parametrosObjeto = JsonSerializer.Deserialize<object>(parametrosJson.ToString());

        var body = new
        {
            call = endpoint.MetodoChamada,
            app_key = empresa.AppChave,
            app_secret = empresa.AppSegredo,
            param = new[]
            {
                parametrosObjeto
            }
        };

        var jsonBody = JsonSerializer.Serialize(body);

        request.AddStringBody(jsonBody, DataFormat.Json);

        RestResponse response = await client.ExecuteAsync(request);

        return response;
    }
}
