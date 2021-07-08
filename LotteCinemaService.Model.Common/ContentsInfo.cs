using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common
{
    public class ContentsInfo
    {
        public int ContentsID;
        public string GroupID;
        public int ItemPositionID;
        public ContentsType ContentsType;
        public string FileName;
        public long FileSize;
        public string FileType;
        public string FtpFilePath;
        public string LocalFilePath;
        public string LocalFilePath02;
    }
}