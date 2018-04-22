using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    /// <summary>
    /// Контроллер, обрабатывающий клиентскую часть приложения
    /// </summary>
    public class ClientController : Controller
    {
        /// <summary>
        /// Обрабатывает стартовую страницу с меню
        /// </summary>
        /// <returns>Стартовая страница с меню</returns>
        [HttpGet]
        public ActionResult MenuPage()
        {
            using (var db = new Contexts())
            {
                ViewBag.Category = db.CategoryMenuItem.ToList();
            }
            return View();
        }
    }
}