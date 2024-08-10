using Microsoft.AspNetCore.Components.Authorization;
using shared.Entity;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace font.Authorization
{
    public class HAuthenticationStateProvider : AuthenticationStateProvider 
    {   
        public HttpClient? _httpClient { set; get; }

        private AuthenticationService service;

        public HAuthenticationStateProvider(AuthenticationService service,  HttpClient _httpClient)
        {     
            this._httpClient = _httpClient;          

            this.service = service;
            service.UserChanged += (newUser) =>
            {
                NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(newUser)));
            };
            service.LoginInState += MarkUserAsAuthenticated;
            service.LogoutState += MarkUserAsLoggedOut;
        }


        /// <summary>
        /// 身份认证提供
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = service.GetAccessToken();

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }
            // 已认证过请求时带上token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(service._bearer, savedToken);

            var user = service.GetUserDetail();
            if (user == null)
            {
                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                return Task.FromResult(new AuthenticationState(anonymousUser));

            }

            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity( new[]{ new Claim(ClaimTypes.Name, user.UserName),  new Claim("userId", user.UserId.ToString())  }, service._authentication))));

        }

        /// <summary>
        /// 辅助登录后刷新认证状态
        /// </summary>
        /// <param name="token"></param>
        public void MarkUserAsAuthenticated(string token, UserDetail user)
        {
            var authenticatedUser = new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, user.UserName), new Claim("userId", user.Id.ToString()) }, service._authentication)));           
            var authState = Task.FromResult(authenticatedUser);
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// 退出登录后刷新状态
        /// </summary>
        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }



    }
}
