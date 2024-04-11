using System.Globalization;

namespace ASP.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline.
            if(context.Session.GetString("auth-user-id") is String userId)
            {
                context.Items.Add("auth", "ok");
            }
            await _next(context);
        }
    }
}
