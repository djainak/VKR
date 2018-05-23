using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    public class RegistrationController : Controller
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <returns>Форма регистрации пользователя</returns>
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// Обработка формы регистрации нового пользователя
        /// </summary>
        /// <returns>Переадресация на личный кабинет (!!!ПОКА НА АДМИНКУ)</returns>
        [HttpPost]
        public ActionResult NewUser()
        {
            User user = new User();
            user.Login = HttpContext.Request.Form["Login"];
            user.Password = HttpContext.Request.Form["Password"];
            user.Email = HttpContext.Request.Form["Email"];
            user.FirstName = HttpContext.Request.Form["FirstName"];
            user.Name = HttpContext.Request.Form["Name"];
            user.Patronymic = HttpContext.Request.Form["Patronymic"];
            user.PhoneNumber = HttpContext.Request.Form["PhoneNumber"];
            user.Status = 0;

            using (var db = new Contexts())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            //В дальнейшем сделать переадресацию на личный кабинет, сейчас переадресация в админку
            return Redirect("../Admin/Home");
        }
    }
}