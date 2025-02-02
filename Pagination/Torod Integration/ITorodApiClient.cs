using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Pagination.Torod_Integration
{
    public interface ITorodApiClient
    {
        Task<HttpResponseMessage> AccessToken(CancellationToken cancellationToken = default);
    }
}
