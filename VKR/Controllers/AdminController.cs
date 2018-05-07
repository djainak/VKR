using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    /// <summary>
    /// Контроллер, реализующий логику администраторской части сайта
    /// </summary>
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
                ViewBag.WorkDays = db.DayWork.Where(d=> d.IsWorkDay == true)
                    .ToList();
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
                ViewBag.WorkDays = db.DayWork.ToList();
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

                foreach (var day in db.DayWork)
                {
                    int id = Convert.ToInt32(HttpContext.Request.Params[day.DayWorkID.ToString()]);
                    if (id == day.DayWorkID)
                    {
                        day.IsWorkDay = true;
                    }
                    else
                        day.IsWorkDay = false;

                    day.StartDayHour = HttpContext.Request.Params["StDH_" + day.DayWorkID];
                    day.StartDayMin = HttpContext.Request.Params["StDM_" + day.DayWorkID];
                    day.EndDayHour = HttpContext.Request.Params["EDH_" + day.DayWorkID]; 
                    day.EndDayMin = HttpContext.Request.Params["EDM_" + day.DayWorkID];
                }
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
                if (menues.Count != 0)
                {
                    foreach (Menu menu in db.Menues)
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
                else
                    @ViewBag.NameMenu = "Нет доступных меню";
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
        public ActionResult AddMenu(HttpPostedFileBase upload)
        {
            //Считываем данные из формы
            string name = HttpContext.Request.Form["Name"];
            string description = HttpContext.Request.Form["Description"];
            bool IsMainMenu = Convert.ToBoolean(HttpContext.Request.Form["UseThisMenu"]);
            bool flag = false;
            string pic = "NoPhoto.jpg";

            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            //Создаем объект Меню и заполняем данными из формы
            Menu menu = new Menu();
            menu.Name = name;
            menu.Description = description;

            if (upload != null)
            {
                pic = Path.GetFileName(upload.FileName);

                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Content/Style/Files/" + pic));
            }
            menu.Picture = pic;
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
            List<string> a = new List<string>();

            using (var db = new Contexts())
            {
                foreach (var c in db.CategoryMenuItem)
                {
                    a.Add(c.Name);
                }
            }
            ViewBag.Category = a;
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
                    foreach (var c in menuItems)
                    {
                        c.Category = db.CategoryMenuItem.Where(cc =>cc.CategoryMenuItemID == c.CategoryMenuItemId).FirstOrDefault();
                        
                    }
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
        /// Реализация удаления пользователя из БД
        /// </summary>
        /// <returns>Страничка без удаленного пользователя</returns>
        [HttpPost]
        public ActionResult RemoveUser()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            int id = Convert.ToInt32(HttpContext.Request.Params["id"]);
            using (var db = new Contexts())
            {
                User user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return Redirect("./ListUsers?UserId=" + ViewBag.UserID);
        }

        /// <summary>
        /// Создаем новый пункт меню
        /// </summary>
        /// <returns>Переадресация к списку пунктов данного меню</returns>
        [HttpPost]
        public ActionResult NewMenuItem(HttpPostedFileBase upload)
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            //Считываем данные из формы
            int menuId = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            string name = HttpContext.Request.Form["Name"];
            string category = HttpContext.Request.Form["Category"];
            string ingredients = HttpContext.Request.Form["Ingredients"];
            int price = Convert.ToInt32(HttpContext.Request.Form["Price"]);
            string pic = "NoPhoto.jpg"; 
            MenuItem menuItem = new MenuItem();
            
            if (upload != null)
            {
                pic = Path.GetFileName(upload.FileName);

                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Content/Style/Files/" + pic));
            }
            
            //Создаем объект пункта Меню и заполняем данными из формы
            menuItem.MenuId = menuId;
            menuItem.Name = name;
            menuItem.Ingredients = ingredients;
            menuItem.Price = price;
            menuItem.Picture = pic;

            //Добавляем новый пункт меню в БД
            using (var db = new Contexts())
            {
                menuItem.Category = db.CategoryMenuItem.Where(c => c.Name == category).FirstOrDefault();
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
        public ActionResult ChMenu(HttpPostedFileBase upload)
        {
            //Считываем данные из формы
            string name = HttpContext.Request.Form["Name"];
            string description = HttpContext.Request.Form["Description"];
            bool IsMainMenu = Convert.ToBoolean(HttpContext.Request.Form["UseThisMenu"]);
            bool flag = false;
            string pic = "";

            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            int m_id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            ViewBag.MenuId = m_id;

            //Добавляем новое меню в БД
            using (var db = new Contexts())
            {
                db.Menues.Find(m_id).Name = name;
                db.Menues.Find(m_id).Description = description;
                if (upload != null)
                {
                    pic = Path.GetFileName(upload.FileName);

                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Content/Style/Files/" + pic));
                    db.Menues.Find(m_id).Picture = pic;
                }
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
            List<CategoryMenuItem> a = new List<CategoryMenuItem>();

            using (var db = new Contexts())
            {
                ViewBag.MenuItem = db.MenuItems.Find(ViewBag.MenuItemId);
                foreach (var c in db.CategoryMenuItem)
                {
                    a.Add(c);
                }
                ViewBag.Category = a;
            }
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий изменение пункта меню
        /// </summary>
        /// <returns>Перенаправление на страницу со списком блюд меню</returns>
        [HttpPost]
        public ActionResult ChMenuItem(HttpPostedFileBase upload)
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            ViewBag.MenuId = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);
            int menuItemId = Convert.ToInt32(HttpContext.Request.Params["MenuItemId"]);
            string category = HttpContext.Request.Form["Category"];
            string pic = "";

            //Изменяем пункт меню в БД
            using (var db = new Contexts())
            {
                
                if (upload != null)
                {
                    pic = Path.GetFileName(upload.FileName);

                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Content/Style/Files/" + pic));
                    db.MenuItems.Find(menuItemId).Picture = pic;
                }
                db.MenuItems.Find(menuItemId).Name = HttpContext.Request.Form["Name"];
                db.MenuItems.Find(menuItemId).Ingredients = HttpContext.Request.Form["Ingredients"];
                db.MenuItems.Find(menuItemId).Category = db.CategoryMenuItem.Where(c => c.Name == category).FirstOrDefault();
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

        /// <summary>
        /// Вывод на экран всех пользователей и их фильтрация
        /// </summary>
        /// <returns>Страница со списком пользователей</returns>
        [HttpGet]
        public ActionResult ListUsers()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            using (var db = new Contexts())
            {
                ViewBag.Users = db.Users.ToList();
            }
            return View();
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <returns>Форма редактирования пользователя</returns>
        [HttpGet]
        public ActionResult ChangeUser()
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);

            using (var db = new Contexts())
            {
                ViewBag.RedUser = db.Users.Find(Convert.ToInt32(HttpContext.Request.Params["RedUserId"]));
            }

            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий изменение данных о пользователе
        /// </summary>
        /// <returns>Перенаправление на страницу со списком пользователей</returns>
        [HttpPost]
        public ActionResult ChUser(HttpPostedFileBase upload)
        {
            //Вытаскиваем ид из адреса
            ViewBag.UserID = Convert.ToInt32(HttpContext.Request.Params["UserId"]);
            int RedUserId = Convert.ToInt32(HttpContext.Request.Params["ID"]);
            string Entitlement = HttpContext.Request.Form["Entitlement"];
            string pic = "";
            int status;
            if (Entitlement == "Покупатель")
                status = 0;
            else if (Entitlement == "Модератор")
                status = 1;
            else
                status = 2;


                using (var db = new Contexts())
            {
                User user = db.Users.Find(RedUserId);
                if (upload != null)
                {
                    pic = Path.GetFileName(upload.FileName);

                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Content/Style/Files/" + pic));
                    user.Picture = pic;
                }
                
                user.Login = HttpContext.Request.Form["Login"];
                user.Email = HttpContext.Request.Form["Email"];
                user.FirstName = HttpContext.Request.Form["FirstName"];
                user.Name = HttpContext.Request.Form["Name"];
                user.Patronymic = HttpContext.Request.Form["Patronymic"];
                user.PhoneNumber = HttpContext.Request.Form["PhoneNumber"];
                user.Status = status;
                db.SaveChanges();
            }
            
            return Redirect("../Admin/ListUsers?UserId=" + ViewBag.UserID);
        }
    }
}
