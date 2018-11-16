using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextReplacerWpf.Infrstr
{
    public static class Mediator
    {
        public static string Search { get; set; }
        public static string Replace { get; set; }
        public static string Pattern { get; set; }

        public static string PathToDir { get; set; }
        public static List<FileInfo> FilesList { get; set; }
    }
}
