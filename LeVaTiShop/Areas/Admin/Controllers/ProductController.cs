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

namespace LeVaTiShop.Areas.Admin.Controllers
{    
    public class ProductController : Controller
    {
        dtDataContext dt = new dtDataContext();
        // GET: Admin/Product
        [HttpGet]
        public ActionResult Index()
        {
            AdminController.ActionName = "ManagerProducts";
            var res = dt.Products.ToList();
            return View(res);
        }
        [HttpPost]
        public ActionResult Index(string key)
        {
            AdminController.ActionName = "ManagerProducts";
            var res = dt.Products.Where(p=>p.idProduct.ToString().Contains(key)||p.nameProduct.Contains(key)|| p.Brand.nameBrand.Contains(key)|| p.Category.name.Contains(key)).ToList();
            return View(res);
        }
        public ActionResult ProductDetail(int id)
        {
            AdminController.ActionName = "ManagerProducts";
            return View(dt.Products.Where(s => s.idProduct == id).Single());
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand");
            ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProduct(FormCollection f, HttpPostedFileBase[] images, string[] colors, string[] Hspecification, string[] specification)
        {
            if (ModelState.IsValid)
            {
                Product model = new Product();
                ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand");
                ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name");
                model.nameProduct = f["nameProduct"];
                model.description = f["description"];
                model.inventory = int.Parse(f["inventory"].Replace(".", ""));
                model.price = decimal.Parse(f["price"].Replace(".",""));
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
                model.specifications = f["specifications"];
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
                if (string.IsNullOrEmpty(model.specifications))
                {
                    string temp = "";
                    for (int i = 0; i < Hspecification.Length; i++)
                    {
                        temp += "@(" + Hspecification[i] + "){" + specification[i] + "}";
                    }
                    model.specifications = temp;
                }
                dt.Products.InsertOnSubmit(model);
                dt.SubmitChanges();
                // Và sau đó, lưu thay đổi trong cơ sở dữ liệu của bạn
                // Ví dụ: dbContext.SaveChanges();

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

                            // Lưu thông tin về ảnh vào cơ sở dữ liệu
                            // Code để lưu thông tin ảnh vào cơ sở dữ liệu của bạn ở đây
                            // Ví dụ: var imageModel = new ImageModel { ProductId = model.Id, FileName = fileName, Color = colors[i] };
                            // Và sau đó, lưu thay đổi trong cơ sở dữ liệu của bạn
                            // Ví dụ: dbContext.Images.Add(imageModel);
                            // Và sau đó, lưu thay đổi trong cơ sở dữ liệu của bạn
                            // Ví dụ: dbContext.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            var p = dt.Products.SingleOrDefault(s=>s.idProduct == id);
            ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand", p.idBrand);
            ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name", p.idCategory);
            return View(p);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProduct(FormCollection f, HttpPostedFileBase[] images, string[] colors, string[] Hspecification, string[] specification)
        {
            if (ModelState.IsValid)
            {
                Product model = dt.Products.FirstOrDefault(p => p.idProduct == int.Parse(f["id"]));
                ViewBag.Brand = new SelectList(dt.Brands.ToList().OrderBy(n => n.nameBrand), "idBrand", "nameBrand");
                ViewBag.Category = new SelectList(dt.Categories.ToList().OrderBy(n => n.name), "idCategory", "name");
                model.nameProduct = f["nameProduct"];
                model.description = f["description"];
                model.inventory = int.Parse(f["inventory"].Replace(".", ""));
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
                model.specifications = f["specifications"];
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
                if (string.IsNullOrEmpty(model.specifications))
                {
                    string temp = "";
                    for (int i = 0; i < Hspecification.Length; i++)
                    {
                        temp += "@(" + Hspecification[i] + "){" + specification[i] + "}";
                    }
                    model.specifications = temp;
                }
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
        public ActionResult DeleteProduct()
        {
            return View();
        }
    }
}