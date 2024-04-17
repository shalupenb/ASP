using ASP.Data.DAL;
using System.Globalization;
using System.Security.Claims;

namespace ASP.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataAccessor dataAccessor)
        {
            // Call the next delegate/middleware in the pipeline.
            if (context.Request.Query.ContainsKey("logout"))
            {
                context.Session.Remove("auth-user-id");
                context.Response.Redirect("/");
                return;
            }
            else if(context.Session.GetString("auth-user-id") is String userId)
            {
                var user = dataAccessor.UserDao.GetUserById(userId);
                if(user != null)
                {
                    Claim[] claims = new Claim[]
                    {
                        new(ClaimTypes.Sid,         userId),
                        new(ClaimTypes.Email,       user.Email),
                        new(ClaimTypes.Name,        user.Name),
                        new(ClaimTypes.UserData,    user.AvatarUrl ?? "")
                    };
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            claims,
                            nameof(AuthSessionMiddleware)
                        )
                    );
                    context.Items.Add("auth", "ok");
                }
            }
            await _next(context);
        }
    }

    public static class AuthSessionMiddlewareExtensions 
    { 
        public static IApplicationBuilder UseAuthSession(this IApplicationBuilder app) 
        { 
            return app.UseMiddleware<AuthSessionMiddleware>(); 
        } 
    }
}
