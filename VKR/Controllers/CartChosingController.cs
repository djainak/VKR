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
    }
}
