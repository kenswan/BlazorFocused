namespace BlazorFocused.Client
{
    internal class OAuthToken
    {
        public string Scheme { get; set; }

        public string Token { get; set; }

        public void Update(string scheme, string token)
        {
            Scheme = scheme;
            Token = token;
        }

        public bool IsEmpty() =>
            string.IsNullOrWhiteSpace(Scheme) || string.IsNullOrEmpty(Token);
    }
}
