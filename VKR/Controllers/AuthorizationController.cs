using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    public class AuthorizationController : UserBasedController
    {
        /// <summary>
        /// Реализация авторизации
        /// </summary>
        /// <returns>Страница авторизации</returns>
        [HttpGet]
        public ActionResult Enter()
        {
            if (HttpContext.Request.Params["id"] == "false")
                ViewBag.isError = true;
            else
                ViewBag.isError = false;
            return View();
        }

        /// <summary>
        /// Авторизация пользователем
        /// </summary>
        /// <returns>Страница "О точке питания"</returns>
        [HttpPost]
        public ActionResult Authorize()
        {
            //Считываем данные из формы
            string Login = HttpContext.Request.Form["Login"];
            string Password = HttpContext.Request.Form["Password"];

            using (var db = new Contexts())
            {
                User user = db.Users.Where(c => c.Login == Login).FirstOrDefault();
                ViewBag.User = user;
                if (ViewBag.User == null || ViewBag.User.Password != Password)
                {
                    ViewBag.isError = true;
                    return Redirect("../Authorization/Enter?id=false");
                }
                else
                {
                    //Добавляем данные о пользователе в куки
                    HttpCookie token = new HttpCookie("user_token", user.UserID.ToString());
                    HttpContext.Response.Cookies.Add(token);

                    // если пользователь прошел проверку, редиректим его не главную страницу
                    return Redirect("../Admin/Home?UserId=" + ViewBag.User.UserID);
                }
            }
        }

        /// <summary>
        /// Метод, обрабатывающий выход из аккаунта
        /// </summary>
        /// <returns>Переадресация на главную страницу КОСТЫЛЬ</returns>
        [HttpGet]
        public ActionResult LogOut()
        {
            HttpCookie token = new HttpCookie("user_token", "");
            HttpContext.Response.Cookies.Add(token);

            //Переадресация на главную страницу КОСТЫЛЬ
            return Redirect("../Admin/Home");
        }
    }
}