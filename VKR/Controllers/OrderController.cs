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
    }
}
