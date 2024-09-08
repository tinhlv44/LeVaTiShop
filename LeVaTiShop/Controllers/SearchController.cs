using LeVaTiShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LeVaTiShop.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        dtDataContext dt = new dtDataContext();
        [HttpGet]
        public ActionResult Search(int ? pa)
        {
            int iSize = 8;
            int iPageNumber = (pa ?? 1);
            return View(((IEnumerable<Product>)Session["resultSorted"]).ToPagedList(iPageNumber, iSize));
        }
        [HttpPost]
        public ActionResult Search(string keyWord)
        {
            ViewBag.keyWord = keyWord;
            var result = dt.Products.Where(s => (s.nameProduct.Contains(keyWord) || s.Brand.nameBrand.Contains(keyWord) || s.Category.name.Contains(keyWord) || s.description.Contains(keyWord)) && !s.isDeleted);
            Session["result"] = result;
            Session["resultSorted"] = result;
            return View(result.ToPagedList(1, 8));
        }
        public ActionResult SideBar()
        {
            return PartialView();
        }
        public ActionResult SortPrice(decimal priceForm, decimal priceTo)
        {
            IPagedList<Product> lP;
            Session["resultSorted"] = ((IEnumerable<Product>)Session["result"]).Where(s => ((s.isDiscounted == true && s.discountedPrice > priceForm && s.discountedPrice < priceTo) || (s.isDiscounted == false && s.price > priceForm && s.price < priceTo)) && s.isDeleted == false);
            lP = ((IEnumerable<Product>)Session["resultSorted"]).ToPagedList(1, 8);

            return View("Search", lP);
        }
    }
}