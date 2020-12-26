using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TrangChuWebsite.Models;
using PagedList.Mvc;
using PagedList;

namespace TrangChuWebsite.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public DataContext db = new DataContext();
        public ActionResult ListNews(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;
            List<News> listNews = db.News.OrderBy(tbl => tbl.id).ToList();
            return View("ListNews", listNews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Detail(int id)
        {
            int _id = id;
            News tin = db.News.Where(tbl => tbl.id == _id).FirstOrDefault();
            return View("Detail", tin);
        }
    }
}