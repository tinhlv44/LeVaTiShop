using LeVaTiShop.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

using Microsoft.AspNet.Identity.Owin;
using System.Text.RegularExpressions;

namespace LeVaTiShop.Controllers
{
    public class LoginController : Controller
    {
        dtDataContext dt = new dtDataContext();
        function func = new function();
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

            // Nếu có thông tin trong ViewBag, hiển thị lại
            ViewBag.nameUser = ViewBag.nameUser ?? "";
            ViewBag.password = ViewBag.password ?? "";
            ViewBag.rePassword = ViewBag.rePassword ?? "";
            ViewBag.email = ViewBag.email ?? "";
            ViewBag.phone = ViewBag.phone ?? "";
            ViewBag.address = ViewBag.address ?? "";
            ViewBag.fullName = ViewBag.fullName ?? "";
            ViewBag.url = ViewBag.url ?? "";

            return View();
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            CartHelper.ClearCart(HttpContext);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var user = f["nameUser"];
            var pass = f["password"];
            pass = function.HashPassword(pass);
            var url = f["url"];

            if (ModelState.IsValid)
            {
                User u = dt.Users.SingleOrDefault(n => n.nameUser == user && n.password == pass);
                if (u != null)
                {
                    Session["User"] = u;
                    if (string.IsNullOrEmpty(url))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(url);
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }
       /* private checkUser(string user, string pass)
        {

        }*/
        [HttpPost]
        public ActionResult Register(FormCollection f)
        {
            var user = f["nameUser"];
            var passTrue = f["password"];
            var confirmPass = f["rePassword"];
            var email = f["email"];
            var phone = f["phone"];
            var address = f["address"];
            var fullName = f["fullName"];
            var url = f["url"];

            var errorMessages = ValidateRegistration(user, passTrue, confirmPass, email, phone, address, fullName);

            if (errorMessages.Any())
            {
                ViewData["ThongBao"] = errorMessages;
                return View();
            }

            // Hash mật khẩu
            var pass = function.HashPassword(passTrue);

            if (ModelState.IsValid)
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
                form.Add("password", passTrue);
                form.Add("url", url);
                return Login(form);
            }

            return View();
        }

        private List<string> ValidateRegistration(string user, string passTrue, string confirmPass, string email, string phone, string address, string fullName)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrEmpty(user))
            {
                errorMessages.Add("Vui lòng nhập tên đăng nhập");
            }
            else if (dt.Users.SingleOrDefault(n => n.nameUser == user) != null)
            {
                errorMessages.Add("Tên đăng nhập đã tồn tại");
            }

            if (string.IsNullOrEmpty(passTrue))
            {
                errorMessages.Add("Vui lòng nhập mật khẩu");
            }
            else if (!IsPasswordValid(passTrue))
            {
                errorMessages.Add("Mật khẩu phải chứa ít nhất 8 ký tự, bao gồm chữ cái, số và ký tự đặc biệt");
            }
            if (string.IsNullOrEmpty(confirmPass))
            {
                errorMessages.Add("Vui lòng nhập nhập lại mật khẩu");
            }
            else if (passTrue != confirmPass)
            {
                errorMessages.Add("Mật khẩu không khớp");
            }
            if (string.IsNullOrEmpty(fullName))
            {
                errorMessages.Add("Vui lòng nhập họ tên");
            }

            if (string.IsNullOrEmpty(phone))
            {
                errorMessages.Add("Vui lòng nhập số điện thoại");
            }
            if (string.IsNullOrEmpty(email))
            {
                errorMessages.Add("Vui lòng nhập địa chỉ email");
            }
            else if (dt.Users.SingleOrDefault(n => n.email == email) != null)
            {
                errorMessages.Add("Email đã được sử dụng");
            }

            if (string.IsNullOrEmpty(address))
            {
                errorMessages.Add("Vui lòng nhập địa chỉ");
            }

            return errorMessages;
        }

        private bool IsPasswordValid(string password)
        {
            // Kiểm tra mật khẩu có ít nhất 8 ký tự, bao gồm chữ cái, số và ký tự đặc biệt
            var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
            return regex.IsMatch(password);
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        /*public ActionResult ChangePassword(FormCollection f)
        {
            User u = (User)Session["User"];
            if (u.password != f["password"])
            {
                ViewBag.ERROR = "Mật khẩu không đúng.";
                return View();
            }

            else if (f["newPassword"] != f["Mật khẩu và xác nhận mật khẩu không khớp!"])
            {
                ViewBag.ERROR = "Mật khẩu không đúng.";
                return View();
            }
            u.password = f["newPassword"];
            dt.SubmitChanges();
            return RedirectToAction("Index","Home");
        }*/
        public JsonResult ChangePassword(string password, string newPassword, string reNewPassword)
        {
            try
            {
                User u = (User)Session["User"];
                var user = dt.Users.SingleOrDefault(n => n.idUser == u.idUser);
                string message;
                if (string.IsNullOrEmpty(newPassword))
                {
                    message = "Bạn chưa nhập mật khẩu mới.";
                    return Json(new { code = false, msg = message }, JsonRequestBehavior.AllowGet);
                }
                /*else if (!IsPasswordValid(newPassword))
                {
                    message = "Mật khẩu mới phải chứa ít nhất 8 ký tự, bao gồm chữ cái, số và ký tự đặc biệt";
                    return Json(new { code = false, msg = message }, JsonRequestBehavior.AllowGet);
                }*/
                else if (newPassword.Length < 8)
                {
                    message = "Mật khẩu mới phải có 8 kí tự";
                    return Json(new { code = false, msg = message }, JsonRequestBehavior.AllowGet);
                }
                else if (!newPassword.Any(char.IsDigit))
                {
                    message = "Mật khẩu mới phải có ít nhất một số";
                    return Json(new { code = false, msg = message }, JsonRequestBehavior.AllowGet);
                }
                else if (!newPassword.Any(char.IsPunctuation))
                {
                    message = "Mật khẩu mới phải có ít nhất một ký tự đặc biệt";
                    return Json(new { code = false, msg = message }, JsonRequestBehavior.AllowGet);
                }

                else if (newPassword != reNewPassword)
                {
                    message = "Mật khẩu và xác nhận mật khẩu không khớp!";
                    return Json(new { code = false, msg = message }, JsonRequestBehavior.AllowGet);
                }
                else if (function.VerifyPassword(newPassword, u.password))
                {
                    message = "Mật khẩu không đúng.";
                    return Json(new { code = false, msg = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    user.password = function.HashPassword(newPassword);
                    dt.SubmitChanges();
                    message = "Đổi mật khẩu thành công!";
                    Session["User"] = user;
                    return Json(new { code = true, msg = message }, JsonRequestBehavior.AllowGet);
                }                
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi "+ ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
            try
            {
                var user = dt.Users.SingleOrDefault(s => s.idUser == id);

                var existingUser = dt.Users.SingleOrDefault(s => s.email == f["email"]);
                if (string.IsNullOrEmpty(f["email"]))
                {
                    TempData["Error"] = "Email không được để trống.";
                    return View(user);
                }
                // Kiểm tra tên
                else if (!System.Text.RegularExpressions.Regex.IsMatch(f["email"], @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$"))
                {
                    TempData["Error"] = "Email không đúng định dạng.";
                    return View(user);
                }
                else
                if (existingUser != null && existingUser.idUser != id)
                {
                    TempData["Error"] = "Email này đã được sử dụng bởi một tài khoản khác.";
                    return View(user);
                }

                if (string.IsNullOrEmpty(f["fullName"]))
                {
                    TempData["Error"] = "Họ và tên không được để trống.";
                    return View(user);
                }
                // Kiểm tra tên
                else if (System.Text.RegularExpressions.Regex.IsMatch(f["fullName"], @"\d"))
                {
                    TempData["Error"] = "Họ và tên không được chứa số.";
                    return View(user);
                }

                if (string.IsNullOrEmpty(f["address"]))
                {
                    TempData["Error"] = "Địa chỉ không được để trống.";
                    return View(user);
                }

                if (string.IsNullOrEmpty(f["phone"]))
                {
                    TempData["Error"] = "Số điện thoại không được để trống.";
                    return View(user);
                }
                // Kiểm tra tên
                else if(f["phone"].Length != 10 || !System.Text.RegularExpressions.Regex.IsMatch(f["phone"], @"^\d{10}$"))
                {
                    TempData["Error"] = "Số điện thoại phải là 10 số.";
                    return View(user);
                }   

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
                Session["User"] = user;
                TempData["Success"] = "Thông tin tài khoản đã được cập nhật thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật thông tin tài khoản: " + ex.Message;
            }
            return View(dt.Users.SingleOrDefault(s => s.idUser == id));
        }

        [HttpGet]
        public ActionResult ForgotPassword(string url)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(FormCollection form)
        {
            string email = form["email"];
            var user = dt.Users.SingleOrDefault(s => s.email == email);
            if (user == null)
            {
                ViewBag.ThongBao = "Email không hơp lệ.";
                return View();
            }
            ViewBag.ThongBaoThanhCong = "Email hợp lệ. Mật khẩu đã được gủi qua mail của bạn.";
            string newPassTrue = func.GenerateRandomPassword();
            string newPass = function.HashPassword(newPassTrue);
            func.SendMail(email, "Quên mật khẩu", "Chào bạn,\nMật khẩu mới của bạn là: " + newPassTrue + ".\nTrân trọng.");
            user.password = newPass;
            dt.SubmitChanges();
            return View();
        }
        /*private string _facebookAppId = ConfigurationManager.AppSettings["facebookAppId"];
        private string _facebookAppSecret = ConfigurationManager.AppSettings["facebookAppSecret"];

        [AllowAnonymous]
        public ActionResult ExternalLogin()
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl = "/" });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Facebook");

            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            // Kiểm tra xem có nhận được thông tin đăng nhập từ Facebook không
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var identity = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie);

            // Lưu thông tin đăng nhập vào cookie
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return RedirectToLocal(returnUrl);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }*/

    }
}