using System.Net.Http.Headers;

namespace SpaceConnect.Web.Services;

public class ApiAuthHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            request.Headers.Remove("X-User-Perfil");
            request.Headers.Remove("X-User-Id");

            var perfil = user.FindFirst("Perfil")?.Value;
            var id = user.FindFirst("Id")?.Value ?? user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(perfil))
                request.Headers.Add("X-User-Perfil", perfil);
            if (!string.IsNullOrEmpty(id))
                request.Headers.Add("X-User-Id", id);
        }

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return base.SendAsync(request, cancellationToken);
    }
}
