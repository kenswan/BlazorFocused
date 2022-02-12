using BlazorFocused.Client;

namespace BlazorFocused
{
    public static class UrlBuilder
    {
        public static IRestClientUrlBuilder SetPath(string relativeOrAbsoluteUrl) =>
            new RestClientUrlBuilder().SetPath(relativeOrAbsoluteUrl);
    }
}
