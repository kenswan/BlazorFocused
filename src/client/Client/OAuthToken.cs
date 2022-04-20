namespace BlazorFocused.Client;

internal class OAuthToken : IOAuthToken
{
    public string Scheme { get; private set; }

    public string Token { get; private set; }

    public OAuthToken()
    {
        Scheme = string.Empty;
        Token = string.Empty;
    }

    public OAuthToken(string scheme, string token)
    {
        Update(scheme, token);
    }

    public void Update(string scheme, string token)
    {
        Scheme = scheme;
        Token = token;
    }

    public bool IsEmpty() =>
        string.IsNullOrWhiteSpace(Scheme) || string.IsNullOrEmpty(Token);

    public override string ToString() =>
        (IsEmpty()) ? "" : $"{Scheme} {Token}";
}
