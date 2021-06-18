using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WinmartTool.Helpers
{
    public class SftpHelper
    {
        private readonly int SftpPort;
        private readonly string host;
        private readonly string username;
        private readonly string password;
        public SftpHelper(string _host, int _SftpPort, string _username, string _password)
        {
            SftpPort = _SftpPort;
            host = _host;
            username = _username;
            password = _password;
        }
        public void DownloadDirectory(string source, string destination)
        {
            var count = 0;
            try
            {
                PrepareDownloadFolder(destination);
                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(username);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                ConnectionInfo conInfo = new ConnectionInfo(host, SftpPort, username, keybAuth);

                using (SftpClient client = new SftpClient(conInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(source);
                    // List the files and folders of the directory
                    var files = client.ListDirectory(source);
                    files = files.OrderBy(e => e.Name);
                    // Iterate over them
                    foreach (SftpFile file in files)
                    {
                        // If is a file, download it
                        if (!file.IsDirectory && !file.IsSymbolicLink)
                        {
                            DownloadFile(client, file, destination);
                            count++;
                            file.Delete();
                        }
                    }
                    client.Disconnect();
                }
                FileHelper.WriteLogs("Download " + count.ToString() + " file");
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogs(ex.ToString());
            }
        }

        public void UploadDirectory(string source, string destination, string archive)
        {
            var count = 0;
            try
            {
                //KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(username);
                //keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                //ConnectionInfo conInfo = new ConnectionInfo(host, SftpPort, username, keybAuth);

                using (SftpClient client = new SftpClient(host, SftpPort, username, password))
                {
                    client.Connect();
                    client.ChangeDirectory(destination);

                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(source);
                    IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.txt", System.IO.SearchOption.AllDirectories);

                    foreach (System.IO.FileInfo file in fileList)
                    {
                        using (var fileStream = new FileStream(source + file.Name, FileMode.Open))
                        {
                            //client.BufferSize = 4 * 1024; // bypass Payload error large files
                            client.UploadFile(fileStream, Path.GetFileName(file.Name));
                            count++;
                        }
                        FileHelper.MoveFileToDestination(source + file.Name, archive);
                        FileHelper.WriteLogs("Uploaded file: " + file.Name);
                    }
                    client.Disconnect();
                }
                FileHelper.WriteLogs("Uploaded " + count.ToString() + " file");
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogs(ex.ToString());
            }
        }

        public void UploadDirectoryWIN(string source, string destination, string archive, string fileType)
        {
            var count = 0;
            try
            {
                //KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(username);
                //keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
                //ConnectionInfo conInfo = new ConnectionInfo(host, SftpPort, username, keybAuth);
                using (SftpClient client = new SftpClient(host, SftpPort, username, password))
                {
                    client.Connect();
                    client.ChangeDirectory(destination);

                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(source);
                    IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles(fileType, System.IO.SearchOption.AllDirectories);

                    foreach (System.IO.FileInfo file in fileList)
                    {
                        using (var fileStream = new FileStream(source + file.Name, FileMode.Open))
                        {
                            //client.BufferSize = 4 * 1024; // bypass Payload error large files
                            client.UploadFile(fileStream, Path.GetFileName(file.Name));
                            count++;
                        }
                        FileHelper.MoveFileToDestination(source + file.Name, archive);
                        FileHelper.WriteLogs("Uploaded file: " + file.Name);
                    }
                    client.Disconnect();
                }
                FileHelper.WriteLogs("Uploaded " + count.ToString() + " file");
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogs(ex.ToString());
            }
        }
        public void UploadDirectoryLinux(string source, string destination, string archive, string fileType)
        {
            var count = 0;
            try
            {
                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(username);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
                ConnectionInfo conInfo = new ConnectionInfo(host, SftpPort, username, keybAuth);
                using (SftpClient client = new SftpClient(conInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(destination);

                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(source);
                    IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles(fileType, System.IO.SearchOption.AllDirectories);

                    foreach (System.IO.FileInfo file in fileList)
                    {
                        using (var fileStream = new FileStream(source + file.Name, FileMode.Open))
                        {
                            client.UploadFile(fileStream, Path.GetFileName(file.Name));
                            count++;
                        }
                        FileHelper.MoveFileToDestination(source + file.Name, archive);
                        FileHelper.WriteLogs("Uploaded file: " + file.Name);
                    }
                    client.Disconnect();
                }
                FileHelper.WriteLogs("Uploaded " + count.ToString() + " file");
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogs(ex.ToString());
            }
        }

        private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = password;
                }
            }
        }

        public void PrepareDownloadFolder(string _folder)
        {
            try
            {
                if (!Directory.Exists(_folder))
                {
                    Directory.CreateDirectory(_folder);
                }
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogs(ex.ToString());
            }
        }

        private void DownloadFile(SftpClient client, SftpFile file, string directory)
        {
            using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
            {
                client.DownloadFile(file.FullName, fileStream);
            }
        }

    }
}
