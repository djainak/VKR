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
        /// Метод, обрабатывающий фильтрациют заказов по номеру и статусам
        /// </summary>
        /// <param name="number_order">Номер заказа</param>
        /// <param name="status0">Выбрана ли галочка "Обработка"</param>
        /// <param name="status1">Выбрана ли галочка "Подтвержден"</param>
        /// <param name="status2">Выбрана ли галочка "Ожидает выдачи"</param>
        /// <returns>Список заказов, подходящих под фильтр</returns>
        public string Get(string number_order, bool status0, bool status1, bool status2)
        {
            List<Order> orders = new List<Order>();
            using (var db = new Contexts())
            {
                orders = db.Orders.Where(o => o.Status != 3 && o.Status != 4).ToList();
                if (number_order != null)
                    orders = orders.Where(u => u.NumberOrder == number_order.Trim()).ToList();
                if (status0 == false)
                    orders = orders.Where(u => u.Status != 0).ToList();
                if (status1 == false)
                    orders = orders.Where(u => u.Status != 1).ToList();
                if (status2 == false)
                    orders = orders.Where(u => u.Status != 2).ToList();
            }
                return JsonConvert.SerializeObject(orders);
        }
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

        public void Get()
        {

        }
    }
}
