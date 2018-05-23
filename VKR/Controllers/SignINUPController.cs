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
        /// Проверяет занятость логина
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <returns>1 - занят, 0 - свободен</returns>
        public string Get(string Login)
        {
            string login = Login;
            using (var db = new Contexts())
            {
                if (db.Users.Where(c => c.Login == Login).FirstOrDefault() == null)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }
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
                cookie["user_token"].Value = "";
            }
        }
    }
}
