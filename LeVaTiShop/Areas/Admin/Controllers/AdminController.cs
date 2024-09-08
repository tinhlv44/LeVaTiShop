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
            var res = dt.Users.Where(u=>u.isDeleted==false).ToList();
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(res);
        }
        [HttpPost]
        public ActionResult Index(string key)
        {
            AdminController.ActionName = "ManagerUsers";
            var res = dt.Users.Where(u => u.idUser.ToString().Contains(key) || u.nameUser.Contains(key) || u.fullName.Contains(key) || u.email.Contains(key)|| u.address.Contains(key)|| u.phone.Contains(key)).Where(u=>u.isDeleted==false).ToList();
            /*if (res == null || res.Count == 0)
            {
                return View();
            }*/
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
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View(dt.Users.Where(s=>s.idUser == id).Single());
        }
        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            try
            {
                if (id == 1)
                {
                    return Json(new { code = false, msg = "Không có quyền xóa tài khoản này." }, JsonRequestBehavior.AllowGet);
                }
                var user = dt.Users.SingleOrDefault(s => s.idUser == id);
                user.isDeleted = true;
                dt.Users.DeleteOnSubmit(user);
                dt.SubmitChanges();
                return Json(new { code = true, msg = "Xóa tài khoản thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Xóa sản phẩm thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.nameUser = ViewBag.nameUser ?? "";
            ViewBag.password = ViewBag.password ?? "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var user = f["nameUser"];
            var pass = f["password"];

            ViewBag.nameUser = user;
            ViewBag.password = pass;
            pass = function.HashPassword(pass);

            if (ModelState.IsValid)
            {
                User u = dt.Users.SingleOrDefault(n => n.nameUser == user && n.password == pass);
                if (u != null)
                {
                    Session["UserAD"] = u;
                    if (u.role)
                    {
                        Session["Admin"] = true;
                        // Gửi mã OTP qua email
                        var otp = GenerateOTP();
                        function fun = new function();
                        string sub = "Gửi OTP xác minh";
                        string body = "Xin chào,\nChào mừng bạn đến với trang quản trị H&T\nMã OTP của bạn là: " + otp + "\nTrân trọng\nH&T";
                        fun.SendMail(u.email,sub, body);
                        Session["OTP"] = otp;
                        Session["OTPEndTime"] = DateTime.Now.AddMinutes(5);
                        return RedirectToAction("VerifyOTP", "Admin", new { area = "Admin" });
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult VerifyOTP()
        {
            var otp = Session["OTP"] as string;
            if (string.IsNullOrEmpty(otp))
            {
                // Nếu OTP chưa được gửi, chuyển hướng người dùng về trang Login
                return RedirectToAction("login", "Admin", new { area = "Admin" });
            }
            return View();
        }


        [HttpPost]
        public ActionResult VerifyOTP(string otp)
        {
            var correctOTP = Session["OTP"] as string;
            var otpEndTime = Session["OTPEndTime"] as DateTime?;

            if (otpEndTime < DateTime.Now)
            {
                ViewBag.ThongBao = "Mã OTP đã hết hạn";
                return View();
            }

            if (otp == correctOTP)
            {
                // Xóa OTP khỏi phiên sau khi xác minh thành công
                Session["OTP"] = null;
                Session["OTPEndTime"] = null;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            else
            {
                ViewBag.ThongBao = "Mã OTP không chính xác";
                return View();
            }
        }
        private string GenerateOTP()
        {
            Random random = new Random();
            string otp = "";
            for (int i = 0; i < 6; i++)
            {
                otp += random.Next(0, 9).ToString();
            }
            return otp;
        }
        [HttpGet]
        public ActionResult ResendOTP()
        {
            var user = Session["UserAD"] as User;
            if (user != null)
            {
                // Tạo và gửi mã OTP mới
                var otp = GenerateOTP();
                function fun = new function();
                string sub = "Gửi OTP xác minh";
                string body = "Xin chào,\nChào mừng bạn đến với trang quản trị H&T\nMã OTP của bạn là: " + otp + "\nTrân trọng\nH&T";
                fun.SendMail(user.email, sub, body);
                Session["OTP"] = otp;
                Session["OTPEndTime"] = DateTime.Now.AddMinutes(5);
                return RedirectToAction("VerifyOTP", "Admin", new { area = "Admin" });
            }
            else
            {
                // Nếu người dùng không tồn tại trong phiên, chuyển hướng họ về trang Login
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
        }


    }
}