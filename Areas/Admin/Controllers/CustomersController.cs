using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrangChuWebsite.Models;

using PagedList.Mvc;
using PagedList;
namespace TrangChuWebsite.Areas.Admin.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Admin/Customers
        public DataContext db = new DataContext();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            List<Customer> listCustomer = db.Customers.OrderBy(tbl => tbl.id).ToList();
            return View("Index", listCustomer.ToPagedList(pageNumber,pageSize));
        }
    }
}