using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace TqkLibrary.Net.CloudStorage.OneDrive
{
public partial class OneDriveApiNonLogin
    {
        public class TokenApiV2Data
        {
            [JsonProperty("authScheme")]
            public string? AuthScheme { get; set; }

            [JsonProperty("token")]
            public string? Token { get; set; }

            [JsonProperty("expiryTimeUtc")]
            public DateTime? ExpiryTimeUtc { get; set; }
        }
    }
}