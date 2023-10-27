using LeVaTiShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        dtDataContext dt = new dtDataContext();
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string keyWord)
        {
            return View(dt.Products.Where(s=>s.nameProduct.Contains(keyWord)|| s.Brand.nameBrand.Contains(keyWord)|| s.Category.name.Contains(keyWord)|| s.description.Contains(keyWord)).ToPagedList(1,8));
        }
    }
}