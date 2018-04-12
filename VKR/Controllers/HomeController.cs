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

    /// <summary>
    /// Класс авторизации
    /// </summary>
    public class AuthorizationController : Controller
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
                ViewBag.User = db.Users.Where(c => c.Login == Login && c.Password == Password).FirstOrDefault();
                if (ViewBag.User == null)
                {
                    ViewBag.isError = true;
                    return Redirect("../Authorization/Enter?id=false" );
                }
            }

            return Redirect("../Admin/Home?UserId=" + ViewBag.User.UserID);
        }
    }

    /// <summary>
    /// Класс админской части
    /// </summary>
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Home()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            using (var db = new Contexts())
            {
                ViewBag.DinningRoom = db.DinningRooms.FirstOrDefault();
                ViewBag.Manager = db.Users.Where(c => c == db.DinningRooms.FirstOrDefault().Manager).FirstOrDefault();
            }

            return View();
        }

        /// <summary>
        /// Редактирование страницы о точке питания
        /// </summary>
        /// <returns>Форма редактирования</returns>
        [HttpGet]
        public ActionResult SetDinningRoom()
        {
            using (var db = new Contexts())
            {
                ViewBag.DinningRoom = db.DinningRooms.FirstOrDefault();
                ViewBag.Manager = db.Users.Where(c => c == db.DinningRooms.FirstOrDefault().Manager).FirstOrDefault();
            }

            return View();
        }

        /// <summary>
        /// Обработка изменения настроек точки питания
        /// </summary>
        /// <returns>Возвращает на страницу с информацией о точке питания</returns>
        [HttpPost]
        public ActionResult SetDR()
        {
            //Считываем данные из формы
            string Adress = HttpContext.Request.Form["Adress"];
            string Phone = HttpContext.Request.Form["Phone"];
            string Email = HttpContext.Request.Form["Email"];
            string login = HttpContext.Request.Form["Login"];
            int Dishes = Convert.ToInt32(HttpContext.Request.Form["Dishes"]);
            int Interval = Convert.ToInt32(HttpContext.Request.Form["Interval"]);

            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            //Добавляем новый пункт меню в БД
            using (var db = new Contexts())
            {
                db.DinningRooms.FirstOrDefault().Adress = Adress;
                db.DinningRooms.FirstOrDefault().PhoneNum = Phone;
                db.DinningRooms.FirstOrDefault().Email = Email;
                db.DinningRooms.FirstOrDefault().Dishes = Dishes;
                db.DinningRooms.FirstOrDefault().Interval = Interval;
                db.DinningRooms.FirstOrDefault().Manager = db.Users.Where(c => c.Login == login).FirstOrDefault();
                db.SaveChanges();
            }

            return Redirect("../Admin/Home?UserId=" + ViewBag.UserID);
        }

        [HttpGet]
        public ActionResult PersonalAccount()
        {
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

            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

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
            return Redirect("./AddMenuItemForm?menuId=" + menu.Id + "&UserId=" + ViewBag.UserID);
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
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            int id = Convert.ToInt32(HttpContext.Request.Params["id"]);
            int m_id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            using (var db = new Contexts())
            {
                MenuItem menuItem = db.MenuItems.Find(id);
                db.MenuItems.Remove(menuItem);
                db.SaveChanges();
            }

            return Redirect("./AddMenuItemForm?menuId=" + m_id + "&UserId=" + ViewBag.UserID);
        }

        /// <summary>
        /// Реализация удаления меню из БД
        /// </summary>
        /// <returns>Страничка без удаленного меню</returns>
        [HttpPost]
        public ActionResult RemoveMenu()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            int id = Convert.ToInt32(HttpContext.Request.Params["id"]);
            using (var db = new Contexts())
            {
                Menu menu = db.Menues.Find(id);
                db.Menues.Remove(menu);
                db.SaveChanges();
            }
            return Redirect("./MenuList?UserId=" + ViewBag.UserID);
        }

        [HttpPost]
        public ActionResult NewMenuItem()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

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
            return Redirect("./AddMenuItemForm?menuId=" + menuId + "&UserId=" + ViewBag.UserID);
            //return View();
        }
    }
}
