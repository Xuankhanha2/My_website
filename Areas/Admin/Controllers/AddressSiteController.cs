using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using TrangChuWebsite.Models;
namespace TrangChuWebsite.Areas.Admin.Controllers
{
    public class AddressSiteController : Controller
    {
        // GET: Admin/Address
        public ActionResult Index()
        {
            return View();
        }
        TrangChuWebsite.Models.Database data = new TrangChuWebsite.Models.Database();
        public ActionResult Read()
        {
            List<Address> List_address = new List<Address>();
            string sql = "select * from Addresses";
            DataTable dt = data.GetTable(sql);
            foreach (DataRow item in dt.Rows)
            {
                List_address.Add(new Address { 
                    id = Convert.ToInt32(item["id"]),
                    TenDiaChi = item["TenDiaChi"].ToString(),
                    Map = item["Map"].ToString()
                });
            }
            return View("Read", List_address);
        }
    }
}