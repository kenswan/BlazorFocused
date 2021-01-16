using System.Threading.Tasks;

namespace BlazorFocused.Client
{
    public interface IRestClient
    {
        ValueTask<T> GetAsync<T>(string relativeUrl);

        ValueTask<T> PostAsync<T>(string relativeUrl, object data);

        ValueTask<T> PutAsync<T>(string relativeUrl, object data);

        ValueTask<T> DeleteAsync<T>(string relativeUrl);
    }
}
