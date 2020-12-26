using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using TrangChuWebsite.Areas.Admin.Attr;
using TrangChuWebsite.Models;
using System.Security.Cryptography;
using System.Web.Helpers;


namespace TrangChuWebsite.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [CheckLogin]
        public ActionResult Index()
        {
            return View();
        }
        

    }
}