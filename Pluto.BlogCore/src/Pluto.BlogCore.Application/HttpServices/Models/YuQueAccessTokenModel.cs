using Newtonsoft.Json;

namespace Pluto.BlogCore.Application.HttpServices.Models
{
    public class YuQueAccessTokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}