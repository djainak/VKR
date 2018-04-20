using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    public class AdminController : UserBasedController
    {

        /// <summary>
        /// Страница с данними о точке питания
        /// </summary>
        /// <returns>Страница с данными о точке питания</returns>
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

        /// <summary>
        /// Список доступных меню
        /// </summary>
        /// <returns>Страница со списком меню</returns>
        [HttpGet]
        public ActionResult MenuList()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            using (var db = new Contexts())
            {
                List<Menu> menues = db.Menues.ToList();
                ViewBag.Menues = menues;
                foreach(Menu menu in db.Menues)
                {
                    if (menu.Status)
                    {
                        @ViewBag.NameMenu = menu.Name;
                    }
                }
                if (@ViewBag.NameMenu == null)
                {
                    db.Menues.FirstOrDefault().Status = true;
                    @ViewBag.NameMenu = db.Menues.FirstOrDefault().Name;
                }
            }

            return View();
        }

        /// <summary>
        /// Добавление нового меню
        /// </summary>
        /// <returns>Страница с формой добавления нового меню</returns>
        [HttpGet]
        public ActionResult AddNewMenu()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            return View();
        }

        /// <summary>
        /// Добавление нового меню
        /// </summary>
        /// <returns>Переадресация на страницу со списком пунктов нового меню</returns>
        [HttpPost]
        public ActionResult AddMenu()
        {
            //Считываем данные из формы
            string name = HttpContext.Request.Form["Name"];
            string description = HttpContext.Request.Form["Description"];
            bool IsMainMenu = Convert.ToBoolean(HttpContext.Request.Form["UseThisMenu"]);
            bool flag = false;

            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            //Создаем объект Меню и заполняем данными из формы
            Menu menu = new Menu();
            menu.Name = name;
            menu.Description = description;

            //Добавляем новое меню в БД
            using (var db = new Contexts())
            {
                if (IsMainMenu)
                {
                    foreach (Menu m in db.Menues)
                    {
                        m.Status = false;
                    }
                    menu.Status = true;
                    db.Menues.Add(menu);
                    db.SaveChanges();
                }
                else
                {
                    menu.Status = false;
                    db.Menues.Add(menu);
                    db.SaveChanges();
                    //Проверка, есть ли у нас тогда выбранное основное меню
                    foreach (Menu m in db.Menues)
                    {
                        if (m.Status)
                            flag = true;
                    }
                    if (!flag)
                    {
                        db.Menues.FirstOrDefault().Status = true;
                        db.SaveChanges();
                    }
                }
            }
            return Redirect("./AddMenuItemForm?MenuId=" + menu.Id + "&UserId=" + ViewBag.UserID);
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
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            ViewBag.MenuId = id;
            return View();
        }

        /// <summary>
        /// Список пунктов данного меню
        /// </summary>
        /// <returns>Страница со списком пунктов меню</returns>
        [HttpGet]
        public ActionResult AddMenuItemForm()
        {
            //Вытаскиваем ид из адреса
            int id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

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

            return Redirect("./AddMenuItemForm?MenuId=" + m_id + "&UserId=" + ViewBag.UserID);
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

        /// <summary>
        /// Создаем новый пункт меню
        /// </summary>
        /// <returns>Переадресация к списку пунктов данного меню</returns>
        [HttpPost]
        public ActionResult NewMenuItem()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            //Считываем данные из формы
            int menuId = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            string name = HttpContext.Request.Form["Name"];
            string category = HttpContext.Request.Form["Category"];
            string ingredients = HttpContext.Request.Form["Ingredients"];
            int price = Convert.ToInt32(HttpContext.Request.Form["Price"]);
            

            //Создаем объект пункта Меню и заполняем данными из формы
            MenuItem menuItem = new MenuItem();
            menuItem.MenuId = menuId;
            menuItem.Name = name;
            menuItem.Ingredients = ingredients;
            menuItem.Price = price;
            //menuItem.Category = category;

            //Добавляем новый пункт меню в БД
            using (var db = new Contexts())
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
            }
            return Redirect("./AddMenuItemForm?MenuId=" + menuId + "&UserId=" + ViewBag.UserID);
        }

        /// <summary>
        /// Форма для редактирования меню
        /// </summary>
        /// <returns>Страница с формой редактирования меню</returns>
        [HttpGet]
        public ActionResult ChangeMenu()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            int menuId = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            //Добавляем новое меню в БД
            using (var db = new Contexts())
            {
                ViewBag.Menu = db.Menues.Find(menuId);
            }
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий изменение меню
        /// </summary>
        /// <returns>Перенаправляет на страницу со списком меню</returns>
        [HttpPost]
        public ActionResult ChMenu()
        {
            //Считываем данные из формы
            string name = HttpContext.Request.Form["Name"];
            string description = HttpContext.Request.Form["Description"];
            bool IsMainMenu = Convert.ToBoolean(HttpContext.Request.Form["UseThisMenu"]);
            bool flag = false;

            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            int m_id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            ViewBag.MenuId = m_id;

            //Добавляем новое меню в БД
            using (var db = new Contexts())
            {
                db.Menues.Find(m_id).Name = name;
                db.Menues.Find(m_id).Description = description;
                if (IsMainMenu)
                {
                    foreach(Menu menu in db.Menues)
                    {
                        menu.Status = false;
                    }
                    db.Menues.Find(m_id).Status = true;
                    db.SaveChanges();
                }
                else
                {
                    db.Menues.Find(m_id).Status = false;
                    db.SaveChanges();
                    //Проверка, есть ли у нас тогда выбранное основное меню
                    foreach (Menu m in db.Menues)
                    {
                        if (m.Status)
                            flag = true;
                    }
                    if (!flag)
                    {
                        db.Menues.FirstOrDefault().Status = true;
                        db.SaveChanges();
                    }
                }  
            }
            return Redirect("./MenuList?UserId=" + ViewBag.UserID);
        }

        /// <summary>
        /// Редактирование пункта меню
        /// </summary>
        /// <returns>Форма редактирования пункта меню</returns>
        [HttpGet]
        public ActionResult ChangeMenuItem()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            ViewBag.MenuId = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            ViewBag.MenuItemId = Convert.ToInt32(HttpContext.Request.Params["MenuItemId"]);

            using (var db = new Contexts())
            {
                ViewBag.MenuItem = db.MenuItems.Find(ViewBag.MenuItemId);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChMenuItem()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            ViewBag.MenuId = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            int menuItemId = Convert.ToInt32(HttpContext.Request.Params["MenuItemId"]);

            //Изменяем пункт меню в БД
            using (var db = new Contexts())
            {
                db.MenuItems.Find(menuItemId).Name = HttpContext.Request.Form["Name"];
                db.MenuItems.Find(menuItemId).Ingredients = HttpContext.Request.Form["Ingredients"];
                //db.MenuItems.Find(menuItemId).Category = HttpContext.Request.Form["Category"];
                db.MenuItems.Find(menuItemId).Price = Convert.ToInt32(HttpContext.Request.Form["Price"]);
                db.SaveChanges();
            }
            return Redirect("./AddMenuItemForm?MenuId=" + ViewBag.MenuId + "&UserId=" + ViewBag.UserID);
        }

        /// <summary>
        /// Страница с личными данными авторизированного пользователя
        /// </summary>
        /// <returns>Страницу с личными данными</returns>
        [HttpGet]
        public ActionResult PersonalAccount()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            using (var db = new Contexts())
            {
                ViewBag.User = db.Users.Find(Convert.ToInt32(HttpContext.Request.Params["UserId"]));
            }

            return View();
        }
    }
}
