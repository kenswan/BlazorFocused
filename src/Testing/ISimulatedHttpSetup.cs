using System.Net;

namespace BlazorFocused.Testing
{
    public interface ISimulatedHttpSetup
    {
        void ReturnsAsync(HttpStatusCode statusCode, object responseObject);
    }
}
