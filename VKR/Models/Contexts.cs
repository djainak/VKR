using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace VKR.Models
{
    public class Contexts : DbContext
    {
        public Contexts() : base("VKR.Models.Contexts")
        { }

        /// <summary>
        /// Коллекция данных, загружаемая из БД, с данными из таблицы Меню
        /// </summary>
        public DbSet<Menu> Menues { get; set; }

        /// <summary>
        /// Коллекция данных, загружаемая из БД, с данными из таблицы пунктов меню
        /// </summary>
        public DbSet<MenuItem> MenuItems { get; set; }

        /// <summary>
        /// Коллекция данных, загружаемая из БД, с данными из таблицы категорий блюд меню
        /// </summary>
        public DbSet<CategoryMenuItem> CategoryMenuItem { get; set; }

        /// <summary>
        /// Коллекция данных, загружаемая из БД, с данными из таблицы пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Коллекция данных, загружаемая из БД, с данными из таблицы заказов
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Коллекция данных, загружаемая из БД, с данными из таблицы точек питания
        /// </summary>
        public DbSet<DinningRoom> DinningRooms { get; set; }
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
        /// Поле, показывающее, какое меню сейчас действующее
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Товары, входящие в меню
        /// </summary>
        public virtual List<MenuItem> MenuItems { get; set; }

        public MenuItem MenuItem
        {
            get => default(MenuItem);
            set
            {
            }
        }
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
        /// Категория блюда
        /// </summary>
        public CategoryMenuItem Category { get; set; }

        /// <summary>
        /// Ключ к категориям блюд
        /// </summary>
        public int CategoryMenuItemId { get; set; }

        /// <summary>
        /// Внешний ключ на меню, в котором лежит пункт меню
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Объект меню, на который ссылается пункт
        /// </summary>
        public Menu Menu { get; set; }

        public CategoryMenuItem CategoryMenuItem
        {
            get => default(CategoryMenuItem);
            set
            {
            }
        }
    }

    /// <summary>
    /// Класс для таблицы категорий блюд
    /// </summary>
    public class CategoryMenuItem
    {
        /// <summary>
        /// Id категории
        /// </summary>
        public int CategoryMenuItemID { get; set; }

        /// <summary>
        /// Название категории
        /// </summary>
        public string Name { get; set; }
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

        /// <summary>
        /// Товары, входящие в заказ и их количество
        /// </summary>
        public virtual Dictionary<MenuItem, int> CartMenuItems { get; set; }

        public User User
        {
            get => default(User);
            set
            {
            }
        }

        public MenuItem MenuItem
        {
            get => default(MenuItem);
            set
            {
            }
        }
    }

    /// <summary>
    /// Класс точки питания
    /// </summary>
    public class DinningRoom
    {
        /// <summary>
        /// ID точки питания
        /// </summary>
        public int DinningRoomID { get; set; }

        /// <summary>
        /// Адрес точки питания
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// Телефон точки питания
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// Адрес электронной почты точки питания
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Контактное лицо точки питания
        /// </summary>
        public User Manager { get; set; }

        /// <summary>
        /// Количество блюд для рассчета предлагаемого времени
        /// </summary>
        public int Dishes { get; set; }

        /// <summary>
        /// Интервал, в который приготавливается определенное в Dishes количество блюд
        /// </summary>
        public int Interval { get; set; }

        public User User
        {
            get => default(User);
            set
            {
            }
        }
    }
}