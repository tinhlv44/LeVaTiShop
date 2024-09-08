using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using PagedList;
using PagedList.Mvc;

namespace LeVaTiShop.Controllers
{
    public class HomeController : Controller
    {
        dtDataContext dt = new dtDataContext();
        public static string page = "chu";
        public ActionResult Index(int ? pa)
        {
            int iSize = 8;
            int iPageNumber = (pa ?? 1);
            if (Session["lP"] == null) { 
            }
            page = "chu";
            return View(dt.Products.Where(p=>p.isFeatured==true&&!p.isDeleted).ToPagedList(iPageNumber, iSize));
        }
        public ActionResult Search(decimal priceForm, decimal priceTo, string nameBrand=null)
        {
            IPagedList<Product> lP;
            if (nameBrand=="chu")
            {
                lP = dt.Products.Where(s => (s.isDiscounted == true && s.discountedPrice > priceForm && s.discountedPrice < priceTo && s.isFeatured == true && !s.isDeleted) || (s.isDiscounted == false && s.price > priceForm && s.price < priceTo && s.isFeatured == true && !s.isDeleted)).ToList().ToPagedList(1,8);
            }
            else if (nameBrand== "khuyen-mai")
            {
                page = nameBrand;
                lP = dt.Products.Where(s => (s.isDiscounted == true && s.discountedPrice > priceForm && s.discountedPrice < priceTo && s.isFeatured == true && !s.isDeleted) ).ToList().ToPagedList(1, 8);
            }
            else
            {
                page = nameBrand;
                lP = dt.Products.Where(s => (s.isDiscounted == true && s.discountedPrice > priceForm && s.discountedPrice < priceTo && s.Brand.nameBrand == nameBrand && !s.isDeleted) || (s.isDiscounted == false && s.price > priceForm && s.price < priceTo && s.Brand.nameBrand == nameBrand && !s.isDeleted)).ToPagedList(1,8);
            }
            return View("Index", lP);
        }
        public ActionResult Header()
        {
            var cartItems = CartHelper.GetCartItems(HttpContext);
            ViewBag.CartItems = cartItems;
            return PartialView();
        }
        public ActionResult Sidebar()
        {
            return PartialView();
        }
        public ActionResult Menu()
        {
            return PartialView(dt.Brands.ToList());
        }
        public ActionResult SlideShow()
        {
            return PartialView();
        }
        public ActionResult DetailProduct(int id)
        {
            var x = dt.Products.Where(s=>s.idProduct == id).Single();
            return PartialView(x);
        }
        [HttpGet]
        public ActionResult Help()
        {
            return View();
        }
        public ActionResult Notify()
        {
            User u = (User)Session["User"];
            if (u != null)
            {
                var messages = dt.Messages.Where(s => s.idUser == u.idUser && s.messageContent[0]=='M').ToList();
                List<int> id = new List<int>();
                foreach(var message in messages)
                {
                    int i = dt.Orders.FirstOrDefault(s => s.dateOrder == message.date)?.idOrder ?? default(int);
                    if (i == 0)
                    {
                    }
                    else
                    {
                        id.Add(i);
                    }
                }
                ViewBag.ListIDOrder = id;
                return PartialView(messages);
            }
            return PartialView(new List<LeVaTiShop.Models.Message>());
        }
        public ActionResult Comment(int id)
        {
            return PartialView(id);
        }
        [HttpPost]
        public JsonResult Help(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Json(new {code=false, msg= "Phản hồi không hợp lệ. Hãy thử lại." },JsonRequestBehavior.AllowGet);
            }
            User u = (User)Session["User"];
            var mes = new LeVaTiShop.Models.Message();
            if (u == null)
            {
                mes.idUser = 0;
            }
            else
            {
                mes.idUser = u.idUser;
            }
            mes.date = DateTime.Now;
            mes.messageContent = message;
            dt.Messages.InsertOnSubmit(mes);
            dt.SubmitChanges();
            return Json(new { code = true, msg = "Phản hồi thành công. Chúng tôi đã ghi nhận câu hỏi của bạn." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listProductComment(int id)
        {
            try
            {
                var listComment = dt.Comments.Where(s => s.idProduct==id).Select(s => new { s.idUser, s.date, s.comment1 }).ToList();
                List<string> avt = new List<string>();
                List<string> date = new List<string>();
                List<string> role = new List<string>();
                List<string> name = new List<string>();
                for (int i=0; i< listComment.Count; i++)
                {
                    var u = dt.Users.Where(s => s.idUser == listComment[i].idUser).SingleOrDefault();
                    string str = u.avatar;
                    avt.Add(str); ;
                    role.Add(u.role?"Admin": "Khách hàng");
                    name.Add(u.fullName);
                    str = listComment[i].date.Value.ToString("dd-MM-yyyy");
                    date.Add(str);
                }
                return Json(new { code = true, listComment = listComment, avt = avt, date = date, role = role, name = name, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Accessory(int? pa)
        {
            page = "phu-kien";
            int iSize = 8;
            int iPageNumber = (pa ?? 1);
            return View((dt.Products.Where(s=>!s.isDeleted&&s.Category.idCategory==2)).ToPagedList(iPageNumber, iSize));
        }

        [HttpPost]
        public JsonResult AddComment(string ct, string idUser, string idProduct)
        {
            try
            {
                var u = dt.Comments.SingleOrDefault(s => s.idUser == int.Parse(idUser)&& s.idProduct == int.Parse(idProduct));
                if (u != null)
                {
                    u.comment1 = ct;
                    u.date = DateTime.Now;
                    dt.SubmitChanges();
                    return Json(new { code = true, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
                }
                var cmt = new Comment { idUser = int.Parse(idUser), idProduct = int.Parse(idProduct), comment1 = ct, date = DateTime.Now, liked = false };
                dt.Comments.InsertOnSubmit(cmt);
                dt.SubmitChanges();
                return Json(new { code = true, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    } 
}