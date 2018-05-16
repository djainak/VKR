using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VKR.Models;

namespace VKR.Controllers
{
    /// <summary>
    /// Контроллер, обрабатывающий действия с заказами в администраторской части приложения
    /// </summary>
    public class OrderController : ApiController
    {
        /// <summary>
        /// Метод, обрабатывающий изменение статуса заказа
        /// </summary>
        /// <param name="status">Новый статус</param>
        /// <param name="order_id">Уникальный идентификатор заказа</param>
        public void Post(int status, int order_id)
        {
            using (var db = new Contexts())
            {
                db.Orders.Find(order_id).Status = status;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод, выгружающий новые заказы 
        /// </summary>
        /// <param name="last_id">Уникальный идентификатор последнего выгруженного заказа</param>
        /// <returns>Уникальный идентификатор последнего выгруженного заказа после применения
        /// метода</returns>
        public string Get(int last_id)
        {
            List<Order> orders = new List<Order>();
            using (var db = new Contexts())
            {
                orders = db.Orders.Where(o => o.OrderID > last_id && (o.Status == 0 || o.Status == 1 || o.Status == 2))
                    .ToList();
            }
            return JsonConvert.SerializeObject(orders);
        }
    }
}
