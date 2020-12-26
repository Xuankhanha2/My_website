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
using TrangChuWebsite.Areas.Admin.Attr;
using System.IO;
namespace TrangChuWebsite.Areas.Admin.Controllers
{
    [CheckLogin]
    public class NewsController : Controller
    {
        // GET: Admin/News
        DataContext db = new DataContext();
        public ActionResult Index()
        {
            return RedirectToAction("Read", "News");
        }
        public ActionResult Read(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1; // neu ko co page thi mac dinh la 1;
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("select * from News order by id asc",conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            List<News> listNew = new List<News>();
            foreach(DataRow item in dt.Rows)
            {
                News a = new News();
                a.id = Convert.ToInt32(item["id"]);
                a.name = item["name"].ToString();
                a.description = item["description"].ToString();
                a.content = item["content"].ToString();
                a.hot = Convert.ToInt32(item["hot"]);
                a.photo = item["photo"].ToString();
                listNew.Add(a);
            }
            return View("Read", listNew.ToPagedList(pageNumber, pageSize));
        }

        //Update - GET
        public ActionResult Update(int id)
        {
            int _id = id;
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("select * from News where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", _id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            DataRow dr = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
               dr = dt.Rows[0];
            }
            return View("CreateUpdate", dr);
        }

        //Update - POST
        [HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult Update(int id, FormCollection fc)
        {
            int _id = id;
            string _name = fc["name"].ToString();
            string _description = fc["description"].ToString();
            string _content = fc["content"].ToString();
            int _hot = fc["hot"] != null ? 1 : 0;
            string _photo = "";
            TrangChuWebsite.Models.Database data = new TrangChuWebsite.Models.Database();
            DataTable dt = new DataTable();
            string sql = "select * from News where id = "+id;
            dt = data.GetTable(sql);
            DataRow dr = dt.NewRow();
            if(dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
                _photo = dr["photo"].ToString();
            }
            if (Request.Files["photo"].ContentLength > 0)
            {
                string pathDeleted = Path.Combine(Server.MapPath("~/Assets/Upload/News"), Path.GetFileName(dr["photo"].ToString()));
                //Xoa anh cu
                if (System.IO.File.Exists(pathDeleted))
                {
                    System.IO.File.Delete(pathDeleted);
                }
                string path = Path.Combine(Server.MapPath("~/Assets/Upload/News"), Path.GetFileName(id + "_" + Request.Files["photo"].FileName));
                Request.Files["photo"].SaveAs(path);
                _photo =id + "_" + Request.Files["photo"].FileName.ToString();
            }
            string sql_update = "update News set name = N'"+_name+"', description = N'"+_description+"', content = N'"+_content+"', hot = "+_hot+", photo = '"+_photo+"' where id = "+id;
            data.Execute(sql_update);
            return RedirectToAction("Read", "News");
        }


        //Create - GET 
        public ActionResult Create()
        {
            return View("CreateUpdate");
        }

        //Create - POST
        [HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult Create(FormCollection fc)
        {
            string _name = fc["name"].ToString();
            string _description = fc["description"].ToString();
            string _content = fc["content"].ToString();
            int _hot = fc["hot"] != null ? 1 : 0;
            string _photo = "";
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            var i = Request.Files["photo"].ContentLength;
            if (Convert.ToInt32(i) > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Assets/Upload/News"), Path.GetFileName(Request.Files["photo"].FileName));
                Request.Files["photo"].SaveAs(path);
                _photo = Request.Files["photo"].FileName;
            }
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd = new SqlCommand("insert into News(name, description, content, hot, photo)  values(@name, @description, @content, @hot, @photo)", conn);
                cmd.Parameters.AddWithValue("@name", _name);
                cmd.Parameters.AddWithValue("@description", _description);
                cmd.Parameters.AddWithValue("@content", _content);
                cmd.Parameters.AddWithValue("@hot", _hot);
                cmd.Parameters.AddWithValue("@photo", _photo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Read", "News");
        }
        public ActionResult Delete(int id)
        {
            int _id = id;
            string StrConnect = ConfigurationManager.ConnectionStrings["connection"].ToString();
            using (SqlConnection conn = new SqlConnection(StrConnect))
            {
                SqlCommand cmd1 = new SqlCommand("select * from News where id = @id", conn);
                cmd1.Parameters.AddWithValue("@id", _id);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                DataRow dr = dt.NewRow();
                string _photo = "";
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    _photo = dr["photo"].ToString();
                }
                string pathDelete = Path.Combine(Server.MapPath("~/Assets/Upload/News"), _photo);
                if (System.IO.File.Exists(pathDelete))
                    System.IO.File.Delete(pathDelete);
                SqlCommand cmd2 = new SqlCommand("delete News where id = @id", conn);
                cmd2.Parameters.AddWithValue("@id", _id);
                conn.Open();
                cmd2.ExecuteNonQuery();
            }
            return RedirectToAction("Read", "News");
        }
    }
}