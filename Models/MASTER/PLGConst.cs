using System;
using System.Collections.Generic;
using System.Text;

namespace ShopInShop.Const
{
    public static class PLGConst
    {
        public static Dictionary<int, string> MappingTenderTypePLG()
        {
            Dictionary<int, string> openWith = new Dictionary<int, string>
            {
                { 1, "PTCS" },
                { 2, "ZCRE" },
                { 3, "ZIVO" },
                { 4, "ZNAP" },
                { 5, "ZNAP" },
                { 6, "ZNAP" },
                { 7, "ZNAP" },
                { 8, "ZNAP" },
                { 9, "ZNAP" },
                { 10, "ZNAP" },
                { 11, "ZNAP" },
                { 12, "ZNAP" },
                { 13, "ZNAP" },
                { 14, "ZCRE" },
                { 15, "ZCRE" },
                { 16, "ZCRE" },
                { 17, "PTCS" },
                { 18, "PTCS" },
                { 19, "CONO" },
                { 20, "PTCS" },
                { 21, "ZNAP" },
                { 22, "ZNAP" },
                { 23, "ZNAP" },
                { 24, "ZFPC" },
                { 25, "PTCS" },
                { 26, "ZNAP" }
            };
            return openWith;
        }
        public static Dictionary<int, string> SalesTypePLG()
        {
            Dictionary<int, string> openWith = new Dictionary<int, string>
            {
                { 1, "Tại chỗ" },
                { 2, "Gojeck" },
                { 3, "NowFood" },
                { 4, "GrabFood" },
                { 5, "BEAMIN" },
                { 6, "Website" },
                { 7, "Giao hàng" },
                { 8, "Kiosk" }
            };
            return openWith;
        }
        public static string GetMasterDataType(int id)
        {
            Dictionary<int, string> openWith = new Dictionary<int, string>();
            openWith.Add(1, "product_product");
            openWith.Add(2, "payment_method");
            openWith.Add(3, "stock_location");
            openWith.Add(4, "sales_promo");
            openWith.Add(5, "sales_combo");
            openWith.Add(6, "uom_uom");
            openWith.Add(7, "product_taxes_rel");
            return openWith[id];
        }
        public static Dictionary<int, decimal> MappingTax()
        {
            Dictionary<int, decimal> openWith = new Dictionary<int, decimal>
            {
                { 1, 0 },
                { 2, 0 },
                { 3, 5 },
                { 4, 10 },
                { 5, 5 },
                { 9, 0 }
            };
            return openWith;
        }
        public static Dictionary<string, int> LocationTempl()
        {
            Dictionary<string, int> openWith = new Dictionary<string, int>
            {
                { "pos_session", 2100000001 },
                
            };
            return openWith;
        }
    }
}
