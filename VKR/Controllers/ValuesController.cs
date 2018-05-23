using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using VKR.Models;

namespace VKR.Controllers
{
    /// <summary>
    /// Контроллер, выполняющий функции удаления меню и пункта меню
    /// </summary>
    public class ValuesController : ApiController
    {
        /// <summary>
        /// Метод, выполняющий удаление меню
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемого меню</param>
        /// <returns>Есть ли еще доступные меню. False - если нет доступных меню
        /// True - если выбранное меню не меняется
        /// Id - уникальный идентификатор нового основного меню</returns>
        public string Post(int id)
        {
            bool flag = false;
            string ans = "";
            using (var db = new Contexts())
            {
                Menu menu = db.Menues.Find(id);
                if (menu.Status == true)
                {
                    flag = true;
                    if (db.Menues.FirstOrDefault() != null)
                        db.Menues.FirstOrDefault().Status = true;
                }
                db.Menues.Remove(menu);
                db.SaveChanges();
                if (flag && db.Menues.FirstOrDefault() != null)
                {
                    return JsonConvert.SerializeObject(db.Menues.FirstOrDefault().Name);
                }
                else if (flag && db.Menues.FirstOrDefault() == null)
                {
                    ans = "false";
                    return JsonConvert.SerializeObject(ans);
                }
                else
                {
                    ans = "true";
                    return JsonConvert.SerializeObject(ans);
                }
            }
        }

        /// <summary>
        /// Метод, реализующий удаление товара из меню и проверку меню на пустоту
        /// </summary>
        /// <param name="id">Уникальный идентификатор товара</param>
        /// <param name="MenuId">Уникальный идентификатор меню</param>
        /// <returns>true - не пустое, false - пустое</returns>
        public string Post(int id, int MenuId)
        {
            string ans = "";
            using (var db = new Contexts())
            {
                MenuItem menuItem = db.MenuItems.Find(id);
                db.MenuItems.Remove(menuItem);
                db.SaveChanges();
                if (db.MenuItems.Where(m => m.MenuId == MenuId).FirstOrDefault() != null)
                    ans = "true";
                else
                    ans = "false";
            }

            return JsonConvert.SerializeObject(ans);
        }
    }
}
