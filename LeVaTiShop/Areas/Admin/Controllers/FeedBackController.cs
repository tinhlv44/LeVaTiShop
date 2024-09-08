using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Areas.Admin.Controllers
{
    public class FeedBackController : Controller
    {
        // GET: Admin/FeedBack
        dtDataContext dt = new dtDataContext();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            AdminController.ActionName = "FeedBack";
            return View(dt.Messages.OrderByDescending(s=>s.date).Where(o=> o.messageContent[0] != 'M').ToList());
        }
        [HttpPost]
        public ActionResult Index(string key)
        {
            AdminController.ActionName = "FeedBack";
            var res = dt.Messages
                .Where(o => (o.User.fullName.Contains(key)
                            || o.messageContent.Contains(key)
                            || o.User.fullName.Contains(key)
                            || o.date.Day.ToString().Contains(key)
                            || o.date.Year.ToString().Contains(key)
                            || o.date.Month.ToString().Contains(key)
                            || o.date.Hour.ToString().Contains(key)
                            || o.date.Minute.ToString().Contains(key)
                            || o.date.Second.ToString().Contains(key)) && o.messageContent[0] != 'M'

                ).OrderByDescending(s => s.date)
                .ToList();
            return View(res);
        }
        [HttpPost]
        public JsonResult SendMail(int idUser, string subject, string body, string date)
        {
            try
            {
                function f = new function();
                User u = dt.Users.SingleOrDefault(s => s.idUser == idUser);
                string email = u.email;
                f.SendMail(email, subject, body);
                
                var repMessage = dt.Messages.ToList();
                foreach (var i in repMessage)
                {
                    if (i.date.ToString() == date) {
                        i.messageContent = f.takeMessage(i.messageContent);
                        i.messageContent = "R_" + i.messageContent;
                        break;
                    }
                }
                dt.SubmitChanges();

                return Json(new { code = true, msg = "Gửi mail phản hồi thành công"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Gửi mail phản hồi thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}