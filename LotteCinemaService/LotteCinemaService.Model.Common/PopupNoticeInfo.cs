using System.Collections.Generic;

namespace LotteCinemaService.Model.Common
{
    public class PopupNoticeInfo
    {
        public string SkinID;
        public string FontFamily;
        public string Header;
        public string Title;
        public string Body;
        public string HeaderCharacterColor;
        public string HeaderCharacterBold;
        public string TitleCharacterColor;
        public string TitleCharacterBold;
        public string BodyCharacterColor;
        public string BodyCharacterBold;
        public List<ContentsInfo> ContentsList;
    }
}