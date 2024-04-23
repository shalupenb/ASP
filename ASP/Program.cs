using ASP.Data;
using ASP.Data.DAL;
using ASP.Middleware;
using ASP.Services.Hash;
using ASP.Services.Kdf;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
/* Додаємо власні сервіси до контейнера builder.Services
* Це можна робити у довільному порядку, але до команди
* var app = builder. Build();
* Сервіси, створені з дотриманням DIP реєструються як
* зв'язка (binding) між інтерфейсом та класом, що його
* реалізує. Інструкцію можна пояснити як "на запит
* інжекції IHashService контейнер має повернути об'єкт
* класу Md5HashService"
*/
// builder.Services. AddSingleton<IHashService, Md5HashService>();
// перехід між різними реалізаціями одного сервісу - один рядок змін
builder.Services.AddSingleton<IHashService, ShaHashService>();


// Реєстрація контексту даних (MSSQL)
builder.Services.AddDbContext<DataContext>(
	options => options.UseSqlServer(
		builder.Configuration.GetConnectionString("LocalMSSQL")),
	ServiceLifetime.Singleton);

builder.Services.AddSingleton<DataAccessor>();
builder.Services.AddSingleton<IKdfService, Pbkdf1Service>();


// Налаштування Http-сесiй
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(10);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(builder => builder
				.AllowAnyMethod()
				.AllowAnyHeader()
				.SetIsOriginAllowed(origin => true) // allow any origin
				.AllowCredentials());

app.UseAuthorization();

// Підключення Http-сесiй
app.UseSession();

// Підключення нашого Middleware
//app.UseMiddleware<AuthSessionMiddleware>();
app.UseAuthSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
