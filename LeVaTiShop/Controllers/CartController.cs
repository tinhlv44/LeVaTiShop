using LeVaTiShop.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebGrease;
using log4net;
using log4net.Repository.Hierarchy;

namespace LeVaTiShop.Controllers
{
    public class CartController : Controller
    {
        function f = new function();
        dtDataContext dt = new dtDataContext();
        // Action để thêm sản phẩm vào giỏ hàng
        /*public ActionResult AddToCart(int id, string img, decimal price)
        {

            var item = new CartItem(id, img, price);

            CartHelper.AddToCart(HttpContext, item);

            return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ hoặc trang giỏ hàng

        }*/
        public JsonResult AddToCart(int id, string img, decimal price)
        {
            try
            {

                var item = new CartItem(id, img, price);

                CartHelper.AddToCart(HttpContext, item);
                return Json(new { code = true, msg = "Thêm vào giỏ hàng thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Thêm vào giỏ hàng thất bạo! Lỗi "+ ex.Message }, JsonRequestBehavior.AllowGet);
            }

            //return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ hoặc trang giỏ hàng

        }
        public ActionResult Cart()
        {
            var cartItems = CartHelper.GetCartItems(HttpContext);
            if (cartItems.Count == 0 || cartItems == null)
            {
                return View();
            }
            return View(cartItems);
        }
        public ActionResult Delete(int id)
        {
            CartHelper.RemoveFromCart(HttpContext, id);
            return RedirectToAction("Cart");
        }

        public ActionResult ClearCart()
        {
            CartHelper.ClearCart(HttpContext);
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int newQuantity)
        {
            CartHelper.UpdateCartItemQuantity(HttpContext, id, newQuantity);
            return RedirectToAction("Cart"); // Chuyển hướng đến trang giỏ hàng hoặc nơi cần thiết khác
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            //kiem tra dang nhap
            ViewBag.Total = CartHelper.Sum(HttpContext);
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("Login", "Login", new { url = "/Cart/Checkout" });
            }
            return View(CartHelper.GetCartItems(HttpContext));
        }
        //[HttpPost]
        public ActionResult OrderConfirmation()
        {
            InsertOrder(0, false);
            ViewBag.Money = CartHelper.Sum(HttpContext);
            CartHelper.ClearCart(HttpContext);
            return View();
        }
        public ActionResult SuccessPayment()
        {
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
            var vnpayData = Request.QueryString;
            VnPayLibrary vnpay = new VnPayLibrary();

            foreach (string s in vnpayData)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }
            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
            //vnp_TransactionNo: Ma GD tai he thong VNPAY
            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
            //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

            long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
            String TerminalID = Request.QueryString["vnp_TmnCode"];
            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            String bankCode = Request.QueryString["vnp_BankCode"];

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    //Thanh toan thanh cong
                    ViewBag.Msg = "Đặt hàng và thanh toán số tiền " + CartHelper.Sum(HttpContext) + " VNĐ thành công.\n Chúng tôi đã ghi nhận đơn đặt hàng của bạn.\nBạn sẽ nhận thông tin sớm nhất từ chúng tôi qua email.\n";
                    //ViewBag.Msg = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ.";
                    InsertOrder(1, true);
                    CartHelper.ClearCart(HttpContext);
                }
                else
                {
                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                    ViewBag.Msg = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                }
                ViewBag.Msg += "Mã Website (Terminal ID):" + TerminalID;
                ViewBag.Msg += "\nMã giao dịch thanh toán:" + orderId.ToString();
                ViewBag.Msg += "\nMã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                ViewBag.Msg += "\nSố tiền thanh toán (VND):" + vnp_Amount.ToString();
                ViewBag.Msg += "\nNgân hàng thanh toán:" + bankCode;
            }
            else
            {
                ViewBag.Msg = "Có lỗi xảy ra trong quá trình xử lý";
            }
            return View();
        }
        public void InsertOrder(int stateOd, bool Pay)
        {
            var cartItems = CartHelper.GetCartItems(HttpContext);
            User u = (User)Session["User"];
            // Tạo đối tượng Order và truyền thông tin từ đơn hàng và thanh toán vào đó
            var dateNow = DateTime.Now;
            var order = new Order
            {
                idUser = u.idUser,
                state = stateOd,
                isPay = Pay,
                dateOrder = dateNow
                // Các thuộc tính khác liên quan đến đơn hàng
            };
            dt.Orders.InsertOnSubmit(order);
            dt.SubmitChanges();
            string subject = "Đặt hàng tại H&T Shop thành công - " + dateNow;
            string body = "Chào bạn,\nBạn đã đặt hàng thành công tại Shop của chúng tôi.\nNhững sản phẩm bạn đặt bao gồm:\n";
            foreach (CartItem c in cartItems)
            {
                var deatailOrder = new DetailOrder
                {
                    idOrder = order.idOrder,
                    idProduct = c.ID,
                    quantity = c.quantity,
                    total = c.Total,
                    version = "#"+c.image
                };
                dt.DetailOrders.InsertOnSubmit(deatailOrder);
                dt.SubmitChanges();
                var product = dt.Products.SingleOrDefault(s => s.idProduct == c.ID);
                f.analysisImage(product.inventory, out List<string> sI, out List<string> sC);
                int v = 0;
                for (int i = 0; i < sI.Count; i++)
                {
                    if (sI[i] == c.image.Substring(0, c.image.IndexOf('.')))
                    {
                        v = i; 
                        break;
                    }
                }
                f.amountProduct(product.inventory, out List<string> amount);
                List<int> amountTrue = new List<int>();
                for (int i=0; i< amount.Count; i++)
                {
                    int s = int.Parse(amount[i]);
                    if (i == v)
                    {
                        s -= c.quantity;
                    }
                    amountTrue.Add(s);
                }
                string invemtory = "";
                foreach (var i in amountTrue)
                {
                    invemtory += "#" + i;
                }
                product.inventory = invemtory;

                dt.SubmitChanges();
                body += c.quantity + "x" + c.nameProduct + " : " + c.Total + " VNĐ\n";
            }
            body += "Tổng số tiền bạn phải thanh toán là: " + CartHelper.Sum(HttpContext) + "VNĐ\n";
            body += "Chúng tôi sẽ liên hệ giao hàng tận nơi từ 1 đến 7 ngày.\n";
            body += "Mọi thắc mắc xin liên hệ:\nSĐT: 09********.\nEmail: hntshop@gmail.com.\nTrân trọng.";
            f.SendMail(u.email, subject, body);
            var message = new LeVaTiShop.Models.Message
            {
                idUser = u.idUser,
                date = dateNow,
                messageContent = "M_Đơn hàng cảu bạn đang ở trạng thái " + f.getState(stateOd, out string state, out string hex)
            };
            dt.Messages.InsertOnSubmit(message);
            dt.SubmitChanges();
            
        }
        [HttpGet]
        public ActionResult FastCheckout(int id, string img, decimal price)
        {
            try
            {
                // Kiểm tra đăng nhập
                var addToCartResult = AddToCart(id, img, price);
                if (Session["User"] == null || Session["User"].ToString() == "")
                {
                    return RedirectToAction("Login", "Login", new { url = "/Cart/Checkout" });
                }
                var addToCartData = addToCartResult.Data as dynamic;
                bool code = addToCartData.code;
                string msg = addToCartData.msg;

                // Thêm vào giỏ hàng

                if (code)
                {
                    ViewBag.Total = CartHelper.Sum(HttpContext);
                    return RedirectToAction("Checkout", "Cart", CartHelper.GetCartItems(HttpContext));
                }
                else
                {
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Thêm vào giỏ hàng thất bại! Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CartDetail(int idOrder)
        {
            return View(dt.DetailOrders.Where(s=>s.idOrder==idOrder).ToList());
        }
        /*public ActionResult PlaceOrder(Order order)
        {
            // Thực hiện các xử lý liên quan đến đặt hàng thành công, ví dụ: lưu vào cơ sở dữ liệu, gửi email xác nhận, v.v.

            return View(order);
        }*/

        public ActionResult ProcessPayment()
        {
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key


            //Get payment input
            /*User u = (User)Session["User"];
            // Tạo đối tượng Order và truyền thông tin từ đơn hàng và thanh toán vào đó
            var dateNow = DateTime.Now;
            var order = new Order
            {
                idUser = u.idUser,
                state = 1,
                isPay = false,
                dateOrder = dateNow
                // Các thuộc tính khác liên quan đến đơn hàng
            };
            dt.Orders.InsertOnSubmit(order);
            dt.SubmitChanges();

            var cartItems = CartHelper.GetCartItems(HttpContext);
            string subject = "Đặt hàng tại H&T Shop thành công - " + dateNow;
            string body = "Chào bạn,\nBạn đã đặt hàng và thanh toán thành công tại Shop của chúng tôi.\nNhững sản phẩm bạn đặt bao gồm:\n";
            foreach (CartItem c in cartItems)
            {
                var deatailOrder = new DetailOrder
                {
                    idOrder = order.idOrder,
                    idProduct = c.ID,
                    quantity = c.quantity,
                    total = c.Total
                };
                dt.DetailOrders.InsertOnSubmit(deatailOrder);
                dt.SubmitChanges();
                var product = dt.Products.SingleOrDefault(s => s.idProduct == c.ID);
                product.inventory -= c.quantity;
                dt.SubmitChanges();
                body += c.quantity + "x" + c.nameProduct + " : " + c.Total + " VNĐ\n";
            }
            body += "Tổng số tiền bạn phải thanh toán là: " + CartHelper.Sum(HttpContext) + "VNĐ\n";
            body += "Chúng tôi sẽ liên hệ giao hàng tận nơi từ 1 đến 7 ngày.\n";
            body += "Mọi thắc mắc xin liên hệ:\nSĐT: 09********.\nEmail: hntshop@gmail.com.\nTrân trọng.";
            f.SendMail(u.email, subject, body);

            var message = new LeVaTiShop.Models.Message
            {
                idUser = u.idUser,
                date = dateNow,
                messageContent = "M_Đơn hàng cảu bạn đang ở trạng thái " + f.getState(1, out string state, out string hex)
            };
            dt.Messages.InsertOnSubmit(message);
            dt.SubmitChanges();*/



            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (CartHelper.Sum(HttpContext) * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            
            //vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            ///vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            int nextOrderId = 1; // Default value if Orders collection is empty

            if (dt.Orders != null && dt.Orders.Any())
            {
                nextOrderId = dt.Orders.Max(s => s.idOrder) + 1;
            }
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang: " + nextOrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", nextOrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return Redirect(paymentUrl);
        }
    }
}