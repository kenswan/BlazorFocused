namespace BlazorFocused.Tools.Client
{
    internal class SimulatedVerificationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            (HttpMethod _, string url, string _) =
                await SimulatedHandler.GetRequestMessageContents(request, cancellationToken);

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri _))
            {
                throw new SimulatedHttpTestException($"Url is not propert Uri: {url}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
