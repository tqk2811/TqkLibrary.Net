namespace TqkLibrary.Net.Mail.OutlookGraphApi.Interfaces
{
    public interface IAuthenticationRefreshToken
    {
        string ClientId { get; }
        string RefreshToken { get; }
    }
}
