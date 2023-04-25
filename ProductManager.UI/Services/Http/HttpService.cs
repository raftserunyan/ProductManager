using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ProductManager.UI.Helpers;

namespace ProductManager.UI.Services.Http
{
    internal class HttpService : IHttpService
    {
        private readonly string _url;

        public HttpService(IConfiguration configuration)
        {
            _url = configuration.GetValue<string>("BackendUrl");
        }

        public async Task<TResponse> MakeRequest<TRequest, TResponse>(HttpMethod httpMethod, string queryPath, TRequest model)
        {
            string httpResult = "";

            var json = JsonConvert.SerializeObject(model);
            httpResult = await HttpClientRequestData(httpMethod, queryPath, json);

            return JsonConvert.DeserializeObject<TResponse>(httpResult);
        }

        private async Task<string> HttpClientRequestData(HttpMethod httpMethod, string queryPath, string data)
        {
            HttpResponseMessage response = null;

            var url = $"{_url}/api/{queryPath}";
            var content = ConvertToHttpContent(data);

            using(var httpClient = new HttpClient())
            { 
                if (httpMethod == HttpMethod.Get)
                {
                    response = await httpClient.GetAsync(url);
                }
                else if (httpMethod == HttpMethod.Post)
                {
                    response = await httpClient.PostAsync(url, content);
                }
                else if (httpMethod == HttpMethod.Put)
                {
                    response = await httpClient.PutAsync(url, content);
                }
                else if (httpMethod == HttpMethod.Patch)
                {
                    response = await httpClient.PatchAsync(url, content);
                }
                else if (httpMethod == HttpMethod.Delete)
                {
                    response = await httpClient.DeleteAsync(url);
                }
            }

            await ThrowIfUnsuccessfulAsync(response);

            var result = response != null ? await response.Content.ReadAsStringAsync() : null;
            return result;
        }

        private HttpContent ConvertToHttpContent(string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }

        private async Task ThrowIfUnsuccessfulAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        try
                        {
                            CustomValidationException validationException = JsonConvert.DeserializeObject<CustomValidationException>(message);
                            throw new Exception(validationException.Errors.First().Value.First());
                        }
                        catch (JsonReaderException)
                        {
                            throw new Exception(message);
                        }
                    case (HttpStatusCode)500:
                        throw new Exception("Something went wrong on the server");
                    default:
                        break;
                }

                await using var stream = await response.Content.ReadAsStreamAsync();
                using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);
                var json = document.RootElement;

                throw null;
            }
        }
    }
}
