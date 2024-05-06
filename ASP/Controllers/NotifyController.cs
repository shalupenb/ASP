using ASP.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace ASP.Controllers
{
	[Route("api/notify")]
	[ApiController]
	public class NotifyController(IEmailService emailService) : ControllerBase
	{
		private readonly IEmailService _emailService = emailService;

		[HttpGet]
		public Object DoGet() 
		{
			try
			{
				MailMessage mailMessage = new()
				{
					IsBodyHtml = true,
					Body = "<h1>Шановний користувач!</h1><br/>" +
					"<p style='color: steelblue'>Вітаємо на сайті " +
					$"<a href='{Request.Host}'>ASP</a></p>"
				};
				mailMessage.To.Add(new MailAddress("danyacorporation6@gmail.com"));
				_emailService.Send(mailMessage);
				return new { Sent = "OK" };
			}
			catch (Exception ex)
			{
				return new { Error = ex.Message };
			}
		}
	}
}
