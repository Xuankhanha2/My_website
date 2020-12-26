using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using TrangChuWebsite.Models;
using PagedList;
using TrangChuWebsite.Areas.Admin.Attr;

namespace TrangChuWebsite.Areas.Admin.Controllers
{
    [CheckLogin]
    public class CategoryController : Controller
    {
        DataContext db = new DataContext();
        // GET: Admin/Category
        public ActionResult Index()
        {
            return RedirectToAction("/Admin/Category/Read");
        }
        // GET: Read
        public ActionResult Read(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;
            string StringConn = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                SqlCommand cmd = new SqlCommand("select * from Categories where parent_id = 0 order by id asc", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            //ra khoi using dt da co day du du lieu cua table Categories
            //--------------------------------------------------------------------------------
            List<Category> listRecord = new List<Category>();
            foreach(DataRow item in dt.Rows)
            {
                Category a = new Category();
                a.id = Convert.ToInt32(item["id"]);
                a.parent_id = Convert.ToInt32(item["parent_id"]);
                a.tensanpham = item["tensanpham"].ToString();
                listRecord.Add(a);
            }
            return View("Read", listRecord.ToPagedList(pageNumber, pageSize));
        }

        //sua ban ghi
        //Edit - GET
        public ActionResult Edit(int id)
        {
            int _id = id;
            string StringConn = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                SqlCommand cmd = new SqlCommand("select * from Categories where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            DataRow dr = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
                //Lay hang dau tien trong nhieu hang
                dr = dt.Rows[0];
            }
            return View("Edit", dr);
        }

        //Edit - POST
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection fc, int id)
        {
            int _id = id;
            string _tensanpham = Request.Form["tensanpham"];
            int _parent_id = Convert.ToInt32(Request.Form["parent_id"]);
            string StringConn = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable da = new DataTable();
            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                SqlCommand cmd = new SqlCommand("update Categories set parent_id = @parent_id, tensanpham = @tensanpham where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                cmd.Parameters.AddWithValue("@parent_id", _parent_id);
                cmd.Parameters.AddWithValue("@tensanpham", _tensanpham);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Read", "Category");
        }
        //Create _ GET
        public ActionResult Create()
        {
            return View();
        }
        //Create _ POST
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection fc)
        {
            string _tensanpham = Request.Form["tensanpham"];
            int _parent_id = Convert.ToInt32(Request.Form["parent_id"]);
            string StringConn = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable da = new DataTable();
            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                SqlCommand cmd = new SqlCommand("insert into Categories(parent_id, tensanpham) values(@parent_id, @tensanpham)", conn);
                cmd.Parameters.AddWithValue("@parent_id", _parent_id);
                cmd.Parameters.AddWithValue("@tensanpham", _tensanpham);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Read", "Category");
        }
        //Delete _ 
        public ActionResult Delete(int id)
        {
            int _id = id;
            string StringConn = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StringConn))
            {
                SqlCommand cmd1 = new SqlCommand("select * from Categories where parent_id = @id",conn);
                cmd1.Parameters.AddWithValue("@id", _id);
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                conn.Open();
                if (dt.Rows.Count > 0)
                {
                    SqlCommand cmd2 = new SqlCommand("delete Categories where parent_id = @id",conn);
                    cmd2.Parameters.AddWithValue("@id", _id);
                    cmd2.ExecuteNonQuery();
                }
                SqlCommand cmd = new SqlCommand("delete Categories where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Read", "Category");
        }
    }
}