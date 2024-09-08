using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace LeVaTiShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        function f = new function();
        dtDataContext dt = new dtDataContext();
        // GET: Admin/Order
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            AdminController.ActionName = "ManagerOrders";
            return View(dt.Orders.ToList());
        }
        [HttpPost]
        public ActionResult Index(string key)
        {
            AdminController.ActionName = "ManagerOrders";
            var res = dt.Orders
                .Where(o => o.idOrder.ToString().Contains(key)
                            || o.idUser.ToString().Contains(key)
                            || o.User.fullName.Contains(key)
                            || o.dateOrder.Day.ToString().Contains(key)
                            || o.dateOrder.Year.ToString().Contains(key)
                            || o.dateOrder.Month.ToString().Contains(key)
                            || o.dateOrder.Hour.ToString().Contains(key)
                            || o.dateOrder.Minute.ToString().Contains(key)
                            || o.dateOrder.Second.ToString().Contains(key)
                            || (o.state==0? "Chờ duyệt".Contains(key):(o.state == 1 ? "Đang xử lí".Contains(key) : (o.state == 2 ? "Đang giao hàng".Contains(key):(o.state == 3 ? "Hoàn thành".Contains(key):"Hủy".Contains(key)))))
                            //|| (f.getState(o.state, out s, out h).Contains(key)?true:false)
                )
                .ToList();

            return View(res);
        }
        public ActionResult DetailOrder(int id)
        {
            if (Session["Admin"] == null || !(bool)Session["Admin"])
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View(dt.DetailOrders.Where(s=>s.idOrder==id).ToList());
        }
        [HttpPost]
        public JsonResult updateState(int idOrder, int state)
        {
            try
            {
                var order = dt.Orders.FirstOrDefault(o => o.idOrder == idOrder);
                if (order != null)
                {
                    order.state = state;
                    var message = dt.Messages.SingleOrDefault(m => m.date == order.dateOrder);
                    string messageState = "M_Đơn hàng của bạn đang ở trạng thái " + f.getState(state, out string s, out string h);
                    if (message == null)
                    {
                        message = new LeVaTiShop.Models.Message { idUser = order.idUser, date = order.dateOrder, messageContent = messageState };
                        dt.Messages.InsertOnSubmit(message);
                        dt.SubmitChanges();
                    }
                    else
                    {
                        message.messageContent = messageState;
                        dt.SubmitChanges();
                    }
                    return Json(new { code = true, msg = "Bạn đã thay đổi trạng thái đơn hàng thành công." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = false, msg = "Lỗi " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult updateIspay(int idOrder)
        {
            try
            {
                var order = dt.Orders.FirstOrDefault(o => o.idOrder == idOrder);
                if (order != null)
                {
                    if (order.isPay)
                    {
                        order.isPay = false;
                    }
                    else
                    {
                        order.isPay = true;
                    }
                    dt.SubmitChanges();
                    return Json(new { code = true, msg = "Bạn đã thay đổi thanh toán thành công." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = false, msg = "Lỗi " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                return Json(new { code = false, msg = "Lỗi " + ex.Message  }, JsonRequestBehavior.AllowGet);
            }
        }
        /*[HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            try
            {
                var pro = dt.Products.SingleOrDefault(s => s.idProduct == id);
                pro.isDeleted = true;
                dt.SubmitChanges();
                return Json(new { code = true, msg = "Xóa sản phẩm thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Xóa sản phẩm thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }*/
    }
}