using System.Net.Http;

namespace BlazorFocused.Testing
{
    public interface ISimulatedHttp
    {
        string BaseAddress { get; }

        HttpClient Client();

        ISimulatedHttpSetup Setup(HttpMethod method, string url);

        void VerifyWasCalled(HttpMethod method = default, string url = default);
    }
}
