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
    /// <summary>
    /// Контроллер, обрабатывающий фильтрацию пользователей в администраторской части приложения
    /// </summary>
    public class FilterUsersController : ApiController
    {
        /// <summary>
        /// Возвращает пользователей после применения фильтра
        /// </summary>
        /// <param name="firstname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="login">Логин</param>
        /// <param name="tel">Телефон</param>
        /// <param name="email">email</param>
        /// <param name="status0">Выбрана ли галочка покупателя</param>
        /// <param name="status1">Выбрана ли галочка модератора</param>
        /// <param name="status2">Выбрана ли галочка администратора</param>
        /// <returns>Отфильтрованный лист данных</returns>
        public string Get(string firstname, string name, string patronymic, string login, string tel, string email, bool status0, bool status1, bool status2)
        {
            List<User> users = new List<User>();
            using (var db = new Contexts())
            {
                users = db.Users.ToList();
                if (firstname != null)
                    users = users.Where(u => u.FirstName == firstname.Trim()).ToList();
                if (name != null)
                    users = users.Where(u => u.Name == name.Trim()).ToList();
                if (patronymic != null)
                    users = users.Where(u => u.Patronymic == patronymic.Trim()).ToList();
                if (login != null)
                    users = users.Where(u => u.Login == login.Trim()).ToList();
                if (tel != null)
                    users = users.Where(u => u.PhoneNumber == tel.Trim()).ToList();
                if (email != null)
                    users = users.Where(u => u.Email == email.Trim()).ToList();
                if (status0 == false)
                    users = users.Where(u => u.Status != 0).ToList();
                if (status1 == false)
                    users = users.Where(u => u.Status != 1).ToList();
                if (status2 == false)
                    users = users.Where(u => u.Status != 2).ToList();
            }

            return JsonConvert.SerializeObject(users);
        }
    }
}