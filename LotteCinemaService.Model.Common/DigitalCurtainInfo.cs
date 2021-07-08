using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common
{
    public class DigitalCurtainInfo
    {
        public WeatherType WeatherType;
        public string ContentsName; 
        /// <summary>
        /// 파일 리스트
        /// </summary>
        public List<ContentsInfo> ContentsList;
        public string MorningBeginDate;
        public string MorningEndDate;
    }
}
