
namespace LotteCinemaService.Model.TBA
{
    public class BoxOfficeMovieInfo
    {
        public string MovieInfoCode;
        public string MovieTitle;
    }

    public class MappedBoxOfficeMovieInfo
    {
        public string MovieInfoCode { get; set; }
        public string TitleKor { get; set; }
    }
}