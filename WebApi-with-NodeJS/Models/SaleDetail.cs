using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi_with_NodeJS.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
    }
}