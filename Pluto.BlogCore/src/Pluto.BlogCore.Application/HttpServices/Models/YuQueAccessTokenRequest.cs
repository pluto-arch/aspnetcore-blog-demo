using Newtonsoft.Json;

namespace Pluto.BlogCore.Application.HttpServices.Models
{
    public class YuQueAccessTokenRequest
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
        
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType => "authorization_code";
    }
}