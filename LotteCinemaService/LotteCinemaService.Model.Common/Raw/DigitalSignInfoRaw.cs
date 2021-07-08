using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Raw
{
    public class DigitalSignInfoRaw : InfoRawBase
    {
        public string NoticeID;
        public string Theater;
        public ContentsType ContentsType;
        public string Text;
        public string FontFamily;
        public string CharacterColor;
        public string CharacterBold;
        public string SkinID;
        public string IconFileName;
        public char Visible;
    }
}