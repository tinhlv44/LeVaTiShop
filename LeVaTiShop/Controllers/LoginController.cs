using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Controllers
{
    public class LoginController : Controller
    {
        dtDataContext dt = new dtDataContext();
        // GET: Login
        [HttpGet]
        public ActionResult Login(string url)
        {
            ViewBag.url = url;
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var user = f["nameUser"];
            var pass = f["password"];
            var url = f["url"];

            if (string.IsNullOrEmpty(user))
            {
                ViewData["errUser"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(pass))
            {
                ViewData["errPass"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                User kh = dt.Users.SingleOrDefault(n => n.nameUser == user && n.password == pass);
                if (kh != null)
                {
                    Session["KhachHang"] = kh;
                    /*if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TenDN"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }*/
                    if (string.IsNullOrEmpty(url))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(url);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection f)
        {
            var user = f["relo_input_user"];
            var pass = f["relo_input_pass"];

            if (string.IsNullOrEmpty(user))
            {
                ViewData["errUser"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(pass))
            {
                ViewData["errPass"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                User kh = dt.Users.SingleOrDefault(n => n.nameUser == user && n.password == pass);
                if (kh != null)
                {
                    Session["KhachHang"] = kh;
                    /*if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TenDN"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }*/
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }
    }
}