using System.Net;

namespace BlazorFocused.Tools.Http;

internal class SimulatedHttpSetup : ISimulatedHttpSetup
{
    private readonly SimulatedHttpRequest request;
    private readonly Action<SimulatedHttpRequest, HttpStatusCode, object> responseAction;

    public SimulatedHttpSetup(
        SimulatedHttpRequest request,
        Action<SimulatedHttpRequest, HttpStatusCode, object> responseAction)
    {
        this.request = request;
        this.responseAction = responseAction;
    }

    public void ReturnsAsync(HttpStatusCode statusCode, object responseObject)
    {
        responseAction(request, statusCode, responseObject);
    }
}
