using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace VKR.Models
{
    public class Contexts : DbContext
    {
    }

    /// <summary>
    /// Класс меню
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Первычный ключ меню
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Назване меню
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание меню
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Товары, входящие в меню
        /// </summary>
        public virtual List<MenuItem> MenuItems { get; set; }
    }

    /// <summary>
    /// Класс пункта меню
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Первичный ключ пункта меню
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название пункта меню
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// Состав
        /// </summary>
        public string Ingredients { get; set; }

        /// <summary>
        /// Цена за единицу товара
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Внешний ключ на меню, в котором лежит пункт меню
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Объект меню, на который ссылается пункт
        /// </summary>
        public Menu Menu { get; set; } 
    }

    /// <summary>
    /// Класс пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 0 - покупатель
        /// 1 - кассир (модератор)
        /// 2 - менеджер (администратор)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Фамилия покупателя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Имя покупателя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Отчество покупателя
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Email покупателя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Логин покупателя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль покупателя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
    
}