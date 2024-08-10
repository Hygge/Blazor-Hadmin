using System;
using Blazored.LocalStorage;
using shared.Entity;
using System.Security.Claims;

namespace font.Authorization
{
    public class AuthenticationService
    {
        public readonly string _token = "accessToken";
        public readonly string _bearer = "bearer";
        public readonly string _userDetail = "userDetail";
        public readonly string _authentication = "User Authentication";
        public event Action<ClaimsPrincipal>? UserChanged;

        public event Action<string, UserDetail>? LoginInState;

        public event Action? LogoutState;
        public readonly ISyncLocalStorageService localStorageService;
  
        private ClaimsPrincipal? currentUser;
        public ClaimsPrincipal CurrentUser
        {
            get { return currentUser ?? new(); }
            set
            {
                currentUser = value;

                if (UserChanged is not null)
                {
                    UserChanged(currentUser);
                }
            }
        }


        public AuthenticationService(ISyncLocalStorageService _syncLocalStorageService)
        {
            this.localStorageService = _syncLocalStorageService;
        }

        public void LoginIn(string accessToken, UserDetail userDetail)
        {
            SetAccessToken(accessToken);
            SetUserDetail(userDetail);
            LoginInState(accessToken, userDetail);
        }
        public void Logout()
        {
            RemoveAccessToken();
            RemoveUserDetail();
            LogoutState();
        }

        public ClaimsPrincipal GetUserClaimsPrincipal(UserDetail user)
        {
            var identity = new ClaimsIdentity(
           new[]
           {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Id", user.Id.ToString()),
                new Claim("userId", user.UserId.ToString()),
           },
           _authentication);


            return new ClaimsPrincipal(identity);
        }
        
        public void RemoveAccessToken()
        {
            localStorageService.RemoveItem(_token);
        }
    
        public string? GetAccessToken()
        {
           return localStorageService.GetItemAsString(_token);
        }
     
        public void SetAccessToken(string accessToken)
        {
            localStorageService.SetItemAsString(_token, accessToken);
        }
        public UserDetail? GetUserDetail()
        {
            return localStorageService.GetItem<UserDetail>(_userDetail);
        }
        public void RemoveUserDetail()
        {
            localStorageService.RemoveItem(_userDetail);
        }
        public void SetUserDetail(UserDetail userDetail)
        {
            localStorageService.SetItem<UserDetail>(_userDetail, userDetail);
        }

    }
}
