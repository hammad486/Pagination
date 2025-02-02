using System.Text.Json.Serialization;

namespace Pagination.Torod_Integration
{
    public class TokenResponse
    {
        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }
    }
}
