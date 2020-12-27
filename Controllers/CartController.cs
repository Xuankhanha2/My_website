
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TrangChuWebsite.Models;
using TrangChuWebsite.Areas.Admin.Attr;

namespace TrangChuWebsite.Controllers
{
    public class CartController : Controller
    {
        public DataContext db = new DataContext();
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> Cart = new List<CartItem>();
            if (Session["Cart"] == null)
                Session["Cart"] = Cart;
            else
                Cart = Session["Cart"] as List<CartItem>;
            return View("Index", Cart);
        }
        public ActionResult Add(int id)
        {
            int _id = id;
            var record = db.Products.Where(tbl => tbl.id == _id).FirstOrDefault();
            if (record != null)
            {
                CartItem item = new CartItem();
                item.id = record.id;
                item.name = record.name;
                item.price = Convert.ToInt32(record.price);
                item.number = 1;
                item.photo = record.photo;
                ShoppingCart objCart = new ShoppingCart();
                objCart.CartAdd(item);
            }
            return RedirectToAction("Index");
        }
        //Xoa mat hang traong gio hang
        public ActionResult Remove(int id)
        {
            int _id = id;
            ShoppingCart objCart = new ShoppingCart();
            objCart.CartDelete(id);
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            List<CartItem> Cart = new List<CartItem>();
            int number = Convert.ToInt32(Request.QueryString["number"]);
            //Khai bao doi tuong shopping de lay CartItem
            ShoppingCart objCart = new ShoppingCart();
            objCart.CartUpdate(id, number);
            Cart = Session["Cart"] as List<CartItem>;
            return View("Cart_update", Cart);
        }

        //Xoa toan bo gio hang
        public ActionResult Clear()
        {
            ShoppingCart objCart = new ShoppingCart();
            objCart.CartDestroy();
            return RedirectToAction("Index");
        }
        public ActionResult Checkout()
        {
            //kiem tra xem user da dang nhap chua, neu chua dang nhap thi di chuyen trang trang login
            if (this.Session["Customer_email"] == null)
                return RedirectToAction("Login", "Account");
            else
            {
                //---
                List<CartItem> Cart = new List<CartItem>();
                Cart = Session["Cart"] as List<CartItem>;
                //---
                //tong gia
                int _price = 0;
                foreach (var item in Cart)
                    _price += item.price;
                //---
                //thay di cua customer
                int customer_id = Convert.ToInt32(this.Session["customer_id"].ToString());
                //tao ban ghi de insert du lieu vao bang Orders  
                Order recordOrder = new Order();
                recordOrder.customer_id = customer_id;
                recordOrder.price = Convert.ToInt32(_price);
                recordOrder.status = 0;
                recordOrder.date = DateTime.Now.ToString();
                //---
                db.Orders.Add(recordOrder);
                db.SaveChanges();
                //---
                int _order_id = recordOrder.id;
                //---
                foreach (var item in Cart)
                {
                    OrdersDetail recordOrderDetail = new OrdersDetail();
                    recordOrderDetail.order_id = _order_id;
                    recordOrderDetail.product_id = item.id;
                    recordOrderDetail.quantity = item.number;
                    recordOrderDetail.price = Convert.ToDouble(item.price);
                    //---
                    db.OrdersDetails.Add(recordOrderDetail);
                    db.SaveChanges();
                    //---
                }
                //---
                //xoa toan bo gio hang
                ShoppingCart objCart = new ShoppingCart();
                objCart.CartDestroy();
                //---
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}