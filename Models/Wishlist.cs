﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TrangChuWebsite.Models
{
    public class Wishlist
    {
        [Key]
        public int id { get; set; }
        public int product_id { get; set; }
        public int customer_id { get; set; }
    }
}