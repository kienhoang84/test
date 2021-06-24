using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using WinmartTool.AppService;
using WinmartTool.Helpers;

namespace WinmartTool
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IConfiguration _configuration = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .Build();
                string localPath = _configuration["AppConfig:localPath"].ToString();
                string _source = _configuration["SftpConfig:path_local_process"].ToString();
                string _archive = _configuration["SftpConfig:path_local_archive"].ToString();
                string _destination = _configuration["SftpConfig:path_sftp_process"].ToString();
                string task = _configuration["AppConfig:Task"].ToString().ToUpper();
                string _host = _configuration["SftpConfig:host"].ToString().ToUpper();
                int _port = Convert.ToInt32(_configuration["SftpConfig:port"].ToString());
                string _username = _configuration["SftpConfig:username"].ToString();
                string _password = _configuration["SftpConfig:password"].ToString();

                switch (task)
                {
                    case "GCP-PUSH-SALE":
                        GCPAppService gCPAppService = new GCPAppService(_configuration);
                        gCPAppService.CreateSalesGCP(localPath);
                        SftpHelper sftpHelper = new SftpHelper(_host,_port,_username,_password);
                        sftpHelper.UploadDirectory(_source,_destination,_archive);
                        Console.WriteLine("End process.");

                        if(DateTime.Now.ToString("HH") == "13")
                        {
                            FileHelper.WriteLogs("delete: " + _archive);
                            FileHelper.DeleteFileHistory(_archive, 1000);
                            Thread.Sleep(200);
                            FileHelper.WriteLogs("delete: " + _configuration["AppConfig:dataArchived"].ToString());
                            FileHelper.DeleteFileHistory(_configuration["AppConfig:dataArchived"], 50000);
                        }

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
