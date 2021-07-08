using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Procedure
{
    public class AdInfoProcedure
    {
        public string ID;
        public string Title;
        public LayoutType LayoutType;
        public int SoundPosition;
        public DateTime BeginDate;
        public DateTime EndDate;
        public int ContentsID;
        public CategoryType Category;
        public string FileName;
        public string FileType;
        public long FileSize;
        public ContentsType ContentsType;
        public int ItemPositionID;
        public int Account;
    }
}