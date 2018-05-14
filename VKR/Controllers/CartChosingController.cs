using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using VKR.Models;

namespace VKR.Controllers
{
    /// <summary>
    /// Контроллер, реализовывающий логику работы с корзиной пользователя
    /// </summary>
    public class CartChosingController : ApiController
    {
        /// <summary>
        /// Метод, обрабатывающий удаление товара из корзины
        /// </summary>
        /// <param name="id">Уникальный идентификатор товара в корзине</param>
        /// <returns>empty - если корзина пуста, иначе пустая строка</returns>
        public bool Post(int id)
        {
            int amount;
            using (var db = new Contexts())
            {
                Cart cart = db.Cart.Find(id);
                db.Cart.Remove(cart);
                db.SaveChanges();
                amount = db.Cart.Count();
            }

            if (amount == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Метод, реализующий очистку корзины пользователя
        /// </summary>
        public void Post()
        {
            int id_user;

            CookieHeaderValue cookie = Request.Headers.GetCookies("user_token").FirstOrDefault();
            if (cookie != null)
            {
                id_user = Convert.ToInt32(cookie["user_token"].Value);
                using (var db = new Contexts())
                {
                    List<Cart> cart = new List<Cart>();
                    cart = db.Cart.Where(c => c.UserId == id_user).ToList();
                    foreach (var c in cart)
                    {
                        db.Cart.Remove(c);
                    }
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Метод, обеспечивающий изменение количества товара в БД
        /// </summary>
        /// <param name="id">Уникальный идентификатор товара</param>
        /// <param name="amount">Новое количество товара</param>
        public void Post(int id, int amount)
        {
            using (var db = new Contexts())
            {
                db.Cart.Find(id).Amount = amount;
                db.SaveChanges();
            }
        }

        public bool Post(string time, string notes, string where_eat, int allprice)
        {
            Dictionary<string, string> dayName = new Dictionary<string, string>(7);
            dayName.Add("Monday", "Понедельник");
            dayName.Add("Tuesday", "Вторник");
            dayName.Add("Wednesday", "Среда");
            dayName.Add("Thursday", "Четверг");
            dayName.Add("Friday", "Пятница");
            dayName.Add("Saturday", "Суббота");
            dayName.Add("Sunday", "Воскресенье");

            //Название сегодняшнего дня
            string Today = dayName[DateTime.Now.DayOfWeek.ToString()];

            using (var db = new Contexts())
            {
                //Выбираю день с названием сегодняшнего
                DayWork day = new DayWork();
                day =  db.DayWork.Where(d => d.Name == Today).FirstOrDefault();

                //Выбираю из доступных времен заказов только сегодняшнее и по выбранному времени
                FreeTime t = new FreeTime();
                t = db.FreeTime.Where(c => c.Time == time && c.DayWorkID == day.DayWorkID)
                    .FirstOrDefault();

                //Проверяю, не успели ли его занять
                if (t.cur_amount + 1 <= t.max_amount)
                {
                    ++t.cur_amount;

                    int id_user;

                    CookieHeaderValue cookie = Request.Headers.GetCookies("user_token").FirstOrDefault();
                    if (cookie != null)
                    {
                        //Уникальный идентификатор пользователя из куки
                        id_user = Convert.ToInt32(cookie["user_token"].Value); 

                        //Заполняю заказ
                        Order order = new Order();
                        List<Cart> cart = db.Cart.Where(c => c.UserId == id_user).ToList();
                        order.CartMenuItems = new Dictionary<MenuItem, int>();
                            foreach (Cart c in cart)
                            {
                                MenuItem item = db.MenuItems.Where(m => m.Id == c.MenuItemId).FirstOrDefault();
                                order.CartMenuItems.Add(item, c.Amount);
                            }
                            
                        order.Notes = notes;
                        order.OrderTime = DateTime.Now;
                        order.ReadyTime = time;
                        order.Status = 0;
                        order.UserId = id_user;
                        order.Sum = allprice;
                        if (where_eat == "true")
                            order.WhereEat = true;
                        else
                            order.WhereEat = false;
                        db.Orders.Add(order);

                        //Чистим корзину
                        foreach(Cart c in cart)
                        {
                            db.Cart.Remove(c);
                        }
                        db.SaveChanges();
                        order.NumberOrder = DateTime.Now.Year.ToString().Substring(2) + DateTime.Now.Month.ToString()
                            + DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Hour.ToString()
                            + order.OrderID;
                        db.SaveChanges();
                    }
                    
                    return true;
                }
                else //Если успели
                    return false;
            }
        }
    }
}
