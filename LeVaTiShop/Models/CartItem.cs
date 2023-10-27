using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace LeVaTiShop.Models
{
    public class CartItem
    {
        dtDataContext dt = new dtDataContext();
        [DisplayName("Mã sản phẩm")]
        public int ID { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string nameProduct { get; set; }
        [DisplayName("Ảnh bìa")]
        public string image { get; set; }
        [DisplayName("Giá")]
        public decimal price { get; set; }
        [DisplayName("Số lượng")]
        public int quantity { get; set; }
        [DisplayName("Thành tiền")]
        public int Total
        {
            get
            {
                return (int)(price * quantity);
            }
        }
        public CartItem(int id, string img, decimal price)
        {
            ID = id;
            Product p = dt.Products.Single(n => n.idProduct == ID);
            nameProduct = p.nameProduct;
            image = img;

            this.price = price;
            quantity = 1;
        }
        public void UpdateQuantity(int newQuantity)
        {
            quantity = newQuantity;
        }
    }

}