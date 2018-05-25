using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using VKR.Models;

namespace VKR.Controllers
{
    /// <summary>
    /// Контроллер, обрабатывающий авторизацию и регистрацию пользователя
    /// </summary>
    public class SignINUPController : ApiController
    {
        /// <summary>
        /// Проверяет занятость логина и Email
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <param name="Email">Email</param>
        /// <returns>Login - занят логин, Email - занят Email, ОК - свободно</returns>
        public string Get(string Login, string Email)
        {
            string login = Login;
            using (var db = new Contexts())
            {
                if (db.Users.Where(c => c.Login == Login.Trim()).FirstOrDefault() != null)
                {
                    return JsonConvert.SerializeObject("Login");
                }
                else if (db.Users.Where(c => c.Email == Email.Trim()).FirstOrDefault() != null)
                {
                    return JsonConvert.SerializeObject("Email");
                }
                else
                    return JsonConvert.SerializeObject("OK");
            }
        }
        /// <summary>
        /// Метод, обрабатывающий выход из аккаунта
        /// </summary>
        public void Get()
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("user_token").FirstOrDefault();
            if (cookie != null)
            {
                cookie["user_token"].Value = string.Empty;
            }
            
        }
    }
}
