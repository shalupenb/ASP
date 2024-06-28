using ASP.Data;
using ASP.Models;
using ASP.Models.FrontendForm;
using ASP.Models.Home.Ioc;
using ASP.Services.Hash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ASP.Models.Home.Signup;
using System.Text.RegularExpressions;
using ASP.Data.DAL;
using ASP.Services.Kdf;
using ASP.Services.Email;
using System.Net.Mail;

namespace ASP.Controllers
{
	public class HomeController : Controller
	{
		/* �������� ������ (�����������) - ����� � ����������
        �� �������� �������� �� ������� ��'����. �������
        �������������� ����� �������� - ����� �����������. ��
        ��������, ��-�����, ��������� ���� �� ������ (readonly) ��,
        ��-�����, ������������ ��������� ��'���� ��� ��������
        �����������. � ���������� ����� �������� �������������
        �� ����� ��������� (_logger)
        */

		// �������� ��������� ����� - ���� � �� ������, �� ���� ������
		private readonly DataContext _dataContext;
		private readonly DataAccessor _dataAccessor;
		private readonly IKdfService _kdfService;
		private readonly IEmailService _emailService;

		private readonly ILogger<HomeController> _logger;

		// ��������� ���� ��� ��������� �� �����
		private readonly IHashService _hashService;

		//������ �� ������������ ��������-��������� � �������� �� � ��
		public HomeController(IHashService hashService, ILogger<HomeController> logger, DataContext dataContext, DataAccessor dataAccessor, IKdfService kdfService, IEmailService emailService)
		{
			_logger = logger;               // ���������� ��������� �����������, �� ��
			_hashService = hashService;     // ������ ��������� ��� �������� ����������
			_dataContext = dataContext;
			_dataAccessor = dataAccessor;
			_kdfService = kdfService;
			_emailService = emailService;
		}
		public IActionResult ConfirmEmail(String id)
		{
			// dXNlckBpLnVhOnF3ZTMyMQ== || user@i.ua:123
			String email, code;
			try
			{
				String data = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(id));
				String[] parts = data.Split(':', 2);
				email = parts[0];
				code = parts[1];
				ViewData["result"] = _dataAccessor.UserDao.ConfirmEmail(email, code) ? "����� ������ �����������" : "������� ������������ �����";
			}
			catch
			{
				ViewData["result"] = "��� �� ��������";
			}
			return View();
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AboutRazor()
		{
			Models.Home.AboutRazor.AboutRazorPageModel aboutRazorModel = new()
			{
				TabHeader = "Razor",
				PageTitle = "��������� Razor",
				RazorIs = "��������� ��������� �� ������ HTML �������\r\n\t������ ���� ������������� C#.",
				RazorInstrc = "���������� - ���������� ������ ���� �������������, �� ������ ����\r\n\t�� �������� " +
				"(���������� ���� �� ������� ��������),\r\n\t��� � ���������������� ���������.\r\n\tRazor-i��������� ������� � ������ ����� " +
				"&commat;{...}\r\n\t� ������� ����� ������ ���� ������ ���������� ����� �#.\r\n\t������������� ������� ���� � ����������, ��" +
				" ���������� �������\r\n\t�� ������������� ��� � �� " +
				"�������������� ������ �� ���� ������\r\n\t������.",
			};
			return View(aboutRazorModel);
		}
		// ������ ����� ����������� ���������� ������, ����������-�����������
		public IActionResult Model(Models.Home.Model.FormModel? formModel)
		{
			// ������ ������������� ����������� � ������������ ������i���
			Models.Home.Model.PageModel pageModel = new()
			{
				TabHeader = "�����i",
				PageTitle = "�����i � ASP",
				FormModel = formModel
			};
			// ������ ������������� ���������� ���������� View()
			return View(pageModel);
		}

		[HttpPost]      //������� �������� ����� ���� POST-�������
		public JsonResult FrontendFrom([FromBody] FrontendFormInput input)
		{
			FrontendFormOutput output = new()
			{
				Code = 200,
				Message = $"{input.UserName} -- {input.UserEmail} -- {input.UserGen} -- {input.UserDate.ToString().Substring(0, 10)}"
			};
			_logger.LogInformation(output.Message);
			return Json(output);
		}
        public IActionResult Signup(SingupFormModel? formModel)
        {
			SingupPageModel pageModel = new()
			{
				FormModel = formModel
			};
			if(formModel?.HasData ?? false)
			{
				pageModel.ValidationErrors = _ValidateSingupModel(formModel);
				if(pageModel.ValidationErrors.Count == 0)
				{
					String code = RandomStringService.GenerateOTP(6);
					String slug = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{formModel.UserEmail}:{code}"));
					MailMessage mailMessage = new()
					{
						Subject = "ϳ����������� �����",
						IsBodyHtml = true,
						Body = "<p>��� ������������ ����� ������ �� ���� ���</p>" +
						$"<h2 style='color: steelblue'>{code}</h2>" +
						$"<p>��� �������� �� <a href='{Request.Scheme}://{Request.Host}/Home/ConfirmEmail/{slug}'>��� ����������</a></p>"
					};
					_logger.LogInformation(mailMessage.Body);
					mailMessage.To.Add(formModel.UserEmail);
					try 
					{
						_emailService.Send(mailMessage);
						String salt = RandomStringService.GenerateSalt(10);
						_dataAccessor.UserDao.Signup(new()
						{
							Name = formModel.UserName,
							Email = formModel.UserEmail,
							EmailConfirmCode = code,
							Birthdate = formModel.UserBirthdate,
							AvatarUrl = formModel.SavedAvatarFilename,
							Salt = salt,
							Derivedkey = _kdfService.DerivedKey(salt, formModel.Password)
						});
					}
					catch (Exception ex)
					{
						pageModel.ValidationErrors["email"] = "�� ������� �������� email";
						_logger.LogInformation(ex.Message);
					}
				}
			}
			//_logger.LogInformation(Directory.GetCurrentDirectory());
            return View(pageModel);
        }

		public ViewResult Admin()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


		private Dictionary<string, string> _ValidateSingupModel(SingupFormModel? model)
		{
			Dictionary<string, string> result = new();
			if (model == null)
			{
				result["model"] = "Model is Null";
			}
			else
			{
				if (String.IsNullOrEmpty(model.UserName))
				{
					result[nameof(model.UserName)] = "User Name should not be empty";
				}
				if (String.IsNullOrEmpty(model.UserEmail))
				{
					result[nameof(model.UserEmail)] = "User Email should not be empty";
				}
				if (model.UserBirthdate == default(DateTime))
				{
					result[nameof(model.UserBirthdate)] = "User Birthdate should not be empty";
				}
				if (model.UserAvatar != null)
				{
					string ext = Path.GetExtension(model.UserAvatar.FileName);
					List<string> imageExtensions = new List<string>() { ".png", ".jpg", ".jpeg", ".svg", ".bmp", ".gif", ".webp" };
					if (!imageExtensions.Contains(ext))
						result[nameof(model.UserAvatar)] = "User Avatar must be an image type (.png, .jpg, .jpeg, .svg, .bmp, .gif, .webp)";
					// ��������� ���� � wwwroot/img/avatars � ������ �������
					// (���������� ����� �� ������������� ���������)
					String path = Directory.GetCurrentDirectory() + "/wwwroot/img/avatars/";
					_logger.LogInformation(path);
					String fileName;
					String pathName;
					do
					{
						fileName = RandomStringService.GenerateFilename(10) + ext;
						pathName = path + fileName;
					}
					while (System.IO.File.Exists(pathName));
					_logger.LogInformation(pathName);

					using var steam = System.IO.File.OpenWrite(pathName);
					model.UserAvatar.CopyTo(steam);

					model.SavedAvatarFilename = fileName;
				}
				if (!model.Agreement)
				{
					result[nameof(model.Agreement)] = "User Agreement must be checked";
				}
				if (String.IsNullOrEmpty(model.Password))
				{
					result[nameof(model.Password)] = "User Password should not be empty";
				}
				if (!String.IsNullOrEmpty(model.Password))
				{
					Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d).+$");
					if (!regex.IsMatch(model.Password))
						result[nameof(model.Password)] = "User Password should contain 1 symbol and 1 digit";
				}
				if (String.IsNullOrEmpty(model.UserRepeat))
				{
					result[nameof(model.UserRepeat)] = "User Repeat Password should not be empty";
				}
				if (!String.IsNullOrEmpty(model.Password))
				{
					if (!(model.UserRepeat == model.Password))
						result[nameof(model.UserRepeat)] = "User Passwords not the same";
				}
			}
			return result;
		}
	}
}
