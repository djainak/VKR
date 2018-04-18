﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    public class UserBasedController : Controller
    {
        protected override void ExecuteCore()
        {
            base.ExecuteCore();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Получаем корневую часть URL
            string server_root_url = filterContext.HttpContext.Request.Url.AbsoluteUri.Replace(filterContext.HttpContext.Request.Url.PathAndQuery, "");

            // проверяем, не является ли наш action тем, к которому пользователь имеет доступ в любом случае
            if (filterContext.ActionDescriptor.ActionName != "Enter" &&
                filterContext.ActionDescriptor.ActionName != "Authorize")
            {
                // тянем токен из мешочка с печеньками
                string token = "";
                try
                {
                    token = filterContext.HttpContext.Request.Cookies["user_token"].Value;
                }
                catch { }

                // проверяем токен
                // если токен проходит проверку, то позволяем action выполняться дальше, если не прошел
                // тогда отправляем пользователя на страницу логина
                if (token == "" || token == null)
                {
                    using (var db = new Contexts())
                    {
                        if (db.Users.Find(token) == null)
                        {
                            filterContext.Result = Redirect(server_root_url + "Authorization/Enter?id=false");
                        }
                    }
                }
            }
        }
    }
}