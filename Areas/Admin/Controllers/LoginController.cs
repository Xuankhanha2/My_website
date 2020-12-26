using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using TrangChuWebsite.Models;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace TrangChuWebsite.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        DataContext db = new DataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection fc)
        {
            string _email = Request.Form["email"].Trim();
            string _password = fc["password"].Trim();
            _password = Crypto.SHA256(_password);
            User record = (from anhxa in db.Users where anhxa.email == _email select anhxa).FirstOrDefault();
            if (record != null && _password == record.password)
            {
                this.Session["email"] = _email;
                this.Session["Username"] = record.hoten;
                if(Request.Form["remember"] != null)
                {
                    Response.Cookies["AdminUsername"].Value = _email;
                    Response.Cookies["AdminPassword"].Value = fc["password"].Trim();
                    Response.Cookies["AdminUsername"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["AdminPassword"].Expires = DateTime.Now.AddDays(30);
                }
                //else{
                //    Response.Cookies["AdminUsername"].Value = null;
                //    Response.Cookies["AdminPassword"].Value = null;
                //}
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        public ActionResult Logout()
        {
            this.Session["email"] = null;
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Pass()
        {
            string pass = Crypto.SHA256("khanh").ToString();
            return Content(pass);
        }
    }
}