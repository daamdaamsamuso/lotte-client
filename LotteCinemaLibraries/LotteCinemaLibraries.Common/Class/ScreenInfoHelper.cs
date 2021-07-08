using System;
using LotteCinemaLibraries.Common.Enum;

namespace LotteCinemaLibraries.Common.Class
{
    public class ScreenInfoHelper
    {
        public static ScreenType GetScreenType(string screenName)
        {
            var replace = screenName.Replace(" ", "").ToUpper();

            if (replace.Contains("샤롯데") || replace.Contains("CHARLOTTE"))
            {
                if (replace.Contains("프라이빗"))
                {
                    return ScreenType.CHARLOTTE_PRIVATE;
                }
                else
                {
                    return ScreenType.CHARLOTTE;
                }
            }
            else if (replace.Contains("프레스티지") || replace.Contains("CINECOUPLE"))
            {
                return ScreenType.CINECOUPLE;
            }
            else if (replace.Contains("4D익스트림") || replace.Contains("SUPER4D"))
            {
                return ScreenType.SUPER4D;
            }
            else if (replace.Contains("사운드익스트림") || replace.Contains("SUPERSOUND"))
            {
                return ScreenType.None;
            }
            //else if (replace.Contains("바이브익스트림") || replace.Contains("SUPERVIBE"))
            //{
            //    return ScreenType.SUPERVIBE;
            //}
            //else if (replace.Contains("애니메이션") || replace.Contains("ARTEANI"))
            //{
            //    return ScreenType.ARTEANI;
            //}
            else if (replace.Contains("아르떼") || replace.Contains("ARTECLASSIC") || replace.Contains("ARTE"))
            {
                return ScreenType.ARTECLASSIC;
            }
            else if (replace.Contains("초대형") || replace.Contains("수퍼플렉스") || replace.Contains("SUPERPLEX"))
            {
                if (replace.Contains("G"))
                {
                    return ScreenType.SUPERPLEX_G;
                }
                else
                {
                    return ScreenType.SUPERPLEX;
                }
            }
            else if (replace.Contains("가족"))
            {
                return ScreenType.CHARLOTTE_PRIVATE;
            }
            else if (replace.Contains("컨벤션") || replace.Contains("CINEBIZ"))
            {
                return ScreenType.CINEBIZ;
            }
            else if (replace.Contains("패밀리부스") || replace.Contains("씨네패밀리") || replace.Contains("CINEFAMILY"))
            {
                return ScreenType.CINEFAMILY;
            }
            //else if (replace.Contains("매점") || replace.Contains("스위트") || replace.Contains("SWEET"))
            //{
            //    return ScreenType.SWEET;
            //}
            else if (replace.Contains("SUPERS") || replace.Contains("슈퍼 S"))
            {
                return ScreenType.SUPERS;
            }
            //2020.12.01
            else if (replace.Contains("COLORIUM") || replace.Contains("컬러리움"))
            {
                return ScreenType.COLORIUM;
            }


            //else if (replace.Contains("장안"))
            //{
            //    return ScreenType.JANGANUNIVERSITY;
            //}
            else
            {
                return ScreenType.None;
            }
        }

        public static string ConvertScreenName(string screenName, bool eng)
        {
            var name = string.Empty;
            var type = GetScreenType(screenName);

            if (type == ScreenType.CHARLOTTE)
            {
                name = eng ? "Charlotte" : "샤롯데";
            }
            else if (type == ScreenType.CHARLOTTE_PRIVATE)
            {
                name = eng ? "Charlotte Private" : "샤롯데 프라이빗";
            }
            else if (type == ScreenType.SUPERPLEX)
            {
                name = eng ? "Superplex" : "수퍼플렉스";
            }
            else if (type == ScreenType.SUPERPLEX_G)
            {
                name = eng ? "Superplex G" : "수퍼플렉스 지";
            }
            else if (type == ScreenType.SUPER4D)
            {
                name = eng ? "Super 4D" : "수퍼포디";
            }
            //else if (type == ScreenType.SUPERSOUND)
            //{
            //    name = eng ? "Super Sound" : "수퍼사운드";
            //}
            //else if (type == ScreenType.SUPERVIBE)
            //{
            //    name = eng ? "Super Vibe" : "수퍼바이브";
            //}
            else if (type == ScreenType.ARTECLASSIC)
            {
                name = eng ? "ARTE classic" : "아르떼클래식";
            }
            //else if (type == ScreenType.ARTEANI)
            //{
            //    name = eng ? "ARTE ani" : "아르떼애니";
            //}
            else if (type == ScreenType.CINEBIZ)
            {
                name = eng ? "Cine Biz" : "씨네비즈";
            }
            else if (type == ScreenType.CINECOUPLE)
            {
                name = eng ? "Cine Couple" : "씨네커플";
            }
            else if (type == ScreenType.CINEFAMILY)
            {
                name = eng ? "Cine Family" : "씨네패밀리";
            }
            //else if (type == ScreenType.SWEET)
            //{
            //    name = eng ? "SWEET #" : "스위트샵";
            //}
            else if (type == ScreenType.SUPERS)
            {
                name = eng ? "Super S" : "슈퍼 S";
            }
            //2020.12.01
            else if (type == ScreenType.COLORIUM)
            {
                name = eng ? "COLORIUM" : "컬러리움";
            }
            //else if (type == ScreenType.JANGANUNIVERSITY)
            //{
            //    name = eng ? "JANGAN UNIVERSITY" : "장안대학교";
            //}

            return name;
        }
    }
}