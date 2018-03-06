using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using VKR.Models;

namespace VKR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //Console.WriteLine();

            return View();
        }
    }

    public class AddMenuItemController : Controller
    {
        [HttpPost]
        public ActionResult AddMenuItem()
        {
            //Считываем данные из формы
            int id = Convert.ToInt32(HttpContext.Request.Form["MenuId"]);
            string name = HttpContext.Request.Form["Name"];
            string ingredients = HttpContext.Request.Form["Ingredients"];
            int price = Convert.ToInt32(HttpContext.Request.Form["Price"]);

            //Создаем объект пункта Меню и заполняем данными из формы
            MenuItem menuItem = new MenuItem();
            menuItem.Id = id;
            menuItem.Name = name;
            menuItem.Ingredients = ingredients;
            menuItem.Price = price;

            //Добавляем новый пункт меню в БД
            using (var db = new Contexts())
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
            }
            return View();
        }

    }
}
