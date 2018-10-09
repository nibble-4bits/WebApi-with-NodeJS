using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi_with_NodeJS.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
        public List<SaleDetail> SaleDetail { get; set; }
    }
}