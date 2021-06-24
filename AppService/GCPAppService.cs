using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopInShop.Const;
using ShopInShop.Models.PLG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinmartTool.Helpers;
using WinmartTool.Models.MASTER;
using WinmartTool.Models.PLG;

namespace WinmartTool.AppService
{
    public class GCPAppService
    {
        private readonly IConfiguration _configuration;
        public GCPAppService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void CreateSalesGCP(string pathLocal)
        {
            var masterPath = _configuration["AppConfig:MasterData"];
            var archive = _configuration["AppConfig:dataArchived"];
            var _lstProduct = JsonConvert.DeserializeObject<List<ProductPLG>>(FileHelper.GetMasterDataPLG(masterPath, PLGConst.GetMasterDataType(1).ToString()));
            var _lstTenderType = JsonConvert.DeserializeObject<List<TenderTypePLG>>(FileHelper.GetMasterDataPLG(masterPath, PLGConst.GetMasterDataType(2).ToString()));
            var _lstStockLocation = JsonConvert.DeserializeObject<List<StockLocationPLG>>(FileHelper.GetMasterDataPLG(masterPath, PLGConst.GetMasterDataType(3).ToString()));
            var _lstUom = JsonConvert.DeserializeObject<List<UomPLG>>(FileHelper.GetMasterDataPLG(masterPath, PLGConst.GetMasterDataType(6).ToString()));
            var _lstProductTax = JsonConvert.DeserializeObject<List<ProductTaxPLG>>(FileHelper.GetMasterDataPLG(masterPath, PLGConst.GetMasterDataType(7).ToString()));
            
            var lstFile = FileHelper.GetFileFromDir(pathLocal, "*.txt");

            if (lstFile.Count > 0)
            {
                int i = 0;
                List<GCPSales> gcpsaleData = new List<GCPSales>();
                foreach (var file in lstFile)
                {
                    if (file.ToString().Substring(0, 3) == "PLG") 
                    {
                        var dataSales = JsonConvert.DeserializeObject<TransactionPLG>(System.IO.File.ReadAllText(pathLocal + file));
                        if (dataSales != null)
                        {
                            foreach (var item in dataSales.TransLine)
                            {
                                if (item.price_subtotal > 0)
                                {
                                    var location = _lstStockLocation.Where(x => x.id == dataSales.TransHeader.location_id).FirstOrDefault();
                                    string locationName = ( location!= null) ? location.name : "";

                                    var product = _lstProduct.Where(x => x.id == item.product_id).FirstOrDefault();
                                    string productName = (product != null) ? product.display_name : "";

                                    var tax = _lstProductTax.Where(x => x.prod_id == item.product_id).FirstOrDefault();
                                    int taxGCP = (tax != null) ? Convert.ToInt16(PLGConst.MappingTax()[tax.tax_id]): 0;

                                    int paymentId = dataSales.TransPaymentEntry.FirstOrDefault().payment_method_id;
                                    gcpsaleData.Add(new GCPSales
                                    {
                                        line_no = item.id,
                                        order_date = item.date_order,
                                        order_id = item.order_id,
                                        location_id = dataSales.TransHeader.location_id,
                                        location_name = locationName,
                                        order_type = dataSales.TransHeader.sale_type_id,
                                        order_type_name = PLGConst.SalesTypePLG()[dataSales.TransHeader.sale_type_id],
                                        product_id = item.product_id,
                                        product_name = productName,
                                        unit_price = item.price_unit,
                                        qty = Math.Round(item.qty),
                                        vat = taxGCP,
                                        payment_method_id = paymentId,
                                        discount = Math.Round(dataSales.TransHeader.discount_amount),
                                        amount_total = Math.Round(dataSales.TransHeader.amount_total),
                                        state = dataSales.TransHeader.state,
                                    });
                                }
                            }
                        }
                    }
                    FileHelper.MoveFileToDestination(pathLocal + file, archive);
                    i++;
                    if (i == 1000)
                    {
                        break;
                    }
                }
                if(gcpsaleData.Count > 0)
                {
                    FileHelper.CreateFileMaster("", "GCP", _configuration["SftpConfig:path_local_process"], JsonConvert.SerializeObject(gcpsaleData));
                }

            }
        }
    }
}
