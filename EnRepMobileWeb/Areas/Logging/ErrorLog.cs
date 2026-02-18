using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EnRepMobileWeb.Areas.Logging
{
    public class ErrorLog
    {
        public void LogError(string Path, Exception ex)
        {
            try
            {
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);

                string FolderPath = Path + ("..\\LogsFile\\ErrorLogFiles\\");
                string FileName = "ErrorLog_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";

                if (!Directory.Exists(FolderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(FolderPath);
                }
                string FinalFilePath = FolderPath + FileName;

                if (!File.Exists(FinalFilePath))
                {
                    string message = "-----------------------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "-----------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", ex.Message);
                    message += Environment.NewLine;
                    message += string.Format("StackTrace: {0}", ex.StackTrace);
                    message += Environment.NewLine;
                    message += string.Format("Source: {0}", ex.Source);
                    message += Environment.NewLine;
                    message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;

                    using (StreamWriter sw = File.CreateText(FinalFilePath))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }
                }
                else
                {
                    string message = "-----------------------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "-----------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", ex.Message);
                    message += Environment.NewLine;
                    message += string.Format("StackTrace: {0}", ex.StackTrace);
                    message += Environment.NewLine;
                    message += string.Format("Source: {0}", ex.Source);
                    message += Environment.NewLine;
                    message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;

                    using (StreamWriter sw = File.AppendText(FinalFilePath))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public void LogError_customsg(string Path, string msg, string lineno, string msgtype)
        {
            try
            {
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);

                string FolderPath = Path + ("..\\LogsFile\\ErrorLogFiles\\");
                string FileName = "ErrorLog_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";

                if (!Directory.Exists(FolderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(FolderPath);
                }
                string FinalFilePath = FolderPath + FileName;

                if (!File.Exists(FinalFilePath))
                {
                    string message = "-----------------------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "-----------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", msg);
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;

                    using (StreamWriter sw = File.CreateText(FinalFilePath))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }
                }
                else
                {
                    string message = "-----------------------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "-----------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", msg);
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;

                    using (StreamWriter sw = File.AppendText(FinalFilePath))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void LogError_InJS(string Path, string msg, string StackTrace, string Source, string TargetSite)
        {
            try
            {
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);

                string FolderPath = Path + ("..\\LogsFile\\ErrorLogFiles\\");
                string FileName = "ErrorLog_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";

                if (!Directory.Exists(FolderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(FolderPath);
                }
                string FinalFilePath = FolderPath + FileName;

                if (!File.Exists(FinalFilePath))
                {
                    string message = "-----------------------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "-----------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", msg);
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;

                    using (StreamWriter sw = File.CreateText(FinalFilePath))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }
                }
                else
                {
                    string message = "-----------------------" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "-----------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", msg);
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;

                    using (StreamWriter sw = File.AppendText(FinalFilePath))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

    }
}