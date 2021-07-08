using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.DigitalSign
{
    public class DigitalSignRawInfo
    {
        public int Seq;
        public string Theater;
        public ContentsType ContentsType;
        public string Text;
        public string FontFamily;
        public string CharacterColor;
        public char Bold;
        public string BackgroundFileName;
        public BackgroundType BackgroundType;
        public string IconFileName;
    }
}