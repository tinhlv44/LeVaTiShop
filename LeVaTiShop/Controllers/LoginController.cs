using LeVaTiShop.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Register(string url)
        {
            ViewBag.url = url;
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
                User u = dt.Users.SingleOrDefault(n => n.nameUser == user && n.password == pass);
                if (u != null)
                {
                    Session["User"] = u;
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
            var user = f["nameUser"];
            var pass = f["pasword"];
            var email = f["email"];
            var phone = f["phone"];
            var address = f["address"];
            var fullName = f["fullName"];
            var url = f["url"];

            if (dt.Users.SingleOrDefault(n => n.nameUser == user) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (dt.Users.SingleOrDefault(n => n.email == email) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else if (ModelState.IsValid)
            {
                User u = new User();
                u.nameUser = user;
                u.password = pass;
                u.phone = phone;
                u.address = address;
                u.fullName = fullName;
                u.email = email;
                u.role = false;
                u.isDeleted = false;
                dt.Users.InsertOnSubmit(u);
                dt.SubmitChanges();
                //Tạo form đề đăng nhập sau khi đăng kí
                FormCollection form = new FormCollection();
                form.Add("nameUser", user);
                form.Add("password", pass);
                form.Add("url", url);
                return Login(form);
            }
            return View();
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection f)
        {
            /*User user = dt.Users.Where(s=>s.idUser == int.Parse(f["id"])).Single();
            user.password = f["newPassword"];
            dt.SubmitChanges();*/
            return View();
        }
        [HttpGet]
        public new ActionResult Profile(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(dt.Users.SingleOrDefault(s => s.idUser == id));
        }
        [HttpPost]
        public new ActionResult Profile(FormCollection f, HttpPostedFileBase image)
        {
            User u = (User)Session["User"];
            int id = u.idUser;
            var user = dt.Users.SingleOrDefault(s => s.idUser == id);
            user.fullName = f["fullName"];
            user.address = f["address"];
            user.phone = f["phone"];
            user.email = f["email"];
            string productImageDirectory = Server.MapPath("~/set/img/user");

            if (image != null && image.ContentLength > 0)
            {
                // Lấy tên tệp tin ảnh
                string name = "user" + user.idUser;
                string extension = Path.GetExtension(image.FileName);
                string newFileName = name + extension;
                // Tạo đường dẫn đến tệp tin ảnh trong thư mục lưu trữ ảnh sản phẩm
                string filePath = Path.Combine(productImageDirectory, newFileName);

                // Kiểm tra xem thư mục lưu trữ ảnh sản phẩm đã tồn tại chưa
                if (!Directory.Exists(productImageDirectory))
                {
                    Directory.CreateDirectory(productImageDirectory);
                }

                // Lưu tệp tin ảnh vào đường dẫn
                image.SaveAs(filePath);

                user.avatar = newFileName;
            }
            dt.SubmitChanges();
            return View(dt.Users.SingleOrDefault(s => s.idUser == id));
        }
    }
}