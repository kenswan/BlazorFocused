using System.Text.Json;

namespace BlazorFocused.Tools.Http;

internal partial class SimulatedHttp
{
    public void VerifyDELETEWasCalled(string url = null)
    {
        VerifyWasCalled(HttpMethod.Delete, url);
    }

    public void VerifyGETWasCalled(string url = null)
    {
        VerifyWasCalled(HttpMethod.Get, url);
    }

    public void VerifyPATCHWasCalled(string url = null, object content = null)
    {
        VerifyWasCalled(HttpMethod.Patch, url, content);
    }

    public void VerifyPOSTWasCalled(string url = null, object content = null)
    {
        VerifyWasCalled(HttpMethod.Post, url, content);
    }

    public void VerifyPUTWasCalled(string url = null, object content = null)
    {
        VerifyWasCalled(HttpMethod.Put, url, content);
    }

    private void VerifyWasCalled(HttpMethod method = default, string url = default, object content = default)
    {
        var requestString = content is not null ? JsonSerializer.Serialize(content) : null;

        SimulatedHttpRequest simulatedRequest = new()
        {
            Method = method,
            Url = url,
            RequestContent = requestString
        };

        var invalidCheck = simulatedRequest switch
        {
            { Method: null, Url: null, RequestContent: null } => CheckAny(),
            { Url: null, RequestContent: null } => CheckMethod(simulatedRequest),
            { RequestContent: null } => CheckMethodAndUrl(simulatedRequest),
            { } when simulatedRequest is not null => CheckFullRequest(simulatedRequest),
            _ => throw new SimulatedHttpTestException("Improper type of validation request")
        };

        if (!string.IsNullOrEmpty(invalidCheck))
            throw new SimulatedHttpTestException($"{invalidCheck} was not requested");
    }

    private string CheckFullRequest(SimulatedHttpRequest simulatedHttpRequest)
    {
        var method = simulatedHttpRequest.Method;
        var url = simulatedHttpRequest.Url;

        var matches = requests
                .Where(request => request.Method == method && request.Url == GetFullUrl(url));

        if (!matches.Any())
            return $"Method {method} & Url {url}";

        foreach (var match in matches)
        {
            var requestMatchesExpected = string.Equals(
                match.RequestContent,
                simulatedHttpRequest.RequestContent,
                StringComparison.InvariantCultureIgnoreCase);

            if (requestMatchesExpected)
                return string.Empty;
        }

        return "Request Object";
    }

    private string CheckMethodAndUrl(SimulatedHttpRequest simulatedHttpRequest)
    {
        var method = simulatedHttpRequest.Method;
        var url = simulatedHttpRequest.Url;

        var match = requests
                .Where(request => request.Method == method && request.Url == GetFullUrl(url))
                .FirstOrDefault();

        return match is not null ? string.Empty : $"Method {method} & Url {url}";
    }

    private string CheckMethod(SimulatedHttpRequest simulatedHttpRequest)
    {
        var method = simulatedHttpRequest.Method;

        var match = requests.Where(request => request.Method == method).FirstOrDefault();

        return match is not null ? String.Empty : $"Method {method}";
    }

    private string CheckAny() =>
        !requests.Any() ? "Generic (Any) Request" : string.Empty;
}
