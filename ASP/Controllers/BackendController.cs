using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ASP.Middleware;
using System.Security.Claims;

namespace ASP.Controllers
{
    public class BackendController : ControllerBase, IActionFilter
    {
        protected bool isAuthenticated;
        protected bool isAdmin;
        protected IEnumerable<Claim>? claims;

        [NonAction]
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var identity = User.Identities.FirstOrDefault(i => i.AuthenticationType == nameof(AuthSessionMiddleware));
            identity ??= User.Identities.FirstOrDefault(i => i.AuthenticationType == nameof(AuthTokenMiddleware));
            this.isAuthenticated = identity != null;
            String? userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            this.isAdmin = "Admin".Equals(userRole);
            claims = identity?.Claims;
        }
        protected String? getAdminAuthMessage()
        {
            if (!isAuthenticated)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return HttpContext.Items[nameof(AuthTokenMiddleware)]?.ToString() ?? "Auth Required";
            }
            if (!isAdmin)
            {
                Response.StatusCode = StatusCodes.Status403Forbidden;
                return "Access to API forbidden";
            }
            return null;
        }
        [NonAction]
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
