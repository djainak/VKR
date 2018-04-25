using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    public class FilterMenuItemsController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Возвращает позиции меню по определенной категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns>Строка JSON с позициями меню</returns>
        public string Get(int id)
        {
            List<MenuItem> menuitems = new List<MenuItem>();
            using (var db = new Contexts())
            {
                //добавить проверку на выгрузку из действующего меню
                //Menu menu = db.Menues.Where(m => m.Status == true).FirstOrDefault(); && m.Menu == menu
                menuitems = db.MenuItems.Where(m => m.CategoryMenuItemId == id).ToList();
            }

            return JsonConvert.SerializeObject(menuitems);
        }
    }
}