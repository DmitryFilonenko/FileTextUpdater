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
    public enum CurrentTask { SearchFiles, ReplaceText, Cancel}

    public static class Replacer
    {
        static public event Action<CurrentTask> TaskFinished;

        #region SearhFiles
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
            TaskFinished?.Invoke(CurrentTask.SearchFiles);
        }
        #endregion

        #region ReplaceText

        public static void ReplaceTextAsync()
        {
            var thread = new Thread(ReplaceText)
            {
                IsBackground = true
            };
            thread.Start();
        }

        static void ReplaceText()
        {
            int count = 0;

            foreach (var item in Mediator.FilesList)
            {
                string oldText = File.ReadAllText(item.FullName, Encoding.Default);
                if (oldText.Contains(Mediator.Search))
                {
                    string newText = oldText.Replace(Mediator.Search, Mediator.Replace);
                    File.WriteAllText(item.FullName, newText, Encoding.Default);
                    count++;
                }
            }
            Mediator.UpdatedFiles = count;
            TaskFinished?.Invoke(CurrentTask.ReplaceText);
        }

        #endregion




    }
}
