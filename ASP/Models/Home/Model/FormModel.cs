using Microsoft.AspNetCore.Mvc;

namespace ASP.Models.Home.Model
{
	/* Модель форми - відображення даних, що приходять з HTML-форми,
	 а також узгодження імен, прийнятних у HTML (user-name), до
	 імен, традиційних для платформи (UserName) */
	public class FormModel
	{
		[FromForm(Name ="user-name")]
		public String UserName { get; set; } = null!;


		[FromForm(Name = "user-email")]
		public String UserEmail { get; set; } = null!;
	}
}
/*
 HTТP-запит

POST /Home/Index HTTP/1.1				| POST - метод запиту
Host: Localhost							| /Home/Index - Path
Connection: close						| НТТР/1.1 - протокол
Content-Type: application/json			| Заголовки - по одному в рядку
										| Порожній рядок - кінець заголовків
{"name":"User Name" ... }				| Тіло запиту - усе до кінця пакету
										|
Метод запиту - перше слово у пакеті. Є стандартні
GET, POST, PUT, DELETE, PATCH,		HEAD, OPTIONS, CONNECT
є промислово-стандартні
LINK, UNLINK, PURGE, ...
є інші - як правило сервер сприймає довільні "слова"



НТТР-відповідь

HTTP/1.1 200 OK							 | 200 - status code
Connection: close						 | OK - reason phrase
Content-Type: text/html					 | 
										 |
<! doctype html>.....					 |
 */