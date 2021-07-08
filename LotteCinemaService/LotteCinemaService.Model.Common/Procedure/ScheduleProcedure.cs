using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Procedure
{
    public class ScheduleProcedure
    {
        public int OrderNo;
        public ContentsType ScheduleType;
        public int SoundPosition;
        public string ID;
        public int LayoutType;
        public string Title;
        public ContentsType AdType;
        public DateTime BeginDate;
        public DateTime EndDate;
        public string FileName;
        public string FileType;
        public long FileSize;
        public int ItemPositionID;
        public ContentsType ContentsType;
        public CategoryType Category;
        public string IsFull;
        public int ContentsID;
    }
}