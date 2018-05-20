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
    /// Контроллер, отвечающий за обновление статусов
    /// </summary>
    public class RefreshStatusesController : ApiController
    {
        /// <summary>
        /// Метод, возвращающий текущий статус заказа
        /// </summary>
        /// <param name="select_id">Уникальный идентификатор селекта, содержащий 
        /// уникальный идентификатор заказа</param>
        /// <returns>Tекущий статус</returns>
        public int Get(string select_id)
        {
            int num = Convert.ToInt32(select_id.Trim().Substring(7));
            using (var db = new Contexts())
            {
                return db.Orders.Find(num).Status;
            }
        }
    }
}
