using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace TrangChuWebsite.Models
{
    public class News
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public int hot { get; set; }
        public string photo { get; set; }
    }
}