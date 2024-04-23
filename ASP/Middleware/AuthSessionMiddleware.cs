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


		// В силу особливостей роботи Middleware, інжекція сервісів здійснюється не у
		// конструктор, а в параметри методу InvokeAsync
		public async Task InvokeAsync(HttpContext context, DataAccessor dataAccessor)
		{
			// "прямий хід" - від запиту до Razor
			if (context.Request.Query.ContainsKey("logout"))
			{
				context.Session.Remove("auth-user-id");
				context.Response.Redirect("/");
				return;     // без _next це припинить роботу
			}
			else if (context.Session.GetString("auth-user-id") is String userId)
			{
				var user = dataAccessor.UserDao.GetUserById(userId);
				if (user != null)
				{
					Claim[] claims = new Claim[] {
					new (ClaimTypes.Sid,        userId),
					new (ClaimTypes.Email,      user.Email),
					new (ClaimTypes.Name,       user.Name),
					new (ClaimTypes.UserData,   user.AvatarUrl ?? "")
					};

					context.User = new ClaimsPrincipal(
						new ClaimsIdentity(
							claims,
							nameof(AuthSessionMiddleware)
						)
					);
				}
				// context.Items.Add("auth", "ok");
				// Система авторизації ASP передбачає заповнення спецiального поля
				// context.User - набофу Claims-параметрів, кожен з яких відповідає
				// за свій атрибут Cid, email, ... )
			}
			await _next(context);
			// "зворотній хід" - від Razor до відповіді
		}
	}

	// Традиція - робити розширення, які замість арр.UseMiddleware<AuthSessionMiddleware>()
	// дозволять використовувати скорочену форму на кшталт арр. UseAuthSession()
	public static class AuthSessionMiddlewareExtensions
	{
		public static IApplicationBuilder UseAuthSession(this IApplicationBuilder app)
		{
			return app.UseMiddleware<AuthSessionMiddleware>();
		}
	}
}
/* Middleware створюється один раз при запуску проєкту, порядок виконання
 * об'єктів визначається з Program.cs та ядра ASP i кожен об'єкт Middleware
 * одержує як залежність RequestDelegate _next -- посилання на наступний
 * об'єкт, до якого слід передати управління після своєї роботи.
 */