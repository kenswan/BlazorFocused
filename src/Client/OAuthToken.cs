namespace BlazorFocused.Client
{
    internal class OAuthToken
    {
        public string Scheme { get; set; }

        public string Token { get; set; }

        public bool IsEmpty() =>
            string.IsNullOrWhiteSpace(Scheme) || string.IsNullOrEmpty(Token);
    }
}
