using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WinmartTool.Helpers;
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
            var lstFile = FileHelper.GetFileFromDir(pathLocal, "*.txt");
            if(lstFile.Count > 0)
            {
                foreach(var file in lstFile)
                {
                    var dataSales = JsonConvert.DeserializeObject<TransactionPLG>(System.IO.File.ReadAllText(pathLocal + file));
                    FileHelper.WriteLogs(JsonConvert.SerializeObject(dataSales));
                }
            }
        }
    }
}
