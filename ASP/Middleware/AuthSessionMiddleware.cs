using ASP.Data.DAL;
using ASP.Middleware;
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
            if (context.Request.Query.ContainsKey("Logout"))
            {
                context.Session.Remove("auth-user-id");
                context.Response.Redirect("/");
                return;
            }

            else if(context.Session.GetString("auth-user-id") is String userId)
            {
                var user = dataAccessor.UserDao.GetUserById(userId);
                if (user != null)
                {
                    Claim[] claims = new Claim[]{
                    new Claim(ClaimTypes.Sid , userId),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.UserData, user.AvatarUrl ?? ""),
                     };
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(claims,
                        nameof(AuthSessionMiddleware)
                        ));
                }
            }
            await _next(context);
        }
    }



    public static class RequestCultureMiddlewareExtensions 
    { public static IApplicationBuilder UseAuthSession(this IApplicationBuilder app) 
        { 
            return app.UseMiddleware<AuthSessionMiddleware>(); 
        } 
    }
}