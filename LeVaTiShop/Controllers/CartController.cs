using LeVaTiShop.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Controllers
{
    public class CartController : Controller
    {
        // Action để thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int id, string img, decimal price)
        {
            
            var item = new CartItem(id, img, price);

            CartHelper.AddToCart(HttpContext, item);

            return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ hoặc trang giỏ hàng
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
        public ActionResult UpdateQuantity(int id, int newQuantity)
        {
            CartHelper.UpdateCartItemQuantity(HttpContext, id, newQuantity);
            return RedirectToAction("Cart"); // Chuyển hướng đến trang giỏ hàng hoặc nơi cần thiết khác
        }

    }
}