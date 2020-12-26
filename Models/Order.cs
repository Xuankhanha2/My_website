using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TrangChuWebsite.Models
{   public class Order
    {
        [Key]
        public int id { get; set; }
        public int customer_id { get; set; }
        public string date { get; set; }
        public int price { get; set; }
        public int status { get; set; }
    }
}