using System;
using System.Collections.Generic;
using System.Text;

namespace ShopInShop.Models.PLG
{
    public class UomPLG
    {
        public int id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public decimal factor { get; set; }
        public decimal rounding { get; set; }
        public bool active { get; set; }
        public string uom_type { get; set; }
        public string measure_type { get; set; }
        public int create_uid { get; set; }
        public DateTime create_date { get; set; }
        public int write_uid { get; set; }
        public DateTime write_date { get; set; }
    }
    public class ProductTaxPLG
    {
        public int prod_id { get; set; }
        public int tax_id { get; set; }
    }
}
