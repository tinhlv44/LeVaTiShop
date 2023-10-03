using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Controllers
{
    public class HomeController : Controller
    {
        dtDataContext dt = new dtDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult Menu()
        {
            return PartialView();
        }
        public ActionResult SlideShow()
        {
            return PartialView();
        }
        public ActionResult DetailProduct()
        {
            var x = dt.Products.First();
            return PartialView(x);
        }

    }
}