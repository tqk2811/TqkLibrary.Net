#if NET462_OR_GREATER
namespace TqkLibrary.Net.Ecommerce.ChoTot
{
    public class OauthResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token { get; set; }

        public Profile profile { get; set; }
    }
}
#endif