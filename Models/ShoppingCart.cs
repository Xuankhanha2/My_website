using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrangChuWebsite.Models
{
    //class tung phan tu cua gio hang
    public class CartItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string photo { get; set; }
        public int number;
        public int toPrice
        {
            get
            {
                return number * price;
            }
        }
    }
    //class gio hang (bao gom nhieu phan tu)
    public class ShoppingCart
    {
        //them moi san pham
        public void CartAdd(CartItem CartItemCart)
        {
            if (HttpContext.Current.Session["Cart"] == null)
            {
                //khoi tao gio hang
                HttpContext.Current.Session["Cart"] = new List<CartItem>();
                List<CartItem> Cart = HttpContext.Current.Session["Cart"] as List<CartItem>;
                Cart.Add(CartItemCart);
                //gan nguoc lai gio hang
                HttpContext.Current.Session["Cart"] = Cart;
            }
            else
            {
                List<CartItem> Cart = HttpContext.Current.Session["Cart"] as List<CartItem>;
                //find id in Cart
                var findCartItemCart = (from CartItem in Cart where CartItem.id == CartItemCart.id select CartItem).FirstOrDefault();
                //neu co san pham trong gio hang
                if (findCartItemCart != null)
                {
                    findCartItemCart.number += CartItemCart.number;
                    //gan nguoc lai vao HttpContext.Current.Session cart
                    HttpContext.Current.Session["Cart"] = Cart;
                }
                //neu khong co thi them san pham nay vao gio hang
                else
                {
                    //them moi CartItemCart vao gio hang
                    Cart.Add(CartItemCart);
                    //gan nguoc lai vao HttpContext.Current.Session cart
                    HttpContext.Current.Session["Cart"] = Cart;
                }
            }
        }
        //xoa san pham khoi gio hang
        public void CartDelete(int id)
        {
            List<CartItem> Cart = HttpContext.Current.Session["Cart"] as List<CartItem>;
            //find CartItem
            var CartItemCart = (from CartItem in Cart where CartItem.id == id select CartItem).FirstOrDefault();
            if(CartItemCart != null)
            {
                //xoa sp khoi Cart
                Cart.Remove(CartItemCart);
                //gan lai Cart vao session Cart
                HttpContext.Current.Session["Cart"] = Cart;
            }
        }
        //update so luong san pham
        public void CartUpdate(int id, int new_number)
        {
            List<CartItem> Cart = HttpContext.Current.Session["Cart"] as List<CartItem>;
            var CartItem = (from item in Cart where item.id == id select item).FirstOrDefault();
            if (CartItem != null)
            {
                CartItem.number = new_number;
            }
        }
        //xoa toan bo gio hang
        public void CartDestroy()
        {
            List<CartItem> objCart = new List<CartItem>();
            HttpContext.Current.Session["Cart"] = objCart;
        }
        //tong tien thanh toan
        public int CartTotal()
        {
            List<CartItem> Cart = HttpContext.Current.Session["Cart"] as List<CartItem>;
            int total = 0;
            foreach (var item in Cart)
            {
                total += item.toPrice;
            }
            return total;
        }
    }
}