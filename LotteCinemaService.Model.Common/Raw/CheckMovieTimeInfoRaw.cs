using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Model.Common.Raw
{
    public class CheckMovieTimeInfoRaw : InfoRawBase
    {
        public int Seq;
        public string CinemaCode;
        public string CinemaName;
        public string MovieCode;
        public string MovieKoreaName;
        public string ScreenCode;
        public string ScreenName;
        public string StartTime;
        public bool UseYN;
    }
}
