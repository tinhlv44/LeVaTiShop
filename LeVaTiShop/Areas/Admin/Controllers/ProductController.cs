using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.Ajax.Utilities;
using System.Web.UI.WebControls.WebParts;
using System.Web.WebPages;
using System.Text.RegularExpressions;

namespace LeVaTiShop.Areas.Admin.Controllers
{    
    public class ProductController : Controller
    {
        dtDataContext dt = new dtDataContext();
        // GET: Admin/Product
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            AdminController.ActionName = "ManagerProducts";
            var res = dt.Products.Where(s => s.isDeleted == false).ToList();
            return View(res);
        }
        [HttpPost]
        public ActionResult Index(string key)
        {
            AdminController.ActionName = "ManagerProducts";
            var res = dt.Products.Where(p=>p.idProduct.ToString().Contains(key)||p.nameProduct.Contains(key)|| p.Brand.nameBrand.Contains(key)|| p.Category.name.Contains(key)).Where(s => s.isDeleted == false).ToList();
            return View(res);
        }
        public ActionResult ProductDetail(int id)
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            AdminController.ActionName = "ManagerProducts";
            return View(dt.Products.Where(s => s.idProduct == id).Single());
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand");
            ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProduct(FormCollection f, HttpPostedFileBase[] images, string[] colors, string[] amount, string[] Hspecification, string[] specification, string choose, string specifications)
        {
            // Lưu thông tin người dùng đã nhập vào ViewBag
            ViewBag.nameProduct = f["nameProduct"];
            ViewBag.description = f["description"];
            ViewBag.price = f["price"];
            ViewBag.discountedPrice = f["discountedPrice"];
            ViewBag.Brand = int.Parse(f["Brand"]);
            ViewBag.Category = int.Parse(f["Category"]);
            ViewBag.isDiscounted = !string.IsNullOrEmpty(f["isDiscounted"]) ? bool.Parse(f["isDiscounted"]) : false;
            ViewBag.isFeatured = !string.IsNullOrEmpty(f["isFeatured"]) ? bool.Parse(f["isFeatured"]) : false;
            ViewBag.specifications = choose == "true" ? specifications : string.Join("", Hspecification.Select((h, i) => $"@({h}){{{specification[i]}}}"));

            ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand");
            ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name");

            var errorMessages = new List<string>();
            if (f["nameProduct"].ToString().IsEmpty())
            {
                errorMessages.Add("Vui lòng nhập tên sản phẩm.");
            }

            var price = decimal.Parse(f["price"].Replace(".", ""));
            var discountedPrice = decimal.Parse(f["discountedPrice"].Replace(".", ""));
            if (f["price"].ToString().IsEmpty())
            {
                errorMessages.Add("Vui lòng nhập giá sản phẩm");
            }
            else if (price<10000)
            {
                errorMessages.Add("Giá sản phẩm tối thiểu là 10.000đ");
            }
            if (f["discountedPrice"].ToString().IsEmpty())
            {
                errorMessages.Add("Vui lòng nhập giá giảm cho sản phẩm");
            }
            else if (discountedPrice < 10000)
            {
                errorMessages.Add("Giá giảm giá sản phẩm tối thiểu là 10.000đ");
            }
            if (f["description"].ToString().IsEmpty())
            {
                errorMessages.Add("Vui lòng nhập mô tả sản phẩm");
            }
            if (specifications.ToString().IsEmpty())
            {
                errorMessages.Add("Vui lòng nhập thông số kỹ thuật sản phẩm");
            }
            else if (!IsValidFormat(specifications))
            {
                errorMessages.Add("Thông số kỹ thuật sản phẩm có dạng: @(Tên thông số){Giá trị}");
            }
            if (colors == null || colors.Length == 0)
            {
                errorMessages.Add("Vui lòng tải lên ít nhất một hình ảnh cho sản phẩm");
            }

            if (errorMessages.Any())
            {
                // Lưu thông báo lỗi vào ViewBag
                ViewData["ThongBao"] = errorMessages;
            }
            else 
            {                
                Product model = new Product();
                
                model.nameProduct = f["nameProduct"];
                model.description = f["description"];
                model.price = decimal.Parse(f["price"].Replace(".", ""));
                model.discountedPrice = decimal.Parse(f["discountedPrice"].Replace(".", ""));
                model.idBrand = int.Parse(f["Brand"]);
                model.idCategory = int.Parse(f["Category"]);
                bool isDiscounted = false; // Giá trị mặc định
                if (!string.IsNullOrEmpty(f["isDiscounted"]))
                {
                    isDiscounted = bool.Parse(f["isDiscounted"]);
                }
                model.isDiscounted = isDiscounted;
                bool isFeatured = false; // Giá trị mặc định
                if (!string.IsNullOrEmpty(f["isFeatured"]))
                {
                    isFeatured = bool.Parse(f["isFeatured"]);
                }
                model.isFeatured = isFeatured;

                //Xu li thông số kĩ thuật
                if (choose == "true")
                {
                    model.specifications = specifications;//f["specifications"];
                }
                else
                {
                    string temp = "";
                    for (int i = 0; i < Hspecification.Length; i++)
                    {
                        temp += "@(" + Hspecification[i] + "){" + specification[i] + "}";
                    }
                    model.specifications = temp;
                }
                model.image = "";
                foreach (var s in colors)
                {
                    model.image += s.ToString();
                }

                string t = "";
                for (int i = 0; i < amount.Length; i++)
                {
                    t += "#" + amount[i].Replace(".", "");
                }
                model.inventory = t;

                dt.Products.InsertOnSubmit(model);
                dt.SubmitChanges();

                // Thư mục lưu trữ ảnh sản phẩm
                string productImageDirectory = Server.MapPath("~/set/img/product/" + model.nameProduct);

                // Kiểm tra xem thư mục lưu trữ ảnh sản phẩm đã tồn tại chưa, nếu chưa thì tạo mới
                if (!Directory.Exists(productImageDirectory))
                {
                    Directory.CreateDirectory(productImageDirectory);
                }
                // Lưu ảnh sản phẩm vào thư mục lưu trữ ảnh
                if (images != null && images.Length > 0)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        HttpPostedFileBase image = images[i];

                        if (image != null && image.ContentLength > 0)
                        {
                            // Lấy tên tệp tin ảnh
                            /*string fileName = colors[i].Substring(1);*/
                            string extension = Path.GetExtension(image.FileName);
                            string newFileName2 = colors[i].ToString().Substring(1) + extension;
                            // Tạo đường dẫn đến tệp tin ảnh trong thư mục lưu trữ ảnh sản phẩm
                            string filePath = Path.Combine(productImageDirectory, newFileName2);

                            // Kiểm tra xem tệp tin ảnh đã tồn tại chưa, nếu chưa thì lưu tệp tin ảnh
                            if (!System.IO.File.Exists(filePath))
                            {
                                image.SaveAs(filePath);
                            }
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        public bool IsValidFormat(string input)
        {
            // Biểu thức chính quy cho định dạng @(tên thông số){giá trị}
            string pattern = @"@\(.+?\)\{.+?\}";

            // Tạo một đối tượng Regex với biểu thức chính quy
            Regex regex = new Regex(pattern);

            // Kiểm tra xem chuỗi đầu vào có khớp với biểu thức chính quy hay không
            return regex.IsMatch(input);
        }


    [HttpGet]
        public ActionResult EditProduct(int id)
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            var p = dt.Products.SingleOrDefault(s=>s.idProduct == id);
            ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand", p.idBrand);
            ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name", p.idCategory);
            return View(p);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProduct(FormCollection f, HttpPostedFileBase[] images, string[] colors, string[] amount, string[] Hspecification, string[] specification, string choose, string specifications)
        {
            if (ModelState.IsValid)
            {
                Product model = dt.Products.FirstOrDefault(p => p.idProduct == int.Parse(f["id"]));
                ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand");
                ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name");
                model.nameProduct = f["nameProduct"];
                model.description = f["description"];
                //model.inventory = int.Parse(f["inventory"].Replace(".", ""));
                model.price = decimal.Parse(f["price"].Replace(".", ""));
                model.discountedPrice = decimal.Parse(f["discountedPrice"].Replace(".", ""));
                model.idBrand = int.Parse(f["Brand"]);
                model.idCategory = int.Parse(f["Category"]);
                bool isDiscounted = false; // Giá trị mặc định
                if (!string.IsNullOrEmpty(f["isDiscounted"]))
                {
                    isDiscounted = bool.Parse(f["isDiscounted"]);
                }
                model.isDiscounted = isDiscounted;
                bool isFeatured = false; // Giá trị mặc định
                if (!string.IsNullOrEmpty(f["isFeatured"]))
                {
                    isFeatured = bool.Parse(f["isFeatured"]);
                }
                model.isFeatured = isFeatured;
                //model.specifications = f["specifications"];
                // Thư mục lưu trữ ảnh sản phẩm
                string productImageDirectory = Server.MapPath("~/set/img/product/" + model.nameProduct);

                // Kiểm tra xem thư mục lưu trữ ảnh sản phẩm đã tồn tại chưa, nếu chưa thì tạo mới
                if (!Directory.Exists(productImageDirectory))
                {
                    Directory.CreateDirectory(productImageDirectory);
                }
                // Lưu thông tin sản phẩm vào cơ sở dữ liệu
                // Code để lưu thông tin sản phẩm vào cơ sở dữ liệu của bạn ở đây
                // Ví dụ:
                model.image = "";
                foreach (var s in colors)
                {
                    model.image += s.ToString();
                }
                //Xu li thông số kĩ thuật
                if (choose == "true")
                {
                    model.specifications = specifications;//f["specifications"];
                }
                else
                {
                    string temp = "";
                    for (int i = 0; i < Hspecification.Length; i++)
                    {
                        temp += "@(" + Hspecification[i] + "){" + specification[i] + "}";
                    }
                    model.specifications = temp;
                }
                string t = "";
                for (int i = 0; i < amount.Length; i++)
                {
                    t += "#" + amount[i].Replace(".", "");
                }
                model.inventory = t;

                dt.SubmitChanges();
                // Lưu ảnh sản phẩm vào thư mục lưu trữ ảnh
                if (images != null && images.Length > 0)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        HttpPostedFileBase image = images[i];

                        if (image != null && image.ContentLength > 0)
                        {
                            // Lấy tên tệp tin ảnh
                            /*string fileName = colors[i].Substring(1);*/
                            string extension = Path.GetExtension(image.FileName);
                            string newFileName2 = colors[i].ToString().Substring(1) + extension;
                            // Tạo đường dẫn đến tệp tin ảnh trong thư mục lưu trữ ảnh sản phẩm
                            string filePath = Path.Combine(productImageDirectory, newFileName2);

                            // Kiểm tra xem tệp tin ảnh đã tồn tại chưa, nếu chưa thì lưu tệp tin ảnh
                            if (!System.IO.File.Exists(filePath))
                            {
                                image.SaveAs(filePath);
                            }
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            try
            {
                var pro = dt.Products.SingleOrDefault(s => s.idProduct == id);
                pro.isDeleted = true;
                dt.Products.DeleteOnSubmit(pro);
                dt.SubmitChanges();
                return Json(new { code = true, msg = "Xóa sản phẩm thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Xóa sản phẩm thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}