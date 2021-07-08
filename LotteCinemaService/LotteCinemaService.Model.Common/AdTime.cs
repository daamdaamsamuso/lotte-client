
namespace LotteCinemaService.Model.Common
{
    public class AdTime
    {
        public string StartTime;
        public string EndTime;
        public int SecondInterval;
        
        public AdTime()
        {
            this.StartTime = "07:00";
            this.EndTime = "26:00";
            this.SecondInterval = 90;
        }
    }
}