using TqkLibrary.Net.Mail.OutlookGraphApi.Interfaces;

namespace TqkLibrary.Net.Mail.OutlookGraphApi.Classes
{
    public class AuthenticationRefreshToken : IAuthenticationRefreshToken
    {
        public required string RefreshToken { get; set; }
        public required string ClientId { get; set; }
    }
}
