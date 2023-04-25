namespace ProductManager.UI.Services.Http
{
    public interface IHttpService
    {
        Task<TResponse> MakeRequest<TRequest, TResponse>(HttpMethod httpMethod, string queryPath, TRequest model);
    }
}
