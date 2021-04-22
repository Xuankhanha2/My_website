using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrangChuWebsite.Models
{
    public class BaseClass
    {
        public string formatMoney(double money)
        {
            string newMoney = "";
            newMoney = String.Format("{0:0,0}", money);
            return newMoney;
        }
    }
}