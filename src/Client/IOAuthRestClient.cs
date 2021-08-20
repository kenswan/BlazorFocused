namespace BlazorFocused.Client
{
    public interface IOAuthRestClient : IRestClient
    {
        void AddAuthorization(string scheme, string token);

        void ClearAuthorization();

        bool HasAuthorization();

        string RetrieveAuthorization();
    }
}
