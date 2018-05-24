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
            //Добавляем данные о пользователе в куки
            HttpCookie token = new HttpCookie("user_token", "1");
            HttpContext.Response.Cookies.Add(token);

            using (var db = new Contexts())
            {
                ViewBag.Category = db.CategoryMenuItem.ToList();
            }
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий страницу корзины пользователя
        /// </summary>
        /// <returns>Страница с корзиной пользователя</returns>
        [HttpGet]
        public ActionResult CartPage()
        {
            int allprice = 0;
            List<Cart> cart = new List<Cart>();
            List<FreeTime> times = new List<FreeTime>();
            int id_user = Convert.ToInt32(HttpContext.Request.Cookies["user_token"].Value);
            int now_h = DateTime.Now.Hour;
            int now_m = DateTime.Now.Minute;
            List<FreeTime> tmp = new List<FreeTime>();

            Dictionary<string, string> dayName = new Dictionary<string, string>(7);
            dayName.Add("Monday", "Понедельник");
            dayName.Add("Tuesday", "Вторник");
            dayName.Add("Wednesday", "Среда");
            dayName.Add("Thursday", "Четверг");
            dayName.Add("Friday", "Пятница");
            dayName.Add("Saturday", "Суббота");
            dayName.Add("Sunday", "Воскресенье");

            string Today = dayName[DateTime.Now.DayOfWeek.ToString()];

            using (var db = new Contexts())
            {
                cart = db.Cart.Where(c => c.UserId == id_user).ToList();

                //Выгружаем товары
                foreach (var c in cart)
                {
                    c.MenuItem = db.MenuItems.Where(cc => cc.Id == c.MenuItemId).FirstOrDefault();
                    c.MenuItem.Category = db.CategoryMenuItem.Where(cc => cc.CategoryMenuItemID == c.MenuItem.CategoryMenuItemId).FirstOrDefault();
                    allprice = allprice + c.MenuItem.Price * c.Amount;
                }

                //Отступаем от нынешнего момента минимальное время готовности заказа
                now_h = now_h + (now_m + db.DinningRooms.FirstOrDefault().Min_time) / 60;
                now_m = (now_m + db.DinningRooms.FirstOrDefault().Min_time) % 60;
                
                //Выгружаем доступное время
                times = db.FreeTime.Where(t => t.DayWork.Name == Today && t.cur_amount < t.max_amount).ToList();
                
                foreach (FreeTime t in times)
                {
                    // 
                    if (Convert.ToInt32(t.Time.Substring(0, t.Time.IndexOf(':'))) > now_h || Convert.ToInt32(t.Time.Substring(0, t.Time.IndexOf(':'))) == now_h && Convert.ToInt32(t.Time.Substring(t.Time.IndexOf(':') + 1, 2)) > now_m)
                    {
                        tmp.Add(t);
                    }
                }
                
            }
            if (cart.Count == 0)
                ViewBag.Empty = "true";
            ViewBag.Cart = cart;
            ViewBag.AllPrice = allprice;
            ViewBag.Times = tmp;
            if (tmp.Count == 0)
                ViewBag.NoTime = true;
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий страницу с заказами
        /// </summary>
        /// <returns>Страница с заказами</returns>
        [HttpGet]
        public ActionResult ListOrders()
        {
            int id_user;
            id_user = Convert.ToInt32(HttpContext.Request.Cookies["user_token"].Value);
            List<Order> orders = new List<Order>();
                using (var db = new Contexts())
                {
                    orders = db.Orders.Where(o => o.UserId == id_user).ToList();
                    orders.Reverse();
                }
            if (orders.Count == 0)
                ViewBag.Empty = true;
            ViewBag.Orders = orders;
                
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий страницу со списком заказанных товаров в определенном заказе
        /// </summary>
        /// <returns>Страницу с информацией о заказанных товарах</returns>
        [HttpGet]
        public ActionResult ViewProducts()
        {
            int id_order = Convert.ToInt32(HttpContext.Request.Params["OrderId"]);
            List<OrderItems> orderitems = new List<OrderItems>();

            using (var db = new Contexts())
            {
                ViewBag.Order = db.Orders.Find(id_order);

                orderitems = db.OrderItems.Where(o => o.OrderId == id_order).ToList();

                foreach (var o in orderitems)
                {
                    o.MenuItem = db.MenuItems.Where(m => m.Id == o.MenuItemId).FirstOrDefault();
                    o.MenuItem.Category = db.CategoryMenuItem.Where(cc => cc.CategoryMenuItemID == o.MenuItem.CategoryMenuItemId).FirstOrDefault();
                }
                ViewBag.AllPrice = db.Orders.Find(id_order).Sum;
                ViewBag.Products = orderitems;
            }
                return View();
        }

        /// <summary>
        /// Метод, обрабатывающий профиль пользователя
        /// </summary>
        /// <returns>Страницу с профилем пользователя</returns>
        [HttpGet]
        public ActionResult Profile()
        {
                int id_user;
                id_user = Convert.ToInt32(HttpContext.Request.Cookies["user_token"].Value);
                using (var db = new Contexts())
                {
                    
                    ViewBag.User = db.Users.Find(id_user);
                    if (db.Users.Find(id_user) == null)
                        ViewBag.Empty = true;
                }
           
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий страницу редактирования данных профиля
        /// </summary>
        /// <returns>Страница редактирования данных профиля</returns>
        [HttpGet]
        public ActionResult ChangeProfile()
        {
            using (var db = new Contexts())
            {
                ViewBag.RedUser = db.Users.Find(Convert.ToInt32(HttpContext.Request.Cookies["user_token"].Value));
            }
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий изменения информации в профиле пользователя
        /// </summary>
        /// <param name="upload">Фотография</param>
        /// <returns>Переадресация на обновленный профиль пользователя</returns>
        [HttpPost]
        public ActionResult ChProfile(HttpPostedFileBase upload)
        {
            int RedUserId = Convert.ToInt32(HttpContext.Request.Cookies["user_token"].Value);
            string pic = "";

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
                db.SaveChanges();
            }

            return Redirect("../Client/Profile");
        }

        /// <summary>
        /// Метод, обрабатывающий страницу с контактами
        /// </summary>
        /// <returns>Страница контактов</returns>
        [HttpGet]
        public ActionResult Contacts()
        {
            using (var db = new Contexts())
            {
                ViewBag.DR = db.DinningRooms.FirstOrDefault();
                @ViewBag.Manager = db.Users.Where(c => c == db.DinningRooms.FirstOrDefault().Manager).FirstOrDefault();
            }

            return View();
        }
    }
}