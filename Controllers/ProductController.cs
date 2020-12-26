using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using TrangChuWebsite.Models;
using PagedList.Mvc;
using PagedList;

namespace TrangChuWebsite.Controllers
{
    public class ProductController : Controller
    {
        public DataContext db = new DataContext();
        Models.Database data = new Models.Database();
        // GET: Product
        public ActionResult Index()
        {
            List<Category> listCategories = new List<Category>();
            listCategories = db.Categories.ToList();
            return View("Index", listCategories);
        }
        //trang phan loai san pham
        public ActionResult PhanLoaiSanPham(int id, int? page)
        {
            int _id = id;
            int pageNumber = page ?? 1;
            int pageSize = 16;
            ViewBag.id = _id;
            string _order = Request.QueryString["order"] != null ? Request.QueryString["order"].ToString() : "";
            List<Product> listProduct = db.Products.Where(anhxa => anhxa.category_id == _id).OrderByDescending(anhxa => anhxa.id).ToList();
            switch (_order)
            {
                case "priceDesc":
                    listProduct = db.Products.Where(anhxa => anhxa.category_id == _id).OrderByDescending(anhxa => anhxa.price).ToList();
                    break;
                case "priceAsc":
                    listProduct = db.Products.Where(anhxa => anhxa.category_id == _id).OrderBy(anhxa => anhxa.price).ToList();
                    break;
                case "nameDesc":
                    listProduct = db.Products.Where(anhxa => anhxa.category_id == _id).OrderByDescending(anhxa => anhxa.name).ToList();
                    break;
                case "nameAsc":
                    listProduct = db.Products.Where(anhxa => anhxa.category_id == _id).OrderBy(anhxa => anhxa.name).ToList();
                    break;
            }
            return View("PhanLoaiSanPham", listProduct.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTietSanPham(int id)
        {
            ViewBag.product_id = id;
            Product SP = db.Products.Where(anhxa => anhxa.id == id).FirstOrDefault();
            return View("ChiTietSanPham", SP);
        }

        //Danh gia san pham 
        //POST
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Rating(int id, FormCollection fc)
        {
            int _id = id;
            if (this.Session["Customer_email"] == null)
            {
                return RedirectToAction("DetailLogin", "Account", new { id = _id });
            }
            else
            {
                Rating record = new Rating();
                record.product_id = id;
                record.customer_id = Convert.ToInt32(this.Session["Customer_id"]);
                record.star = Convert.ToInt32(fc["star"]);
                record.content = Request.Form["content"].ToString();
                db.Ratings.Add(record);
                db.SaveChanges();
                return RedirectToAction("ChiTietSanPham", "Product", new { id = _id });
            }

        }

        //Loc theo gia
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult FilterPrice1(int id, int? page, FormCollection fc)
        {
            ViewBag.id = id;
            int pageNumber = page ?? 1; // Neu null thi cha ve 1
            int pageSize = 20;
            List<Product> listFilter = new List<Product>();
            string strConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            int _from;
            int _to;
            try
            {
                _from = Convert.ToInt32(Request.Form["from"]);
                _to = Convert.ToInt32(Request.Form["to"]);
            }
            catch
            {
                _from = 0;
                _to = 0;
            }
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(strConnect))
            {
                SqlCommand cmd = new SqlCommand("select * from Products where price between @from and @to and category_id = @id", conn);
                cmd.Parameters.AddWithValue("@from", _from);
                cmd.Parameters.AddWithValue("@to", _to);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            foreach (DataRow item in dt.Rows)
            {
                listFilter.Add(new Product
                {
                    id = Convert.ToInt32(item["id"]),
                    category_id = Convert.ToInt32(item["category_id"]),
                    description = item["description"].ToString(),
                    content = item["content"].ToString(),
                    discount = Convert.ToInt32(item["discount"]),
                    name = item["name"].ToString(),
                    hot = Convert.ToInt32(item["hot"]),
                    photo = item["photo"].ToString(),
                    price = Convert.ToInt32(item["price"])
                });
            }
            if (listFilter.Count == 0)
            {
                ViewBag.result = "noResult";
            }
            return View("FilterPrice", listFilter.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult FilterPrice(int id, int? page)
        {
            ViewBag.id = id;
            int pageNumber = page ?? 1; // Neu null thi cha ve 1
            int pageSize = 20;
            List<Product> listFilter = new List<Product>();
            string strConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            int _from;
            int _to;
            try
            {
                _from = Convert.ToInt32(Request.QueryString["from"]);
                _to = Convert.ToInt32(Request.QueryString["to"]);
            }
            catch
            {
                _from = 0;
                _to = 0;
            }
            DataTable dt = new DataTable();
            string sql = "select * from Products where price between " + _from + " and " + _to + " and category_id = " + id + " ";
            dt = data.GetTable(sql);
            foreach (DataRow item in dt.Rows)
            {
                listFilter.Add(new Product
                {
                    id = Convert.ToInt32(item["id"]),
                    category_id = Convert.ToInt32(item["category_id"]),
                    description = item["description"].ToString(),
                    content = item["content"].ToString(),
                    discount = Convert.ToInt32(item["discount"]),
                    name = item["name"].ToString(),
                    hot = Convert.ToInt32(item["hot"]),
                    photo = item["photo"].ToString(),
                    price = Convert.ToInt32(item["price"])
                });
            }
            if (listFilter.Count <= 0)
            {
                ViewBag.result = "noResult";
            }
            return View("FilterPrice", listFilter.ToPagedList(pageNumber, pageSize));

        }
        //end Loc theo gia

        //Normal - Search
        public ActionResult Search(int? page)
        {
            int pageNumber = page ?? 1; // Neu null thi cha ve 1
            int pageSize = 20;
            //Lay ra chuoi tim kiem
            string _key = Request.QueryString["key"] != null ? Request.QueryString["key"] : "";

            string strConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(strConnect))
            {
                SqlCommand cmd = new SqlCommand("select * from Products where name like N'%" + _key + "%'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            List<Product> list = new List<Product>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new Product
                {
                    id = Convert.ToInt32(item["id"]),
                    category_id = Convert.ToInt32(item["category_id"]),
                    description = item["description"].ToString(),
                    content = item["content"].ToString(),
                    discount = Convert.ToInt32(item["discount"]),
                    name = item["name"].ToString(),
                    hot = Convert.ToInt32(item["hot"]),
                    photo = item["photo"].ToString(),
                    price = Convert.ToInt32(item["price"])
                });
            }
            return View("Search", list.ToPagedList(pageNumber, pageSize));
        }

        //Ajax search
        public ActionResult AJaxSearch()
        {
            string _key = Request.QueryString["key"] != null ? Request.QueryString["key"] : "";
            List<Product> _list = db.Products.Where(tbl => tbl.name.Contains(_key)).ToList();
            return View("AJaxSearch", _list);
        }


        //Danh sach san pham ua thic
        public ActionResult Wishlist(int id, int? page)
        {
            int pageSize = 8;
            int pageNumber = page ?? 1;
            int _customer_id = id;
            List<Wishlist> listWishlist = db.Wishlists.Where(tbl => tbl.customer_id == _customer_id).ToList();
            List<Product> listProduct = new List<Product>();
            foreach (Wishlist item in listWishlist)
            {
                Product record = db.Products.Where(tbl => tbl.id == item.product_id).FirstOrDefault();
                listProduct.Add(record);
            }
            return View("Wishlist", listProduct.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddWishlist(int id, int customer_id)
        {
            if (this.Session["Customer_email"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Wishlist wl = new Wishlist();
                wl.product_id = id;
                wl.customer_id = customer_id;
                int kt = 0;
                List<Wishlist> list = db.Wishlists.Where(tbl => tbl.customer_id == customer_id).ToList();
                foreach (Wishlist item in list)
                {
                    if (item.product_id == id)
                        kt = 1;
                }
                if (kt == 0)
                {
                    db.Wishlists.Add(wl);
                    db.SaveChanges();
                }
                return RedirectToAction("Wishlist", "Product", new { id = customer_id });
            }
        }

        //Xoa 1 san pham ua thich
        public ActionResult DeleteWishlist(int id, int customer_id)
        {
            Wishlist wl = db.Wishlists.Where(tbl => tbl.customer_id == customer_id && tbl.product_id == id).FirstOrDefault();
            db.Wishlists.Remove(wl);
            db.SaveChanges();
            return RedirectToAction("Wishlist", "Product", new { id = customer_id });
        }
    }
}