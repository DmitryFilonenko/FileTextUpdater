using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using TextReplacerWpf.Infrstr;

namespace TextReplacerWpf.Models
{
    public static class Replacer
    {
        static public event Action TaskFinished;

        public static bool GetPathToDir()
        {
            bool res = false;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = @"x:\Progs\Registry",
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Mediator.PathToDir = dialog.FileName;
                res = true;
            }
            return res;
        }

        public static void GetFilesAsync()
        {            
            var thread = new Thread(GetFiles)
            {
                IsBackground = true
            };
            thread.Start();
        }

        static void GetFiles()
        {
            DirectoryInfo di = new DirectoryInfo(Mediator.PathToDir);
            Mediator.FilesList = new List<FileInfo>(); 
            Mediator.FilesList = di.GetFiles(Mediator.Pattern, SearchOption.AllDirectories).ToList();
            TaskFinished?.Invoke();
        }








        //static Task<string> StrMethod()
        //{
        //    WebClient webClient = new WebClient();
        //    Task<string> task = webClient.DownloadStringTaskAsync("http://habrahabr.ru/");
        //    //task = webClient.DownloadStringTaskAsync("https://rozetka.com.ua/");
        //    task = new WebClient().DownloadStringTaskAsync("https://appsrv.eadr.com.ua/");
        //   // MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
        //    return task;
        //}

        //static Task<string> StrMethod___1()
        //{
        //    MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
        //    //Thread.Sleep(1000);
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        int res = i / i + 1;
        //    }
        //    MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
        //    return Task.FromResult<string>("Other");
        //}
    }
}
