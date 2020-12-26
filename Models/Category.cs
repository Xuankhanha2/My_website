using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TrangChuWebsite.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        public int parent_id { get; set; }
        public string tensanpham { get; set; }
    }
}