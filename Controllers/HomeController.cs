using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//add thu vien ma hoa
using System.Security.Cryptography;
using System.Web.Helpers;

namespace TrangChuWebsite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        //Tao Control trang chi tiet san pham 

        public ActionResult Contact()
        {
            return View();
        }
    }
}