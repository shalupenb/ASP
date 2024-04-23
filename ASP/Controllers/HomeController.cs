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

namespace ASP.Controllers
{
	public class HomeController : Controller
	{
		/* Інжекція сервісів (залежностей) - запит у контейнера
        на передачу посилань на відповідні об'єкти. Найбільш
        рекомендований спосіб інжекції - через конструктор. Це
        дозволяє, по-перше, оголосити поля як незмінні (readonly) та,
        по-друге, унеможливити створення об'єктів без передачі
        залежностей. У стартовому проєкті інжекція демонструється
        на сервісі логування (_logger)
        */

		// інжекція контексту даних - така ж за формою, як інші сервіси
		private readonly DataContext _dataContext;
		private readonly DataAccessor _dataAccessor;
		private readonly IKdfService _kdfService;

		private readonly ILogger<HomeController> _logger;

		// створюємо поле для посилання на сервіс
		private readonly IHashService _hashService;

		//додаємо до конструктора параметр-залежність і зберігаємо її у тілі
		public HomeController(IHashService hashService, ILogger<HomeController> logger, DataContext dataContext, DataAccessor dataAccessor, IKdfService kdfService)
		{
			_logger = logger;               // Збереження переданих залежностей, що їх
			_hashService = hashService;     // передає контейнер при створенні контролера
			_dataContext = dataContext;
			_dataAccessor = dataAccessor;
			_kdfService = kdfService;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Intro()
		{
			return View();
		}
		public IActionResult AboutRazor()
		{
			Models.Home.AboutRazor.AboutRazorPageModel aboutRazorModel = new()
			{
				TabHeader = "Razor",
				PageTitle = "Синтаксис Razor",
				RazorIs = "технологія включення до складу HTML розмітки\r\n\tзасобів мови програмування C#.",
				RazorInstrc = "Інструкції - узагальнені засоби мови програмування, які можуть бути\r\n\tяк виразами " +
				"(результати яких не потрібно виводити),\r\n\tтак і безрезультатними літералами.\r\n\tRazor-iнструкції вміщують у фігурні дужки " +
				"&commat;{...}\r\n\tВ середині дужок можуть бути довільні інструкції мовою С#.\r\n\tРекомендується вживати лише ті інструкції, які" +
				" обробляють передані\r\n\tдо представлення дані і не " +
				"використовують сервіси та інші засоби\r\n\tпроєкту.",
			};
			return View(aboutRazorModel);
		}
		// Модель форми зазначається параметром методу, заповнення-автоматичне
		public IActionResult Model(Models.Home.Model.FormModel? formModel)
		{
			// Модель представлення створюється і заповнюється самостiйно
			Models.Home.Model.PageModel pageModel = new()
			{
				TabHeader = "Моделi",
				PageTitle = "Моделi в ASP",
				FormModel = formModel
			};
			// модель представлення передається аргументом View()
			return View(pageModel);
		}

		[HttpPost]      //атрибут дозволяє запит лише POST-методом
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

		public IActionResult Data()
		{
			Models.Home.Data.DataPageModel dataPageModel = new()
			{
				TabHeader = "Робота з БД",
				PageTitle = "Робота з даними. Пiдключення БД",
				NuGetPackages = new List<String>
				{
					"Microsoft.EntityFrameworkCore - ядро фреймворку, основні засоби",
					"Microsoft. EntityFrameworkCore.Tools - інструменти управління міграціями",
					"Драйвер БД: у залежності від СУБД -\r\n\t\tдля MSSQL: " +
					"Microsoft.EntityFrameworkCore.SqlServer\r\n\t\tMySQL: Pomelo.EntityFrameworkCore.MySql"
				},
				DataStruct = new List<String> 
				{
					"Створюємо в корені проєкту папку Data, у ній - клас DataContext",
					"Реалізуємо рядок підключення до БД. MSSQL може створювати БД,\r\n\t\tвідповідно можна створити рядок до поки що неіснуючої БД.\r\n\t\tMySQL - краще створити БД, але залишити порожньою. Рядки підключення\r\n\t\tвміщують до appsettings.json у спеціальну секцію \"ConnectionStrings\"",
					"Створюємо папку Data/Entities, у ній клас - User",
					"Клас DataContext успадковується від класу DbContext. У класі DataContext потрібно перевизначити методи \r\n\t\tOnConfiguring() і OnModelCreating(), а його конструкторі необхідно передати DbContextOptions через base(options).",
					"У файлі Program.cs необхідно додати контекст даних за допомогою builder.Services.AddDbContext<DataContext>\r\n\t\t\t\t\t(options =>\r\n\t\t\t\t\toptions.UseSqlServer(\r\n\t\t\t\t\tbuilder.Configuration.GetConnectionString(\"LocalMSSQL\")));",
					"Потім можна використовувати команду add-migration 'название миграции' у терміналі для створення міграції.",
					"Після створення міграції її можна застосувати до бази даних за допомогою команди update-database."
				}
			};
			ViewData["users-count"] = _dataContext.Users.Count();
			return View(dataPageModel);
		}
		public IActionResult Ioc(String? format)  // Inversion of Control
		{
			// користуємось інжектованим сервісом
			// ViewData["hash"] = 
			// ViewData - спеціальний об'єкт для передачі даних
			// до представлення. Його ключі на кшталт ["hash"]
			// можна створювати з довільними назвами
			IocPageModel pageModel = new()
			{
				TabHeader = "IoC",
				PageTitle = "Інверсiя управління. Сервіси. ",
				SingleHash = _hashService.Digest("123"),
				IoCIs = "IoC (Inversion of Control, Інверсія управління) - архітектурний шаблон," +
				"\r\n\tзгідно з яким задачі управління життєвим циклом об'єктів перекладаються" +
				"\r\n\tна спеціальний модуль (інжектор, контейнер залежностей, провайдер)." +
				"\r\n\tЖиттєвий цикл об'єкта: CRUD. Практично це означає, що замість операторів" +
				"\r\n\tnew / delete будуть відповідні звернення до контейнеру.",
				IoCOptions = new List<String>
				{
					"Створення сервісу - класу, що надає необхідну функціональність. ",
					"Реєстрація всіх сервісів у контейнері (інжекторі)",
					"Інжекція сервісів у інші об'єкти, яким вони потрібні"
				},
				HashExm = new List<String>
				{
					"(одноразово) Створюємо папку Services у корені проєкту. ",
					"Оскільки сервіс - це щонайменше два файли (клас та інтерфейс),\r\n\t\tдля кожного сервісу також створюються папки (Hash)",
					"Створюємо інтерфейс IHashService та клас Md5HashService ",
					"Pеєструємо сервіс (див. Program.cs, рядок 8 і далі)",
					"Інжектуємо сервіс (див. HomeController)",
					$"Перевіряємо його роботу: {_hashService.Digest("123")}",
					"Iмітуємо задачу: необхідно перейти на інший геш-алгоритм SHA",
					"OCP (3 SOLID) \"доповнюй, але не змінюй\" -- створюємо новий\r\n\t\tклас ShaHashService у папці Services/Hash"
				},
				Title = "Декілька випадкових дайджестів:",
			};
			for (int i = 0; i < 5; i++)
			{
				String str = (i + 100500).ToString();
				pageModel.Hashes[str] = _hashService.Digest(str);
			}
			if (format == "json")
				return Json(pageModel);
			return View(pageModel);
		}
		public IActionResult URLStruct()
		{
			Models.Home.URLStruct.URLStructPageModel uRLStructPage = new()
			{
				TabHeader = "URL",
				PageTitle = "Структура URL",
				PageText = new List<String>
				{
                    "Протокол: This is the designation of the protocol that is used to access the resource. \r\n        For example, http://, https://, ftp://, mailto:, etc..",
                    "Домен: This is the part of the URL that indicates the address of the server on which the resource is hosted. \r\n        For example, www.example.com.",
                    "Путь: This points to a specific path to a resource on the server. This is the part of the URL after the domain. \r\n        For example, /path/to/resource.",
                    "Строка запроса: These are the parameters passed in the URL for a request to a resource. \r\n        They begin with a question symbol ? and can contain key-value pairs separated by the ampersand & character. For example, ?key1=value1&key2=value2.",
                    "Фрагмент: This is a specific part of the resource that you need to go to or scroll to after the page has loaded. \r\n        The fragment begins with a hash symbol #. For example, #section1."
                },
                PageImageSrc = "/img/url.jpg"
            };
			return View(uRLStructPage);
		}

        public IActionResult Privacy()
        {
            Models.Home.Privacy.PrivacyPageModel privacyPage = new()
            {
                TabHeader = "Privacy",
                PageTitle = "Privacy Policy",
                PageText = "Use this page to detail your site's privacy policy."
            };
            return View(privacyPage);
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
					String salt = RandomStringService.GenerateSalt(10);
					_dataAccessor.UserDao.Signup(new()
					{
						Name = formModel.UserName,
						Email = formModel.UserEmail,
						Birthdate = formModel.UserBirthdate,
						AvatarUrl = formModel.SavedAvatarFilename,
						Salt = salt,
						Derivedkey = _kdfService.DerivedKey(salt, formModel.Password)
					});
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
					// сохраняем файл в wwwroot/img/avatars с новыми именами
					// (переданные имена не рекомендовано оставлять)
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
