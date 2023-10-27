using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using PagedList;
using PagedList.Mvc;

namespace LeVaTiShop.Controllers
{
    public class HomeController : Controller
    {
        dtDataContext dt = new dtDataContext();
        public static string page = "chu";
        public ActionResult Index(int ? pa)
        {
            int iSize = 8;
            int iPageNumber = (pa ?? 1);
            page = "chu";
            return View(dt.Products.Where(p=>p.isFeatured==true).ToPagedList(iPageNumber, iSize));
        }
        public ActionResult Search(decimal priceForm, decimal priceTo, string nameBrand=null)
        {
            IPagedList<Product> lP;
            if (nameBrand=="chu")
            {
                lP = dt.Products.Where(s => (s.isDiscounted == true && s.discountedPrice > priceForm && s.discountedPrice < priceTo && s.isFeatured == true) || (s.isDiscounted == false && s.price > priceForm && s.price < priceTo && s.isFeatured == true)).ToList().ToPagedList(1,8);
            }
            else
            {
                page = nameBrand;
                lP = dt.Products.Where(s => (s.isDiscounted == true && s.discountedPrice > priceForm && s.discountedPrice < priceTo && s.Brand.nameBrand == nameBrand) || (s.isDiscounted == false && s.price > priceForm && s.price < priceTo && s.Brand.nameBrand == nameBrand)).ToPagedList(1,8);
            }
            return View("Index", lP);
        }
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult Sidebar()
        {
            return PartialView();
        }
        public ActionResult Menu()
        {
            return PartialView(dt.Brands.ToList());
        }
        public ActionResult SlideShow()
        {
            return PartialView();
        }
        public ActionResult DetailProduct(int id)
        {
            var x = dt.Products.Where(s=>s.idProduct == id).Single();
            return PartialView(x);
        }
        
        public ActionResult Help()
        {
            return View();
        }
        
    } 
}