using System.Collections.Generic;

namespace LotteCinemaLibraries.Common.Class
{
    public class DownloadItemComparer : IEqualityComparer<DownloadItem>
    {
        public bool Equals(DownloadItem x, DownloadItem y)
        {
            return x.FileName == y.FileName && x.LocalPath == y.LocalPath && x.FtpPath == y.FtpPath;
        }

        public int GetHashCode(DownloadItem obj)
        {
            return 0;
        }
    }
}