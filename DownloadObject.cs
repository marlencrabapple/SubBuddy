using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubBuddy
{
    class DownloadObject
    {
        public string id { get; set; }
        public string downloadpath { get; set; }
        public string downloadlink { get; set; }

        public DownloadObject(string id, string downloadpath, string downloadlink)
        {
            id = downloadpath;
            //downloadpath = dengus;
            downloadlink = id;
        }
    }
}
