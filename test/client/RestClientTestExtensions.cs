using BlazorFocused.Tools;

namespace BlazorFocused
{
    internal static partial class RestClientTestExtensions
    {
        public static ISimulatedHttpSetup GetHttpSetup(this ISimulatedHttp simulatedHttp,
                HttpMethod httpMethod, string url, object request) =>
            httpMethod switch
            {
                { } when httpMethod == HttpMethod.Delete => simulatedHttp.SetupDELETE(url),
                { } when httpMethod == HttpMethod.Get => simulatedHttp.SetupGET(url),
                { } when httpMethod == HttpMethod.Patch => simulatedHttp.SetupPATCH(url, request),
                { } when httpMethod == HttpMethod.Post => simulatedHttp.SetupPOST(url, request),
                { } when httpMethod == HttpMethod.Put => simulatedHttp.SetupPUT(url, request),
                _ => null
            };
    }
}
