using AttributeRouting.Web.Http;
using LotteCinemaLibraries.Config;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.Model.TBA;
using LotteCinemaService.WebAPI.TBA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.TBA.Controllers
{
    public class TBAController : ApiController
    {
        private TBAManager _tbaManager;
        private CommonManager _cmManager;

        public TBAController()
        {
            this._tbaManager = new TBAManager(Settings.SERVER_DID_CONNECTION_STRING);
            this._cmManager = new CommonManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("TBA/CurrentTime")]
        public DateTime GetCurrentTime()
        {
            return this._cmManager.GetCurrentTime();
        }

        #region GetMovieInfoList ( 영화정보 )
        [GET("TBA/MovieInfoList?{cinemaCode1}&{cinemaCode2}")]
        public List<MovieInfo> GetMovieInfoList(string cinemaCode1, string cinemaCode2)
        {
            var movieList = this._cmManager.GetMovieInfoList(cinemaCode1, cinemaCode2);

            foreach (var movie in movieList)
            {
                var configFile = ConfigHelper.GetFilePath(movie.ContentsCode, "", ItemID.TBA, ContentsType.Poster);

                movie.PosterFtpFilePath = configFile.FtpFilePath;
                movie.PosterLocalFilePath = configFile.LocalFilePath;
                movie.ScreenNumber = int.Parse(movie.ScreenCode);

                if (movie.ScreenName.Contains("관"))
                {
                    var index = movie.ScreenName.IndexOf("관");
                    var number = movie.ScreenName.Remove(index);
                    int tryNumber = 0;
                    if (int.TryParse(number, out tryNumber))
                    {
                        if (tryNumber != movie.ScreenNumber)
                        {
                            movie.ScreenNumber = tryNumber;
                        }
                    }
                }
            }

            return movieList;
        }
        #endregion

        #region GetMovieTimeCellInfoList ( 영화상영정보 )
        [GET("TBA/MovieTimeCellInfoList?{cinemaCode1}&{cinemaCode2}")]
        public List<MovieTimeCellInfo> GetMovieTimeCellInfoList(string cinemaCode1, string cinemaCode2)
        {
            var movieCellList = this._cmManager.GetMovieTimeCellInfoList(cinemaCode1, cinemaCode2);

            foreach (var movieCell in movieCellList)
            {
                var configFile = ConfigHelper.GetFilePath(movieCell.ContentsCode, "", ItemID.TBA, ContentsType.Poster);

                movieCell.PosterFtpFilePath = configFile.FtpFilePath;
                movieCell.PosterLocalFilePath = configFile.LocalFilePath;
                movieCell.ScreenNumber = int.Parse(movieCell.ScreenCode);

                if (movieCell.ScreenName.Contains("관"))
                {
                    var index = movieCell.ScreenName.IndexOf("관");
                    var number = movieCell.ScreenName.Remove(index);

                    int tryNumber = 0;

                    if (int.TryParse(number, out tryNumber))
                    {
                        if (tryNumber != movieCell.ScreenNumber)
                        {
                            movieCell.ScreenNumber = tryNumber;
                        }
                    }
                }
            }
            return movieCellList;
        }
        #endregion

        [GET("TBA/AdInfoList?{cinemaCode}&{isSpecial}")]
        public List<AdInfo> GetAdInfoList(string cinemaCode, bool isSpecial)
        {
            List<AdInfo> list = new List<AdInfo>();

            var itemID = ((int)ItemID.TBA).ToString();

            if (isSpecial)
            {
                var result = this._cmManager.GetAdInfoList(cinemaCode, itemID, ContentsType.SpecialAdver);

                var groups = (from s in result
                              group s by new { s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

                foreach (var group in groups)
                {
                    AdInfo adverInfo = new AdInfo
                    {
                        ID = group.Key.ID,
                        Title = group.Key.Title,
                        LayoutType = group.Key.LayoutType,
                        SoundPosition = group.Key.SoundPosition,
                        BeginDate = group.Key.BeginDate,
                        EndDate = group.Key.EndDate,
                        ContentsList = new List<ContentsInfo>()
                    };

                    foreach (var item in group)
                    {
                        ContentsInfo contentsInfo = new ContentsInfo
                        {
                            ContentsID = item.ContentsID,
                            GroupID = group.Key.ID,
                            FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                            FileType = item.FileType,
                            FileSize = item.FileSize,
                            ItemPositionID = item.ItemPositionID,
                            ContentsType = item.ContentsType
                        };

                        var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, ItemID.TBA, ContentsType.SpecialAdver);

                        contentsInfo.FtpFilePath = configFile.FtpFilePath;
                        contentsInfo.LocalFilePath = configFile.LocalFilePath;

                        adverInfo.ContentsList.Add(contentsInfo);
                    }

                    list.Add(adverInfo);
                }

                result.Clear();
            }
            else
            {
                var result = this._cmManager.GetAdInfoList(cinemaCode, itemID, ContentsType.Adver);

                var groups = (from s in result
                              group s by new { s.Account, s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

                foreach (var group in groups)
                {
                    AdInfo adverInfo = new AdInfo
                    {
                        Account = group.Key.Account,
                        ID = group.Key.ID,
                        Title = group.Key.Title,
                        LayoutType = group.Key.LayoutType,
                        SoundPosition = group.Key.SoundPosition,
                        BeginDate = group.Key.BeginDate,
                        EndDate = group.Key.EndDate,
                        ContentsList = new List<ContentsInfo>()
                    };

                    foreach (var item in group)
                    {
                        ContentsInfo contentsInfo = new ContentsInfo
                        {
                            ContentsID = item.ContentsID,
                            GroupID = group.Key.ID,
                            FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                            FileType = item.FileType,
                            FileSize = item.FileSize,
                            ItemPositionID = item.ItemPositionID,
                            ContentsType = item.ContentsType
                        };

                        var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, ItemID.TBA, ContentsType.Adver);

                        contentsInfo.FtpFilePath = configFile.FtpFilePath;
                        contentsInfo.LocalFilePath = configFile.LocalFilePath;

                        adverInfo.ContentsList.Add(contentsInfo);
                    }

                    list.Add(adverInfo);

                    result.Clear();
                }
            }

            return list;
        }

        [GET("TBA/TheaterFloorInfoList?{cinemaCode1}&{cinemaCode2}")]
        public List<TheaterFloorInfoRaw> GetTheaterFloorInfoList(string cinemaCode1, string cinemaCode2)
        {
            return this._cmManager.GetTheaterFloorInfoList(cinemaCode1, cinemaCode2);
        }

        [GET("TBA/NoticeInfoList?{cinemaCode}")]
        public List<NoticeInfoRaw> GetNoticeInfoList(string cinemaCode)
        {
            return this._cmManager.GetNoticeInfoList(cinemaCode, ((int)ItemID.TBAInfo).ToString());
        }

        [GET("TBA/PopupNoticeInfoList?{cinemaCode}")]
        public List<PopupNoticeInfo> GetPopupNoticeInfoList(string cinemaCode)
        {
            List<PopupNoticeInfo> list = new List<PopupNoticeInfo>();

            var result = this._cmManager.GetPopupNoticeInfoList(cinemaCode, ((int)ItemID.TBAInfo).ToString());

            var groups = (from s in result
                          group s by new
                          {
                              s.SkinID,
                              s.FontFamily,
                              s.Header,
                              s.Title,
                              s.Body,
                              s.HeaderCharacterColor,
                              s.HeaderCharacterBold,
                              s.TitleCharacterColor,
                              s.TitleCharacterBold,
                              s.BodyCharacterColor,
                              s.BodyCharacterBold
                          }).ToList();

            foreach (var group in groups)
            {
                PopupNoticeInfo adverInfo = new PopupNoticeInfo
                {
                    SkinID = group.Key.SkinID,
                    FontFamily = group.Key.FontFamily,
                    Header = group.Key.Header,
                    Title = group.Key.Title,
                    Body = group.Key.Body,
                    HeaderCharacterColor = group.Key.HeaderCharacterColor,
                    HeaderCharacterBold = group.Key.HeaderCharacterBold,
                    TitleCharacterColor = group.Key.TitleCharacterColor,
                    TitleCharacterBold = group.Key.TitleCharacterBold,
                    BodyCharacterColor = group.Key.BodyCharacterColor,
                    BodyCharacterBold = group.Key.BodyCharacterBold,
                    ContentsList = new List<ContentsInfo>()
                };

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        GroupID = group.Key.SkinID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        FileSize = item.FileSize,
                        ItemPositionID = item.ItemPositionID,
                        ContentsType = item.ContentsType
                    };

                    var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, ItemID.TBAInfo, ContentsType.Skin);

                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    adverInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(adverInfo);
            }

            return list;
        }

        [GET("TBA/PlannedMovieInfoList?{cinemaCode}")]
        public List<PlannedMovieInfoRaw> GetPlannedMovieInfoList(string cinemaCode)
        {
            return this._cmManager.GetPlannedMovieInfoList(cinemaCode, ((int)ItemID.TBAInfo).ToString());
        }

        [GET("TBA/CheckList")]
        public List<string> GetCheckList()
        {
            return this._tbaManager.GetCheckList();
        }

        [HttpPost]
        [POST("TBA/AdverLogInfo?{adverInfoCode}&{cinemaCode}&{adverType}&{startTime}&{endTime}&{isBoxOffice}")]
        public void SetAdverLogInfo(string adverInfoCode, string cinemaCode, int adverType, DateTime startTime, DateTime endTime, bool isBoxOffice)
        {
            if (!this._tbaManager.SetAdverLogInfo(adverInfoCode, cinemaCode, adverType, startTime, endTime, isBoxOffice))
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
                throw new HttpResponseException(response);
            }
        }

        [HttpPut]
        [PUT("TBA/AdverUpdate?{adverInfoCode}&{update}&isBoxOffice")]
        public void SetAdverUpdate(string adverInfoCode, string update, bool isBoxOffice)
        {
            if (!this._tbaManager.SetAdverUpdate(adverInfoCode, update, isBoxOffice))
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
                throw new HttpResponseException(response);
            }
        }

        [GET("TBA/GetAdverMainInfoCode?{cinemaCode}&{isBoxOffice}")]
        public List<ADInfo> GetAdverMainInfoCode(string cinemaCode, bool isBoxOffice)
        {
            return this._tbaManager.GetAdverMainInfoCode(cinemaCode, isBoxOffice);
        }

        [GET("TBA/AdverTime")]
        public AdTime GetAdverTime()
        {
            return this._cmManager.GetAdTimeInfo("9999");
        }

        [GET("TBA/AdverTime?{theater}")]
        public AdTime GetAdverTime(string theater)
        {
            return this._cmManager.GetAdTimeInfo(theater);
        }

        [GET("TBA/TodayDateTime")]
        public DateTime GetTodayDateTime()
        {
            return this._tbaManager.GetTodayDateTime();
        }

        [GET("TBA/BoxOfficeList")]
        public List<BoxOfficeMovieInfo> GetBoxOfficeList()
        {
            return this._tbaManager.GetBoxOfficeList();
        }

        [GET("TBA/BoxOfficeList_TEST")]
        public List<BoxOfficeMovieInfo> GetBoxOfficeList_TEST() 
        {
            return this._tbaManager.GetBoxOfficeList();
        }

        [GET("TBA/ForecastList")]
        public List<ForecastInfo> GetForecastList()
        {
            return this._tbaManager.GetForecastList();
        }

        [GET("TBA/OneLineNotices?{cinemaCode}")]
        public List<NoticeInfo> GetOneLineNotices(string cinemaCode)
        {
            return this._tbaManager.GetOneLineNotices(cinemaCode);
        }

        [GET("TBA/GeneralNotice?{cinemaCode}")]
        public List<NoticeInfo> GetGeneralNotice(string cinemaCode)
        {
            return this._tbaManager.GetGeneralNotice(cinemaCode);
        }

        [GET("TBA/FloorInfoList?{cinemaCode1}&{cinemaCode2}")]
        public List<FloorInfo> GetFloorInfoList(string cinemaCode1, string cinemaCode2)
        {
            return this._tbaManager.GetFloorInfoList(cinemaCode1, cinemaCode2);
        }

        [GET("TBA/PosterAdList?{cinemaCode}")]
        public List<AdInfo> GetPosterAdList(string cinemaCode)
        {
            List<AdInfo> list = new List<AdInfo>();

            var result = this._cmManager.GetAdInfoList(cinemaCode, ((int)ItemID.TBAInfo).ToString(), ContentsType.Adver);

            var groups = (from s in result
                          group s by new { s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

            foreach (var group in groups)
            {
                AdInfo adverInfo = new AdInfo
                {
                    ID = group.Key.ID,
                    Title = group.Key.Title,
                    LayoutType = group.Key.LayoutType,
                    SoundPosition = group.Key.SoundPosition,
                    BeginDate = group.Key.BeginDate,
                    EndDate = group.Key.EndDate,
                    ContentsList = new List<ContentsInfo>()
                };

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        GroupID = group.Key.ID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        FileSize = item.FileSize,
                        ItemPositionID = item.ItemPositionID,
                        ContentsType = item.ContentsType
                    };

                    var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, ItemID.TBAInfo, ContentsType.Poster);

                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    adverInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(adverInfo);
            }

            result.Clear();

            return list;
        }

        [GET("TBA/TransparentAdInfoList?{cinemaCode}")]
        public List<TransparentAdInfo> GetTransparentAdInfoList(string cinemaCode)
        {
            List<TransparentAdInfo> list = new List<TransparentAdInfo>();

            var result = this._tbaManager.GetTransparentAdInfoList(cinemaCode);

            var resultGroup = result.GroupBy(a => a.Account);

            foreach (var item in resultGroup)
            {
                var fileName = string.Format("{0}.{1}", item.First().FileName, item.First().FileType);
                var configFile = ConfigHelper.GetFilePath(item.First().ID.ToString(), fileName, ItemID.TBA, ContentsType.TransparentAd);

                TransparentAdInfo adverInfo = new TransparentAdInfo
                {
                    ID = item.First().ID,
                    Title = item.First().Title,
                    Account = item.First().Account,
                    BeginDate = item.First().BeginDate,
                    EndDate = item.First().EndDate,
                    FtpFilePath01 = configFile.FtpFilePath,
                    LocalFilePath01 = configFile.LocalFilePath
                };

                if(item.Count()>1)
                {
                    var fileName02 = string.Format("{0}.{1}", item.ToArray()[1].FileName, item.ToArray()[1].FileType);
                     var configFile02 = ConfigHelper.GetFilePath(item.ToArray()[1].ID.ToString(), fileName02, ItemID.TBA, ContentsType.TransparentAd);
                     adverInfo.FtpFilePath02 = configFile02.FtpFilePath;
                     adverInfo.LocalFilePath02 = configFile02.LocalFilePath;
                }

                list.Add(adverInfo);
            }

            result.Clear();

            return list;
        }

        [HttpPost]
        [POST("TBA/SetItemStatus?{status}")]
        public void SetItemStatus(ItemStatusInfo status)
        {
            this._cmManager.SetItemStatusInfo(status);
        }

        [HttpPost]
        [POST("TBA/SetAdLog?{log}")]
        public bool SetAdLog(AdLogInfo log)
        {
            return this._cmManager.SetAdLogInfo(log);
        }

        [NonAction]
        private void ResponseException()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
            throw new HttpResponseException(response);
        }
    }
}