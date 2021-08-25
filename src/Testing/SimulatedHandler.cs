using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorFocused.Testing
{
    internal class SimulatedHandler
    {
        public static async Task<(HttpMethod method, string url, string content)> GetRequestMessageContents(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var method = request.Method;
            var url = request.RequestUri.OriginalString;
            var content = (request.Content is not null) ?
                await request.Content.ReadAsStringAsync(cancellationToken) : string.Empty;

            return (method, url, content);
        }
    }
}
