using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotte_Client.Models
{
    public class AdInfoRaw
    {
        public AdInfoRaw()
        {
            ContentsList = new List<Contents>();
        }
        public int LayoutType { get; set; }
        public List<Contents> ContentsList { get; set; }
    }

    public class Contents
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
