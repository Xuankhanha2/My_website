using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TrangChuWebsite.Models
{
    public class OrdersDetail
    {
        [Key]
        public int id { get; set; }
        public int product_id { get; set; }
        public int order_id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }
}