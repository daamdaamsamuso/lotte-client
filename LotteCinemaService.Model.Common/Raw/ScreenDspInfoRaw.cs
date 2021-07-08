
namespace LotteCinemaService.Model.Common.Raw
{
    public class ScreenDspInfoRaw
    {
        public string ScreenCode;
        public int DspSeq;
        public string DspType;
        public int DspLeft;
        public int DspTop;
        public int DspHeight;
        public int DspWidth;
        public string DspCaption;
    }

    public class MappedScreenDspInfoRaw
    {
        public string ScreenCode { get; set; }
        public string DspType { get; set; }
        public int dsLeft { get; set; }
        public int dsTop { get; set; }
        public int dspHight { get; set; }
        public int dspWidth { get; set; }
        public string dspCaption { get; set; }
    }
}
