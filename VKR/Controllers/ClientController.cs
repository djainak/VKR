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
            List<string> times = new List<string>();
            int id_user = Convert.ToInt32(HttpContext.Request.Cookies["user_token"].Value);
            
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
                    c.Product = db.MenuItems.Where(cc => cc.Id == c.MenuItemId).FirstOrDefault();
                    c.Product.Category = db.CategoryMenuItem.Where(cc => cc.CategoryMenuItemID == c.Product.CategoryMenuItemId).FirstOrDefault();
                    allprice = allprice + c.Product.Price * c.Amount;
                }

                //Выгружаем доступное время
                List<FreeTime> tmp = new List<FreeTime>();
                tmp = db.FreeTime.Where(t => t.DayWork.Name == Today && t.cur_amount < t.max_amount).ToList();
                foreach(var t in tmp)
                {
                    times.Add(t.Time);
                }
            }
            if (cart.Count == 0)
                ViewBag.Empty = "true";
            ViewBag.Cart = cart;
            ViewBag.AllPrice = allprice;
            ViewBag.Times = times;
            return View();
        }

        /// <summary>
        /// Метод, обрабатывающий страницу с оформлением заказа
        /// </summary>
        /// <returns>Страница оформления заказа</returns>
        [HttpGet]
        public ActionResult AddOrder()
        {
            return View();
        }
    }
}