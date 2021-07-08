
namespace LotteCinemaService.Model.Common.Raw
{
    public class MovistInfoRaw
    {
        public int MovistType;
        public int Priority;
        public string Name;
    }

    public class MappedMovistInfoRaw
    {
        public int MovistType { get; set; }
        public int Priority { get; set; }
        public string NameKor { get; set; }
    }
}
