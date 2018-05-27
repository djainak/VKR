using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    /// <summary>
    /// Контроллер, отвечающий за авторизацию пользователя
    /// </summary>
    public class AuthorizationController : Controller
    {
        /// <summary>
        /// Реализация авторизации клиента
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
        /// Реализация авторизации панели администратора
        /// </summary>
        /// <returns>Страница авторизации</returns>
        [HttpGet]
        public ActionResult Enter_Admin()
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
        /// <returns>Страница с клиентским меню</returns>
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
                    return Redirect("../Client/MenuPage");
                }
            }
        }

        /// <summary>
        /// Авторизация пользователем администраторской части приложения
        /// </summary>
        /// <returns>Страница "О точке питания"</returns>
        [HttpPost]
        public ActionResult Authorize_Admin()
        {
            //Считываем данные из формы
            string Login = HttpContext.Request.Form["Login"];
            string Password = HttpContext.Request.Form["Password"];

            using (var db = new Contexts())
            {
                User user = db.Users.Where(c => c.Login == Login).FirstOrDefault();
                ViewBag.User = user;
                if (ViewBag.User == null || ViewBag.User.Password != Password || user.Status == 0)
                {
                    ViewBag.isError = true;
                    return Redirect("../Authorization/Enter_Admin?id=false");
                }
                else
                {
                    //Добавляем данные о пользователе в куки
                    HttpCookie token = new HttpCookie("user_token", user.UserID.ToString());
                    HttpContext.Response.Cookies.Add(token);

                    // если пользователь прошел проверку, редиректим его нa главную страницу
                    return Redirect("../Admin/Home");
                }
            }
        }

        /// <summary>
        /// Метод, реализующий выход из аккаунта клиентской части приложения
        /// </summary>
        /// <returns>Перенаправление на страницу авторизации клиентской части приложения</returns>
        [HttpGet]
        public ActionResult LogOut()
        {
            HttpCookie token = new HttpCookie("user_token", "");
            HttpContext.Response.Cookies.Add(token);
            return Redirect("../Authorization/Enter");
        }

        /// <summary>
        /// Метод, реализующий выход из аккаунта администраторской части приложения
        /// </summary>
        /// <returns>Перенаправление на страницу авторизации администраторской части приложения</returns>
        [HttpGet]
        public ActionResult LogOut_Admin()
        {
            HttpCookie token = new HttpCookie("user_token", "");
            HttpContext.Response.Cookies.Add(token);
            return Redirect("../Authorization/Enter_Admin");
        }
    }
}