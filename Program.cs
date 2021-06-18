using Microsoft.Extensions.Configuration;
using System;
using WinmartTool.AppService;
using WinmartTool.Helpers;

namespace WinmartTool
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .Build();
            string task = _configuration["AppConfig:Task"].ToString().ToUpper();
            string localPath = _configuration["SftpConfig:path_local_process"].ToString().ToUpper();
            try
            {
                switch (task)
                {
                    case "GCP-PUSH-SALE":
                        GCPAppService gCPAppService = new GCPAppService(_configuration);

                        gCPAppService.CreateSalesGCP(localPath);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogs("Main Exception: " + ex.Message.ToString());
            }
           
        }
    }
}
