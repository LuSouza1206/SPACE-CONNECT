using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SpaceConnect.ApiService.Filters;

public class AdminApiAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var perfil = context.HttpContext.Request.Headers["X-User-Perfil"].FirstOrDefault();
        if (perfil != "Administrador")
        {
            context.Result = new UnauthorizedObjectResult(new { mensagem = "Acesso restrito a administradores." });
        }
    }
}
