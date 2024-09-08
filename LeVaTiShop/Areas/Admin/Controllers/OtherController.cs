using LeVaTiShop.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;

namespace LeVaTiShop.Areas.Admin.Controllers
{
    public class OtherController : Controller
    {
        function f = new function();
        dtDataContext dt = new dtDataContext();
        // GET: Admin/Other
        public ActionResult Index()
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area=""});
            }
            AdminController.ActionName = "Other";
            return View();
        }
        public ActionResult AddSlide()
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View();
        }
        [HttpGet]
        public JsonResult listProduct()
        {
            try
            {
                var listPro = dt.Products.Where(s => s.isDeleted == false).Select(s => new {s.idProduct,s.nameProduct}).ToList();
                return Json(new { code = true, listPro = listPro, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult listUserDeleted()
        {
            try
            {
                var listUser = dt.Users.Where(s => s.isDeleted==true).Select(s => new { s.idUser, s.nameUser,s.email, s.role }).ToList();
                return Json(new { code = true, listUser = listUser, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult listProductDeleted()
        {
            try
            {
                var listProduct = dt.Products.Where(s => s.isDeleted == true).Select(s => new { s.idProduct, s.nameProduct }).ToList();
                List<string> brand = dt.Products.Where(s => s.isDeleted == true).Select(s => s.Brand.nameBrand).ToList();
                List<string> image = dt.Products.Where(s => s.isDeleted == true).Select(s => s.image).ToList();

                for (int i = 0; i < image.Count; i++)
                {

                    f.analysisImage(image[i], out List<string> c, out List<string> im);
                    // Truy cập và chỉnh sửa giá trị của phần tử thứ i trong danh sách
                    image[i] = im[0];
                }
                return Json(new { code = true, listProduct = listProduct,image=image,brand=brand, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult RestoreUser(int id)
        {
            try
            {
                var user = dt.Users.SingleOrDefault(s => s.idUser == id);
                user.isDeleted = false;
                dt.SubmitChanges();
                return Json(new { code = true, msg = "Khôi phục tài khoản thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult RestoreProduct(int id)
        {
            try
            {
                var pro = dt.Products.SingleOrDefault(s => s.idProduct == id);
                pro.isDeleted = false;
                dt.SubmitChanges();
                return Json(new { code = true, msg = "Khôi phục sản phẩm thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AddSlide(string selectedOption, HttpPostedFileBase imageFile)
        {
            try
            {
                if ((!(bool)Session["Admin"]) || string.IsNullOrEmpty(Session["Admin"].ToString()))
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                // Xử lý logic tại đây, sử dụng selectedOption và imageFile
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var slideShow = new SlideShow { idProduct = int.Parse(selectedOption), show = true };

                    // Tạo tên mới cho tệp ảnh
                    string fileName = "sl" + selectedOption + Path.GetExtension(imageFile.FileName);

                    // Đường dẫn tới thư mục lưu trữ tệp ảnh
                    string imagePath = Server.MapPath("~/set/img/slideshow/"); // Thay đổi đường dẫn thư mục nếu cần

                    // Tạo đường dẫn đầy đủ cho tệp ảnh
                    string fullPath = Path.Combine(imagePath, fileName);

                    // Lưu tệp ảnh vào thư mục chỉ định
                    imageFile.SaveAs(fullPath);
                    dt.SlideShows.InsertOnSubmit(slideShow);
                    dt.SubmitChanges();

                    // Hiển thị thông báo thành công trong giao diện người dùng
                    TempData["SuccessMessage"] = "Thêm slide thành công.";

                    return View("AddSlide");
                }

                return View("AddSlide");
            }
            catch (Exception ex)
            {
                var ms = ex.Message;
                return View("AddSlide");
            }
        }
        /*[HttpPost]
        public JsonResult AddSlide(string selectedOption, HttpPostedFileBase imageFile)
        {
            try
            {
                // Xử lý logic tại đây, sử dụng selectedOption và imageFile
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var slideShow = new SlideShow { idProduct = int.Parse(selectedOption), show=true };
                    // Tạo tên mới cho tệp ảnh
                    string fileName = "sl"+slideShow.idSlide + Path.GetExtension(imageFile.FileName);

                // Đường dẫn tới thư mục lưu trữ tệp ảnh
                    string imagePath = Server.MapPath("~/set/img/slideshow/"); // Thay đổi đường dẫn thư mục nếu cần

                    // Tạo đường dẫn đầy đủ cho tệp ảnh
                    string fullPath = Path.Combine(imagePath, fileName);

                    // Lưu tệp ảnh vào thư mục chỉ định
                    imageFile.SaveAs(fullPath);
                    dt.SlideShows.InsertOnSubmit(slideShow);
                    dt.SubmitChanges();
                    // Tiếp tục xử lý logic khác hoặc trả về kết quả tùy ý
                    return Json(new { code = true, msg = "Thêm slide thành công." }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { code = false , msg = "Chưa chọn ảnh." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }*/
    }
}