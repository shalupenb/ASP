using ASP.Data;
using ASP.Data.DAL;
using ASP.Middleware;
using ASP.Services.Hash;
using ASP.Services.Kdf;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
/* ������ ����� ������ �� ���������� builder.Services
* �� ����� ������ � ��������� �������, ��� �� �������
* var app = builder. Build();
* ������, ������� � ����������� DIP ����������� ��
* ��'���� (binding) �� ����������� �� ������, �� ����
* ������. ���������� ����� �������� �� "�� �����
* �������� IHashService ��������� �� ��������� ��'���
* ����� Md5HashService"
*/
// builder.Services. AddSingleton<IHashService, Md5HashService>();
// ������� �� ������ ����������� ������ ������ - ���� ����� ���
builder.Services.AddSingleton<IHashService, ShaHashService>();


// ��������� ��������� ����� (MSSQL)
builder.Services.AddDbContext<DataContext>(
	options => options.UseSqlServer(
		builder.Configuration.GetConnectionString("LocalMSSQL")),
	ServiceLifetime.Singleton);

builder.Services.AddSingleton<DataAccessor>();
builder.Services.AddSingleton<IKdfService, Pbkdf1Service>();


// ������������ Http-���i�
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

// ϳ��������� Http-���i�
app.UseSession();

// ϳ��������� ������ Middleware
//app.UseMiddleware<AuthSessionMiddleware>();
app.UseAuthSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
