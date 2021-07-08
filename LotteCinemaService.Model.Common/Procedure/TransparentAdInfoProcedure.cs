using LotteCinemaService.Model.Enum;
using System;

namespace LotteCinemaService.Model.Common.Procedure
{
    public class TransparentAdInfoProcedure
    {
        public string ID;
        public string Title;
        public DateTime BeginDate;
        public DateTime EndDate;
        public int Account;
        public int ContentsID;
        public CategoryType Category;
        public string FileName;
        public string FileType;
        public long FileSize;
        public ContentsType ContentsType;
        public int ItemPositionID;
    }
}