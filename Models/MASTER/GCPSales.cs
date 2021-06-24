using System;
using System.Collections.Generic;
using System.Text;

namespace WinmartTool.Models.MASTER
{
    public class GCPSales
    {
        public int line_no { get; set; }
        public DateTime order_date { get; set; }
        public int order_id { get; set; }
        public int location_id { get; set; }
        public string location_name  { get; set; }
        public int order_type { get; set; }
        public string order_type_name { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public decimal unit_price { get; set; }
        public decimal qty { get; set; }
        public int vat { get; set; }
        public int payment_method_id { get; set; }
        public decimal discount { get; set; }
        public decimal amount_total { get; set; }
        public string state { get; set; }

    }
}
