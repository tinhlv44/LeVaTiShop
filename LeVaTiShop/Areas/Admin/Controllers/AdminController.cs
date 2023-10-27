using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public static string ActionName { get; set; }
        dtDataContext dt = new dtDataContext();
        // GET: Admin/Admin
        [HttpGet]
        public ActionResult Index()
        {
            AdminController.ActionName = "ManagerUsers";
            var res = dt.Users.ToList();
            /*User u = (User)Session["User"];
            if (u==null)
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }*/

            return View(res);
        }
        [HttpPost]
        public ActionResult Index(string key)
        {
            AdminController.ActionName = "ManagerUsers";
            var res = dt.Users.Where(p => p.idUser.ToString().Contains(key) || p.nameUser.Contains(key) || p.fullName.Contains(key) || p.email.Contains(key)|| p.address.Contains(key)|| p.phone.Contains(key)).ToList();
            if (res == null || res.Count == 0)
            {
                return View();
            }
            return View(res);
        }
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult Sidebar()
        {
            return PartialView();
        }
        public ActionResult UserDetail(int id)
        {
            AdminController.ActionName = "ManagerUsers";
            return View(dt.Users.Where(s=>s.idUser == id).Single());
        }
    }
}