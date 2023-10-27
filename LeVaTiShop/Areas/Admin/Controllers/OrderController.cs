using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        dtDataContext dt = new dtDataContext();
        // GET: Admin/Order
        public ActionResult Index()
        {
            AdminController.ActionName = "ManagerOrders";
            return View(dt.Orders.ToList());
        }
        public ActionResult DetailOrder(int id)
        {
            return View(dt.DetailOrders.Where(s=>s.idOrder==id).ToList());
        }
        public ActionResult SearchOrder()
        {
            return View();
        }
        /*public ActionResult Index()
        {
            return View();
        }*/
    }
}