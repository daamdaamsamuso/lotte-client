using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Raw
{
    public class DigitalCurtainInfoRaw : InfoRawBase
    {
        public int Seq;
        public ContentsType ContentsType;
        public string MorningBeginDate;
        public string MorningEndDate;
        public WeatherType WeatherType;
        public string GroupID;
        public string ContentsName;
        public string FileType;
        public string FileName;
        public long FileSize;
        public int ItemPositionID;
    }
}