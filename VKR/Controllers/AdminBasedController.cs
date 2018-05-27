using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    public class AdminBasedController : Controller
    {
        protected override void ExecuteCore()
        {
            base.ExecuteCore();
        }

        /// <summary>
        /// Используется для получение корневого URL сервера
        /// </summary>
        public class URLInfo
        {
            public static string getHostUrl(HttpContextBase context)
            {
                string hostName = context.Request.Url.AbsoluteUri.Replace(context.Request.Url.PathAndQuery, "");
                string server_root_url = hostName + "/";
                if (!server_root_url.StartsWith("http://") && server_root_url.StartsWith("http:"))
                    server_root_url = server_root_url.Replace("http:", "http://");
                if (!server_root_url.StartsWith("https://") && server_root_url.StartsWith("http:"))
                    server_root_url = server_root_url.Replace("https:", "https://");

                return server_root_url;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Получаем корневую часть URL
            string server_root_url = URLInfo.getHostUrl(filterContext.HttpContext);

            // проверяем, не является ли  action тем, к которому пользователь имеет доступ в любом случае
            if (filterContext.ActionDescriptor.ActionName != "enter" &&
                filterContext.ActionDescriptor.ActionName != "signup" &&
                filterContext.ActionDescriptor.ActionName != "enter_admin")
            {
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
                    filterContext.Result = Redirect(server_root_url + "Authorization/Enter_Admin");
                }
                else
                {
                    using (var db = new Contexts())
                    {
                        if (db.Users.Find(Convert.ToInt32(token)).Status == 0)
                            filterContext.Result = Redirect(server_root_url + "Authorization/Enter_Admin");
                    }
                }
            }
        }
    }
}