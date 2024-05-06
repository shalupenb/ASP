using System.Net.Mail;

namespace ASP.Services.Email
{
	public interface IEmailService
	{
		void Send(MailMessage mailMessage);
	}
}
