using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrangChuWebsite.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string hoten { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}