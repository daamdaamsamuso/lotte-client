namespace LotteCinemaLibraries.Common.LCRenewal
{
    public class LCTypeManager
    {
        public static LCScreenDivCode ConvertScreenDivCode(string divCode)
        {
            if (divCode == "100") return LCScreenDivCode.Normal;
            if (divCode == "200") return LCScreenDivCode.CineCouple;
            if (divCode == "300") return LCScreenDivCode.CharLotte;
            if (divCode == "301") return LCScreenDivCode.CharLottePrivate;
            if (divCode == "400") return LCScreenDivCode.ArteClassic;
            if (divCode == "401") return LCScreenDivCode.ArteAni;
            if (divCode == "700") return LCScreenDivCode.WineCinemaTrain;
            if (divCode == "800") return LCScreenDivCode.KTXCinema;
            if (divCode == "900") return LCScreenDivCode.SuperSound;
            if (divCode == "910") return LCScreenDivCode.SuperVibe;
            if (divCode == "920") return LCScreenDivCode.CineCoupleSeat;
            if (divCode == "930") return LCScreenDivCode.Super4D;
            if (divCode == "940") return LCScreenDivCode.SuperPlex;
            if (divCode == "941") return LCScreenDivCode.SuperPlexG;
            if (divCode == "950") return LCScreenDivCode.CineBiz;
            if (divCode == "960") return LCScreenDivCode.CineFamily;
            if (divCode == "980") return LCScreenDivCode.SuperS;
            //2020.11.30
            if (divCode == "988") return LCScreenDivCode.COLORIUM;

            return LCScreenDivCode.None;
        }
         
        public static LCFilmCode ConvertFilmCode(string filmCode)
        {
            if (filmCode == "100") return LCFilmCode.Film; // 일반
            if (filmCode == "200") return LCFilmCode._2D;
            if (filmCode == "300") return LCFilmCode._3D;

            return LCFilmCode.None;
        }

        public static LC4DTypeCode Convert4DTypeCode(int _4DTypeCode)
        {
            if (_4DTypeCode == 0) return LC4DTypeCode.None;
            if (_4DTypeCode == 100) return LC4DTypeCode.Normal;

            //if (_4DTypeCode == 200) return LC4DTypeCode.Super4D;
            //if (_4DTypeCode == 300) return LC4DTypeCode.SuperVibe;
            //if (_4DTypeCode == 900) return LC4DTypeCode.Normal_SuperVibe;

            return LC4DTypeCode._4D;
        }

        public static LCCaptionCode ConvertCaptionCode(string captionCode)
        {
            if (captionCode == "50") return LCCaptionCode.Dubbing;
            if (captionCode == "100") return LCCaptionCode.Subtitle;
            if (captionCode == "200") return LCCaptionCode.Subtitle;
            if (captionCode == "300") return LCCaptionCode.Subtitle;
            if (captionCode == "500") return LCCaptionCode.Subtitle;
            if (captionCode == "600") return LCCaptionCode.Subtitle;
            if (captionCode == "800") return LCCaptionCode.SignLanguage;
            if (captionCode == "850") return LCCaptionCode.Subtitle;
            if (captionCode == "900") return LCCaptionCode.None;

            return LCCaptionCode.None;
        }
    }
}
