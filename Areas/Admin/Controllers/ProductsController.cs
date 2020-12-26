using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using TrangChuWebsite.Models;
using System.Security.Cryptography;
using System.Configuration;
using PagedList;
using System.IO;
using System.Net;
using TrangChuWebsite.Areas.Admin.Attr;
namespace TrangChuWebsite.Areas.Admin.Controllers
{
    [CheckLogin]
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        TrangChuWebsite.Models.Database data = new TrangChuWebsite.Models.Database();
        public ActionResult Index()
        {
            return View("Read");
        }

        //
        public ActionResult Read(int? page)
        {
            int pageSize = 12;
            int pageNumber = page ?? 1; // neu ko co page thi mac dinh la 1;
            DataTable dt = new DataTable();
            string sql = "select * from Products order by id asc";
            dt = data.GetTable(sql);
            List<Product> listNew = new List<Product>();
            foreach (DataRow item in dt.Rows)
            {
                Product a = new Product();
                a.id = Convert.ToInt32(item["id"]);
                a.name = item["name"].ToString();
                a.description = item["description"].ToString();
                a.content = item["content"].ToString();
                a.hot = Convert.ToInt32(item["hot"]);
                a.price = Convert.ToInt32(item["price"]);
                a.discount = Convert.ToInt32(item["discount"]);
                a.photo = item["photo"].ToString();
                listNew.Add(a);
            }
            return View("Read", listNew.ToPagedList(pageNumber, pageSize));
        }

        //Upadte - GET
        public ActionResult Update(int id)
        {
            int _id = id;
            DataTable dt = new DataTable();
            string sql = "select * from Products where id = "+id;
            dt = data.GetTable(sql);
            DataRow dr = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            return View("CreateUpdate", dr);
        }


        //Update - POST
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Update(FormCollection fc, int id)
        {
            
            int _id = id;
            int _category_id = Convert.ToInt32(fc["category_id"]);
            string _name = fc["name"].ToString();
            string _description = fc["description"].ToString();
            string _content = fc["content"] != null ? "1" : "0";
            int _hot = fc["hot"] != null ? 1 : 0;
            double _price = 0;
            int _discount = 0;
            try
            {
                _price = Convert.ToInt32(fc["price"]);
                _discount = Convert.ToInt32(fc["discount"]);
            }
            catch
            {

            }
            string _photo = "";
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("select * from Products where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            DataRow dr = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            var i = Request.Files["photo"].ContentLength;
            if (Convert.ToInt32(i) > 0)
            {
                string pathDeleted = Path.Combine(Server.MapPath("~/Assets/Upload/Products"), dr["photo"].ToString());
                //Xoa anh cu
                if (System.IO.File.Exists(pathDeleted))
                {
                    System.IO.File.Delete(pathDeleted);
                }
                string path = Path.Combine(Server.MapPath("~/Assets/Upload/Products"), Path.GetFileName(id + "_" + Request.Files["photo"].FileName));
                Request.Files["photo"].SaveAs(path);
                _photo = id + "_" + Request.Files["photo"].FileName;
            }
            else
            {
                _photo = dr["photo"].ToString();
            }
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("update Products set category_id = @category_id, name = @name, description = @description, content = @content, hot = @hot, price = @price, discount = @discount, photo = @photo where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                cmd.Parameters.AddWithValue("@category_id", _category_id);
                cmd.Parameters.AddWithValue("@name", _name);
                cmd.Parameters.AddWithValue("@description", _description);
                cmd.Parameters.AddWithValue("@content", _content);
                cmd.Parameters.AddWithValue("@hot", _hot);
                cmd.Parameters.AddWithValue("@price", _price);
                cmd.Parameters.AddWithValue("@discount", _discount);
                cmd.Parameters.AddWithValue("@photo", _photo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Read", "Products");
        }

        //Create - GET
        public ActionResult Create()
        {
            return View("CreateUpdate");
        }

        //Create - POST
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create(FormCollection fc)
        {
            int _category_id = Convert.ToInt32(fc["category_id"]);
            string _name = fc["name"].ToString();
            string _description = fc["description"].ToString();
            string _content = fc["content"] != null ? "1" : "0";
            int _hot = fc["hot"] != null ? 1 : 0;
            double _price = 0;
            int _discount = 0;
            try
            {
                _price = Convert.ToInt32(fc["price"]);
                _discount = Convert.ToInt32(fc["discount"]);
            }
            catch
            {

            }
            string _photo = "";
            var i = Request.Files["photo"].ContentLength;
            if (Convert.ToInt32(i) > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Assets/Upload/Products"), Path.GetFileName(Request.Files["photo"].FileName));
                Request.Files["photo"].SaveAs(path);
                _photo = Request.Files["photo"].FileName;
            }
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            string sql = "insert into Products(category_id, name, description, content, hot, price, discount, photo) values("+_category_id+", N'"+_name+"', N'"+_description+"', N'"+_content+"', "+_hot+", "+_price+", "+_discount+", '"+_photo+"' )";
            data.Execute(sql);
            return RedirectToAction("Read", "Products");
        }
        public ActionResult Delete(int id)
        {
            int _id = id;
            TrangChuWebsite.Models.Database data = new Models.Database();
            DataTable dt = new DataTable();
            string sql = "select * from Products where id = " + id;
            dt = data.GetTable(sql);
            //-----------Lay ra 1 row de lay ten anh ----------
            DataRow dr = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            string _photo = dr["photo"].ToString();
            //--------------------------------------------------
            string pathDeleted = Path.Combine(Server.MapPath("~/Assets/Upload/Products"), _photo);
            //Xoa anh cu
            if (System.IO.File.Exists(pathDeleted))
            {
                System.IO.File.Delete(pathDeleted);
            }
            string sql_delete = "delete Products where id = " + id;
            data.Execute(sql_delete);
            return RedirectToAction("Read", "Products");
        }

        public ActionResult Images(int id)
        {
            int _id = id;
            ViewBag.product_id = _id;
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("select * from Images where product_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            
            List<Image> list = new List<Image>();
            foreach(DataRow item in dt.Rows)
            {
                Image img = new Image();
                img.id = Convert.ToInt32(item["id"]);
                img.product_id = Convert.ToInt32(item["product_id"]);
                img.photo = item["photo"].ToString();
                list.Add(img);


            }
            return View("Images", list);
        }

        //get - Images
        public ActionResult CreateImages()
        {
            return View();
        }
        //POST - Images
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateImages(FormCollection fc, int product_id)
        {
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            
            string _photo = "";
            if (Request.Files["photo"].ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Assets/Upload/Products/Images"), Path.GetFileName(product_id.ToString() + Request.Files["photo"].FileName));
                Request.Files["photo"].SaveAs(path);
                _photo =product_id.ToString() + Request.Files["photo"].FileName.ToString();
            }
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("insert into Images(product_id, photo) values(@product_id, @photo)", conn);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@photo", _photo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Images", "Products", new { id = product_id });
            //new { id = _id } -> gắn lại id cho ~/Images/id
        }

        //Thay anh moi
        public ActionResult ChangeImages(int id, int product_id)
        {
            return View();
        }
        //POST Change-images
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult ChangeImages(int id, int product_id, FormCollection fc)
        {
            Models.Database data = new Models.Database();
            string get_image = "select * from Images where product_id = " + product_id + " and id = "+ id;
            DataTable dt = new DataTable();
            dt = data.GetTable(get_image);
            string _photo = "";
            //Lay ra 1 dong 
            DataRow dr = dt.NewRow();
            if(dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
                _photo = dr["photo"].ToString();
            }
            if (Request.Files["photo"].ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Assets/Upload/Products/Images"),Path.GetFileName(_photo));
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                string image = Path.Combine(Server.MapPath("~/Assets/Upload/Products/Images"), Path.GetFileName(id + Request.Files["photo"].FileName.ToString()));
                Request.Files["photo"].SaveAs(image);
                _photo = id + Request.Files["photo"].FileName;
            }
            string change = "update Images set photo = '" + _photo + "' where product_id = " + product_id + " and id = "+id;
            data.Execute(change);
            return RedirectToAction("Images", "Products", new { id = product_id});
        }

        //Delete Image
        public ActionResult DeleteIamge(int id, int product_id )
        {
            int _id = id;
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("select * from Images where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            DataRow dr = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            string _photo = dr["photo"].ToString();
            //--------------------------------------------------
            string pathDeleted = Path.Combine(Server.MapPath("~/Assets/Upload/Products/Images"), _photo);
            //Xoa anh cu
            if (System.IO.File.Exists(pathDeleted))
            {
                System.IO.File.Delete(pathDeleted);
            }
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("Delete Images where id  = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Images", "Products", new { id = product_id});
        }
    }
}