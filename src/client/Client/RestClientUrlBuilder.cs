namespace BlazorFocused.Client
{
    internal class RestClientUrlBuilder : IRestClientUrlBuilder
    {
        private string basePath = string.Empty;
        private readonly Dictionary<string, string> parameters = new();

        public string Build()
        {
            var parameterString = parameters.Count > 0 ? "?" +
                string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")) : string.Empty;

            return basePath + parameterString;
        }

        public IRestClientUrlBuilder SetPath(string absoluteOrRelativeUrl)
        {
            basePath = absoluteOrRelativeUrl;
            return this;
        }

        public IRestClientUrlBuilder WithParameter(string key, string value)
        {
            parameters.Add(key, value);
            return this;
        }
    }
}
