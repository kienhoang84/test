using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WinmartTool.Helpers
{
    public static class FileHelper
    {
        public static bool CreateFileMaster(string _fileType, string file_name, string _pathSaveFile, string _str)
        {
            CreateFolder(_pathSaveFile);
            var _filename = _pathSaveFile + file_name + "_" + _fileType + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmssfff") + ".txt";
            try
            {
                if (_str != null)
                {
                    File.Create(_filename).Dispose();
                    using (TextWriter tw = new StreamWriter(_filename))
                    {
                        tw.WriteLine(_str);
                        tw.Close();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string ReadFileTxt(string fileText)
        {
            string line;
            string result = "";
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileText);
                while ((line = file.ReadLine()) != null)
                {
                    result = line;
                }
                file.Close();
            }
            catch (IOException ex)
            {
                WriteLogs("IOException ReadFileTxt: " + ex.Message.ToString());
            }
            return result;
        }
        public static void WriteLogs(string message)
        {
            var path = Directory.GetCurrentDirectory() + @"\logs\";
            try
            {
                CreateFolder(path);
                string fileName = path + @"log-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string[] strLine = { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff") + ": " + message };
                if (!File.Exists(fileName))
                {
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, fileName)))
                    {
                        foreach (string line in strLine)
                            outputFile.WriteLine(line);
                    }
                    DeleteFileHistory(path, 60);
                }
                else
                {
                    File.AppendAllLines(Path.Combine(path, fileName), strLine);
                }
            }
            catch
            {

            }
        }
        public static bool MoveFileToDestination(string _file, string _destination)
        {
            try
            {
                CreateFolder(_destination);
                FileInfo file = new FileInfo(_file);
                var file_temp = _destination + file.Name;
                if (File.Exists(file_temp))
                {
                    System.IO.File.Delete(file_temp);
                }
                System.IO.File.Move(_file, file_temp);
                return true;
            }
            catch (IOException ex)
            {
                WriteLogs("IOException MoveFile: " + ex.Message.ToString());
                return false;
            }
        }
        public static int DeleteFileHistory(string _path, int _numberFile)
        {
            int fileNumber = 0;
            try
            {
                string[] fileArray = Directory.GetFiles(_path, "*.*", SearchOption.AllDirectories);
                fileNumber = fileArray.Length;
                var numberFileDelete = fileNumber - _numberFile;
                Array.Sort(fileArray);
                if (numberFileDelete > 0)
                {
                    int i = 0;
                    foreach (var file in fileArray)
                    {
                        if (i == numberFileDelete) return numberFileDelete;
                        if (File.Exists(file)) File.Delete(file);
                        i++;
                    }
                }
            }
            catch { }
            return fileNumber;
        }
        public static void CreateFolder(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
            catch
            {

            }
        }
        public static List<string> GetFileFromDir(string path, string extention)
        {
            var fileName = (new DirectoryInfo(path))
                            .GetFiles(extention, SearchOption.AllDirectories)
                            .Select(a => a.Name)
                            .ToList();

            return fileName;
        }
        public static void MoveFileToFolder(string destination, string fileName)
        {
            try
            {
                CreateFolder(destination);
                FileInfo _file = new FileInfo(fileName);
                var _fileDestination = destination + _file.Name.ToString();
                FileInfo _destination = new System.IO.FileInfo(_fileDestination);
                if (_destination.Exists)
                {
                    _destination.Delete();
                }
                _file.MoveTo(destination + _file.Name);
            }
            catch
            {

            }
        }
    }
}
