using System.Threading.Tasks;
using BlazorFocused.Client;

namespace BlazorFocused.Store
{
    public interface IActionAsync<TState>
    {
        ValueTask<TState> ExecuteAsync(IRestClient restClient, TState state);
    }
}
