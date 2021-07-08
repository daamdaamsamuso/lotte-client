using System;

namespace LotteCinemaService.Model.Common.Raw
{
    public class PopupNoticeInfoRaw : InfoRawBase
    {
        public string NoticeID;
        public string SkinID;
        public string Theater;
        public string ItemID;
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
        public char IsVisible;
    }
}