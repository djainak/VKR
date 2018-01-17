﻿using System;
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
        /// Название меню
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

    /// <summary>
    /// Класс пользвательской корзины
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// ID корзины
        /// </summary>
        public int CartID { get; set; }

        /// <summary>
        /// Внешний ключ на покупателя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Товары, входящие в корзину и их количество
        /// </summary>
        public virtual Dictionary <MenuItem, int> CartMenuItems { get; set; }

    }

    /// <summary>
    /// Класс заказа
    /// </summary>
    public class Order
    {
        /// <summary>
        /// ID заказа
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Упаковка заказа
        /// 0 - с собой
        /// 1 - на вынос
        /// </summary>
        public bool WhereEat { get; set; }

        /// <summary>
        /// Время поступления заказа в обработку
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// Время готовности заказа
        /// </summary>
        public DateTime ReadyTime { get; set; }

        /// <summary>
        /// Заметки к заказу
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// 0 - обработка
        /// 1 - подтверждет
        /// 2 - ожидает выдачи
        /// 3 - выполнен
        /// 4 - отменен
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Внешний ключ на покупателя
        /// </summary>
        public int UserId { get; set; }
    }
    

}