﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using VKR.Models;

namespace VKR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult MenuList()
        {
            using (var db = new Contexts())
            {
                List<Menu> menues = db.Menues.ToList();
                ViewBag.Menues = menues;
            }

            return View();
        }
    }
    public class AdminController : Controller
    {

    }
    public class AddMenuItemController : Controller
    {
        [HttpGet]
        public ActionResult AddMenuItemForm()
        {
            //Вытаскиваем ид из адреса
            int id = Convert.ToInt32(HttpContext.Request.Params["MenuId"]);

            ViewBag.isError = false;
            //Обращаемся к БД
            using (var db = new Contexts())
            {
                //Ищем там меню по ид
                Menu menu = db.Menues.Where(ent => ent.Id == id).FirstOrDefault();
                if(menu == null)
                {
                    ViewBag.isError = true;
                }
                else
                {
                    ViewBag.MenuId = id;
                    List<MenuItem> menuItems = db.MenuItems.Where(ent => ent.MenuId == id).ToList();
                    ViewBag.MenuItems = menuItems;
                }
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult AddMenuItem()
        {
            //Считываем данные из формы
            int menuId = Convert.ToInt32(HttpContext.Request.Form["MenuId"]);
            string name = HttpContext.Request.Form["Name"];
            string ingredients = HttpContext.Request.Form["Ingredients"];
            int price = Convert.ToInt32(HttpContext.Request.Form["Price"]);

            //Создаем объект пункта Меню и заполняем данными из формы
            MenuItem menuItem = new MenuItem();
            menuItem.MenuId = menuId;
            menuItem.Name = name;
            menuItem.Ingredients = ingredients;
            menuItem.Price = price;

            //Добавляем новый пункт меню в БД
            using (var db = new Contexts())
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
            }
            return Redirect("./AddMenuItemForm?menuId=" + menuId);
            //return View();
        }

    }
}
