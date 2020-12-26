using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Security.Cryptography;
using TrangChuWebsite.Models;
using PagedList;
using System.Web.Helpers;
using TrangChuWebsite.Areas.Admin.Attr;
namespace TrangChuWebsite.Areas.Admin.Controllers
{
    
    public class UsersController : Controller
    {
        DataContext db = new DataContext();
        // GET: Admin/Users
        public ActionResult Index()
        {
            return View("Read");
        }
        public ActionResult Read(int ? page)
        {
            //khai bao 1 trang
            int pageSize = 4; // ten bien tu dat ten
            // so trang
            int pageNumber = page ?? 1; // neu == null thi tra ve 1 - co the dung if
            List<User> list = (from anhxa in db.Users orderby anhxa.id ascending select anhxa).ToList();
            //List<User> list1 = db.Users.OrderByDescending(anhxa => anhxa.id).ToList();
                return View("Read", list.ToPagedList(pageNumber, pageSize));
        }

        //Create user - Get
        public ActionResult Create()
        {
            return View();
        }
        //Create - POST
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection fc)
        {
            string _hoten = fc["hoten"];
            string _email = fc["email"];
            string _password = Request.Form["password"];
            _password = Crypto.SHA256(_password); 
            User record = db.Users.Where(anhxa => anhxa.email == _email).FirstOrDefault();
            if(record == null)
            {
                record = new User();
                record.hoten = _hoten;
                record.email = _email;
                record.password = _password;
                db.Users.Add(record);
                db.SaveChanges();
            }
            return RedirectToAction("Read", "Users");
        }

        //Xoa user
        public ActionResult Delete(int id)
        {
            int _id = id;
            User record = db.Users.Where(anhxa => anhxa.id == _id).FirstOrDefault();
            if(record != null)
            {
                db.Users.Remove(record);
                db.SaveChanges();
            }
            return RedirectToAction("Read", "Users");
        }
    }
}