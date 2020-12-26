using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Security.Cryptography;
using System.Web.Helpers;
using TrangChuWebsite.Models;
using PagedList.Mvc;
using PagedList;

namespace TrangChuWebsite.Controllers
{
    public class AccountController : Controller
    {
        DataContext db = new DataContext();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        //Login - POST
        [HttpPost,ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Login(FormCollection fc){
            string _email = fc["email"].Trim();
            string _password = fc["password"].Trim();
            _password = Crypto.SHA256(_password);
            Customer record = db.Customers.Where(tbl => tbl.email == _email && tbl.password == _password).FirstOrDefault();
            if(record == null)
            {
                return RedirectToAction("Login", "Account", new { notify = "invalid" });
            }
            else
            {
                this.Session["Customer_email"] = record.email;
                this.Session["Customer_id"] = record.id;
                this.Session["Customer_name"] = record.name;
                if(fc["remember"] != null)
                {
                    Response.Cookies["UEmail"].Value = _email;
                    Response.Cookies["UPassword"].Value = fc["password"].Trim();
                    Response.Cookies["UEmail"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["UPassword"].Expires = DateTime.Now.AddDays(30);
                }
                return RedirectToAction("Index", "Home");
            }
        }
        //Detail - Login - GET
        public ActionResult DetailLogin(int id)
        {
            return View();
        }

        // Detail - Login - POST
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult DetailLogin(int id, FormCollection fc)
        {
            int _id = id;
            string _email = fc["email"].Trim();
            string _password = fc["password"].Trim();
            _password = Crypto.SHA256(_password);
            Customer record = db.Customers.Where(tbl => tbl.email == _email && tbl.password == _password).FirstOrDefault();
            if (record == null)
            {
                return RedirectToAction("Login", "Account", new { notify = "invalid" });
            }
            else
            {
                this.Session["Customer_email"] = record.email;
                this.Session["Customer_id"] = record.id;
                this.Session["Customer_name"] = record.name;
                return RedirectToAction("ChiTietSanPham", "Product", new { id = _id});
            }
        }
        //Register - GET
        public ActionResult Register()
        {
            return View();
        }

        //Register - POST
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Register(FormCollection fc)
        {
            string _name = fc["username"].ToString();
            string _phone = fc["phone"].ToString();
            string _address = fc["address"].ToString();
            string _email = fc["email"].ToString();
            string _password = fc["password1"].ToString();
            if(_password != fc["password2"].ToString())
            {
                return RedirectToAction("Register", "Account", new { notify = "passNotSame" });
            }
            _password = Crypto.SHA256(_password);
            Customer record = db.Customers.Where(tbl => tbl.email == _email).FirstOrDefault();
            if(record != null)
            {
                return RedirectToAction("Register", "Account", new { notify = "emailExists" });
            }
            else
            {
                record = new Customer();
                record.name = _name;
                record.phone = _phone;
                record.address = _address;
                record.email = _email;
                record.password = _password;
                db.Customers.Add(record);
                db.SaveChanges();
            }
            return RedirectToAction("Login", "Account");
        }
        public ActionResult Logout()
        {
            this.Session["Customer_id"] = null;
            this.Session["Customer_email"] = null;
            this.Session["Customer_name"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccountInfor(int id)
        {
            if(this.Session["Customer_email"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Customer infor = new Customer();
            infor = db.Customers.Where(tbl => tbl.id == id).FirstOrDefault();
            return View("AccountInfor", infor);
        }

        //POST - ACCOUNT 
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AccountInfor(int id, FormCollection fc)
        {
            
            string _name = fc["customer_name"].ToString();
            string _email = fc["email"].ToString();
            string _phone = fc["phone"].ToString();
            string _address = fc["address"].ToString();
            Database data = new Database();
            string sql = "update Customers set name = N'"+_name+"', email = '"+_email+"', phone = '"+_phone+"', address = N'"+_address+"' where id = "+id+" ";
            data.Execute(sql);
            return RedirectToAction("AccountInfor", "Account", new { id = id});
        }

        //Thong tin don hang da mua
        public ActionResult MyCheckout(int id, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            int customer_id = id;
            List<Order> order = db.Orders.Where(tbl => tbl.customer_id == customer_id).ToList();
            return View("MyCheckout", order.ToPagedList(pageNumber, pageSize));
        }
    }
}
