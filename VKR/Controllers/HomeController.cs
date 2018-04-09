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

            return View();
        }

    }
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Home()
        {
            ViewBag.Title = "О точке";

            return View();
        }

        [HttpGet]
        public ActionResult MenuList()
        {
            using (var db = new Contexts())
            {
                List<Menu> menues = db.Menues.ToList();
                ViewBag.Menues = menues;
            }

            return View();
        }

        [HttpGet]
        public ActionResult AddNewMenu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMenu()
        {
            //Считываем данные из формы
            string name = HttpContext.Request.Form["Name"];
            string description = HttpContext.Request.Form["Description"];

            //Создаем объект Меню и заполняем данными из формы
            Menu menu = new Menu();
            menu.Name = name;
            menu.Description = description;

            //Добавляем новое меню в БД
            using (var db = new Contexts())
            {
                db.Menues.Add(menu);
                db.SaveChanges();
            }
            return Redirect("./AddMenuItemForm?menuId=" + menu.Id);
        }

        /// <summary>
        /// Контроллер, обрабатывающий страницу с формой для добавления нового блюда в меню.
        /// </summary>
        /// <returns>Страницу с формой добавления пункта меню.</returns>
        [HttpGet]
        public ActionResult ListMenuItems()
        {
            //Вытаскиваем ид из адреса
            int id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            ViewBag.MenuId = id;
            return View();
        }

        [HttpGet]
        public ActionResult AddMenuItemForm()
        {
            //Вытаскиваем ид из адреса
            int id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);

            ViewBag.isError = false;
            //Обращаемся к БД
            using (var db = new Contexts())
            {
                //Ищем там меню по ид
                Menu menu = db.Menues.Where(ent => ent.Id == id).FirstOrDefault();
                if (menu == null)
                {
                    ViewBag.isError = true;
                }
                else
                {
                    ViewBag.MenuId = id;
                    List<MenuItem> menuItems = db.MenuItems.Where(ent => ent.MenuId == id).ToList();
                    ViewBag.MenuItems = menuItems;
                }
            }

            return View();
        }

        /// <summary>
        /// Реализация удаления пункта меню из БД
        /// </summary>
        /// <returns>Страничка без удаленного пункта меню</returns>
        [HttpPost]
        public ActionResult RemoveMenuItem()
        {
            int id = Convert.ToInt32(HttpContext.Request.Params["id"]);
            int m_id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            using (var db = new Contexts())
            {
                MenuItem menuItem = db.MenuItems.Find(id);
                db.MenuItems.Remove(menuItem);
                db.SaveChanges();
            }

            return Redirect("./AddMenuItemForm?menuId=" + m_id);
        }

        /// <summary>
        /// Реализация удаления меню из БД
        /// </summary>
        /// <returns>Страничка без удаленного меню</returns>
        [HttpPost]
        public ActionResult RemoveMenu()
        {
            int id = Convert.ToInt32(HttpContext.Request.Params["id"]);
            using (var db = new Contexts())
            {
                Menu menu = db.Menues.Find(id);
                db.Menues.Remove(menu);
                db.SaveChanges();
            }
            return Redirect("./MenuList");
        }

        [HttpPost]
        public ActionResult NewMenuItem()
        {
            //Считываем данные из формы
            int menuId = Convert.ToInt32(HttpContext.Request.Form["MenuId"]);
            string name = HttpContext.Request.Form["Name"];
            string ingredients = HttpContext.Request.Form["Ingredients"];
            int price = Convert.ToInt32(HttpContext.Request.Form["Price"]);

            //Создаем объект пункта Меню и заполняем данными из формы
            MenuItem menuItem = new MenuItem();
            menuItem.MenuId = menuId;
            menuItem.Name = name;
            menuItem.Ingredients = ingredients;
            menuItem.Price = price;

            //Добавляем новый пункт меню в БД
            using (var db = new Contexts())
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
            }
            return Redirect("./AddMenuItemForm?menuId=" + menuId);
            //return View();
        }
    }
}
