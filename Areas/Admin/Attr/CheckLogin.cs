using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace TrangChuWebsite.Areas.Admin.Attr
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {          
            if (HttpContext.Current.Session["email"] == null)
            {
                HttpContext.Current.Response.Redirect("/Admin/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}