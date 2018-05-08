
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VKR.Models;

namespace VKR.Controllers
{
    public class FilterMenuItemsController : ApiController
    {
        /// <summary>
        /// Возвращает позиции меню по определенной категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns>Строка JSON с позициями меню</returns>
        public string Get(int id)
        {
            string res = "";
            try {
                List<MenuItem> menuitems = new List<MenuItem>();
                int menu_id;
                using (var db = new Contexts())
                {
                    //добавить проверку на выгрузку из действующего меню
                    menu_id = db.Menues.Where(m => m.Status == true).FirstOrDefault().Id; 
                    menuitems = db.MenuItems.Where(m => (m.CategoryMenuItemId == id && m.MenuId == menu_id)).ToList();
                    
                }
                foreach(MenuItem m in menuitems)
                {
                    m.Category = null;
                    m.Menu = null;
                }
                res = JsonConvert.SerializeObject(menuitems);
            }
            catch (Exception e)
            {
                res = e.StackTrace;
            }

            return res;
        }

        /// <summary>
        /// Метод, обрабатывающий добавление товара в корзину
        /// </summary>
        /// <param name="id_product"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string Post(int id_product, int amount)
        {
            string id_user = "";
            
            CookieHeaderValue cookie = Request.Headers.GetCookies("user_token").FirstOrDefault();
            if (cookie != null)
            {
                id_user = cookie["user_token"].Value;
            }

            return id_user;
        }
    }
}