using BlazorFocused.Client;
using Integration.Client.Extensions;
using Integration.Sdk.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Integration.Client.Services
{
    public class UserAuthenticationProvider : AuthenticationStateProvider
    {
        protected ClaimsPrincipal claimsPrincipal;
        private readonly IOAuthRestClient oAuthRestClient;
        private readonly ILogger<UserAuthenticationProvider> logger;

        public UserAuthenticationProvider(
            IOAuthRestClient oAuthRestClient,
            ILogger<UserAuthenticationProvider> logger) : base()
        {
            claimsPrincipal = GetDefaultClaimsPrincipal();
            this.oAuthRestClient = oAuthRestClient;
            this.logger = logger;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(new AuthenticationState(claimsPrincipal));

        public virtual async Task LoginAsync(string userName, string password)
        {
            var userResponse = await oAuthRestClient.TryPostAsync<User>(
                "/api/auth/login", new Login
                {
                    UserName = userName,
                    Password = password
                });

            GetNewAuthenticatedUser(userResponse);
        }

        public void Logout()
        {
            claimsPrincipal = GetDefaultClaimsPrincipal();

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public virtual async Task RegisterAsync(Register newUser)
        {
            var userResponse = await oAuthRestClient.TryPostAsync<User>("/api/auth/register", newUser);

            GetNewAuthenticatedUser(userResponse);
        }

        private void GetNewAuthenticatedUser(RestClientResponse<User> restClientResponse)
        {
            if (restClientResponse.IsValid)
            {
                var user = restClientResponse.Value;
                
                oAuthRestClient.AddAuthorization("Bearer", user.Token);
                
                CreateClaimsPrincipalFromUser(user);
            }
            else
            {
                logger.LogError($"User authentication failed with {restClientResponse.StatusCode} - {restClientResponse.Exception.Message}");
            }
        }

        private void CreateClaimsPrincipalFromUser(User user)
        {
            if (user is not null)
            {
                claimsPrincipal = user.ToClaimsPrincipal();

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        private static ClaimsPrincipal GetDefaultClaimsPrincipal() => new(new ClaimsIdentity());
    }
}
