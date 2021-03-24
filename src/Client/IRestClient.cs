using System.Threading.Tasks;

namespace BlazorFocused.Client
{
    public interface IRestClient
    {
        Task<T> DeleteAsync<T>(string relativeUrl, object[] parameters = null);

        Task<T> GetAsync<T>(string relativeUrl, object[] parameters = null);

        Task<T> PostAsync<T>(string relativeUrl, object data, object[] parameters = null);

        Task<T> PutAsync<T>(string relativeUrl, object data, object[] parameters = null);

        Task<RestClientResponse<T>> TryDeleteAsync<T>(string relativeUrl, object[] parameters = null);

        Task<RestClientResponse<T>> TryGetAsync<T>(string relativeUrl, object[] parameters = null);

        Task<RestClientResponse<T>> TryPostAsync<T>(string relativeUrl, object data, object[] parameters = null);

        Task<RestClientResponse<T>> TryPutAsync<T>(string relativeUrl, object data, object[] parameters = null);
    }
}
