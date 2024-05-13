using ASP.Data.DAL;
using Azure.Core;
using Azure;
using Microsoft.Extensions.Primitives;
using ASP.Data.Entities;
using System.Security.Claims;

namespace ASP.Middleware
{
	public class AuthTokenMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthTokenMiddleware(RequestDelegate next)
		{
			_next = next;
		}


		// В силу особливостей роботи Middleware, інжекція сервісів здійснюється не у
		// конструктор, а в параметри методу InvokeAsync
		public async Task InvokeAsync(HttpContext context, DataAccessor dataAccessor)
		{
			var authHeader = context.Request.Headers["Authorization"];
			try
			{
				if (authHeader == StringValues.Empty)
				{
					throw new Exception("Authentification required");
				}
				String authValue = authHeader.First()!;
				if (!authValue.StartsWith("Bearer "))
				{
					throw new Exception("Bearer scheme required");
				}
				String token = authValue[7..];
				Guid tokenId;
				try { tokenId = Guid.Parse(token); }
				catch { throw new Exception("Token invalid: GUID required"); }
				User? user = dataAccessor.UserDao.GetUserByToken(tokenId) 
					?? throw new Exception("Token invalid or expired");
				Claim[] claims = new Claim[] {
						new (ClaimTypes.Sid,        user.Id.ToString()),
						new (ClaimTypes.Email,      user.Email),
						new (ClaimTypes.Name,       user.Name),
						new (ClaimTypes.UserData,   user.AvatarUrl ?? ""),
						new(ClaimTypes.Role,        user.Role ?? ""),
						new("EmailConfirmCode", user.EmailConfirmCode ?? "")
					};

				context.User.AddIdentity(
					new ClaimsIdentity(
						claims,
						nameof(AuthTokenMiddleware)
					)
				);
			}
			catch (Exception ex)
			{
				context.Items.Add(new(nameof(AuthTokenMiddleware), ex.Message));
			}
			await _next(context);
		}
	}
	public static class AuthTokenMiddlewareExtensions
	{
		public static IApplicationBuilder UseAuthToken(this IApplicationBuilder app)
		{
			return app.UseMiddleware<AuthTokenMiddleware>();
		}
	}
}
