using ASP.Data;
using ASP.Data.DAL;
using ASP.Models;
using ASP.Models.FrontendForm;
using ASP.Models.Home.Ioc;
using ASP.Models.Home.Signup;
using ASP.Services.Hash;
using ASP.Services.Kdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;

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
            return View();
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
                Message = $"{input.UserName} -- {input.UserEmail}"
            };
            _logger.LogInformation(output.Message);
            return Json(output);
        }

        public IActionResult Data()
        {
            ViewData["users-count"] = _dataContext.Users.Count();
            return View();
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
                Title = "Декілька випадкових дайджестів:",
                SingleHash = _hashService.Digest("123")
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
            return View();
        }
        public IActionResult Signup(SignupFormModel? formModel)
        {
            SignupPageModel pageModel = new()
            {
                FormModel = formModel
            };
            if (formModel?.HasData ?? false)
            {
                pageModel.ValidationsErrors = _ValidateSignupModel(formModel);
                if(pageModel.ValidationsErrors.Count == 0)
                {
                    String salt = RandomStringService.GenerateSalt(8);
                    _dataAccessor.UserDao.Signup(new()
                    {
                        Name = formModel.UserName,
                        Email = formModel.UserEmail,
                        Birthdate = formModel.UserBirthdate,
                        AvatarUrl = formModel.SavedAvataFilename,
                        Salt = salt,
                        Derivedkey = _kdfService.DerivedKey(salt, formModel.Password),
                    });
                }
            }
            //_logger.LogInformation(Directory.GetCurrentDirectory());
            return View(pageModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private Dictionary<string, string> _ValidateSignupModel(SignupFormModel? model)
        {
            Dictionary<string, string> result = new();
            if (model == null)
            {
                result["model"] = "Model is null";
            }
            else
            {
                if (String.IsNullOrEmpty(model.UserName))
                {
                    result[nameof(model.UserName)] = "UserName cannot be empty";
                }
                if (String.IsNullOrEmpty(model.UserEmail))
                {
                    result[nameof(model.UserEmail)] = "UserEmail cannot be empty";
                }
                if (model.UserBirthdate == default(DateTime))
                {
                    result[nameof(model.UserBirthdate)] = "UserBirthdate cannot be empty";
                }
                if (model.UserAvatar == null)
                {
                    result[nameof(model.UserAvatar)] = "UserAvatar cannot be empty";
                }
                if (model.UserAvatar != null)
                {
                    int dotPosition = model.UserAvatar.FileName.LastIndexOf('.');
                    if (dotPosition == -1)
                    {
                        result[nameof(model.UserAvatar)] = "File without extension not allowed";
                    }
                    String ext = model.UserAvatar.FileName[dotPosition..];
                    // _logger.LogInformation(ext);
                    String path = Directory.GetCurrentDirectory() + "/wwwroot/img/Avatars/";
                    _logger.LogInformation(path);
                    String fileName;
                    String pathName;
                    string randomFileName = RandomStringService.GenerateFilename(10);
					do
					{
                        fileName = randomFileName + ext;
                        pathName = path + fileName;

                    }
                    while (System.IO.File.Exists(pathName));
                    using var stream = System.IO.File.OpenWrite(pathName);
                    model.UserAvatar.CopyTo(stream);
                    model.SavedAvataFilename = fileName;
                }
                
            }
			return result;
		}
    }
}
