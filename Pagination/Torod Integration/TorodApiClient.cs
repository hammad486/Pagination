using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Pagination.Torod_Integration
{
    public class TorodApiClient : ITorodApiClient
    {
        private HttpClient _httpClient;

       
        public TorodApiClient(HttpClient client)
        {
                _httpClient = client;
        }
        public async Task<HttpResponseMessage> AccessToken(CancellationToken cancellationToken = default)
        {
            _httpClient.BaseAddress = new Uri("https://demo.stage.torod.co/en/api/");
            AccessTokenRequest res = new AccessTokenRequest();
            //var cont=Serialize
           var response=await _httpClient.PostAsJsonAsync("token",res, cancellationToken = default);
            
          
            return response;
        }
    }
}
