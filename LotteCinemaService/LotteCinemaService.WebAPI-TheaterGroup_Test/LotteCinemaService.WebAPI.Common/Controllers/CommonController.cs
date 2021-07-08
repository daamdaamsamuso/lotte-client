using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using LotteCinemaLibraries.Config;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.WebAPI.Common.Models;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class CommonController : ApiController
    {
        #region Variable

        private CommonManager _cmManager;

        #endregion

        #region Constructor

        public CommonController() 
        {
            this._cmManager = new CommonManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        #endregion

        #region Method

        #region Select Method

        #region 00GroupTest

        [GET("Common/TheaterGroupList")]
        public List<TheaterGroupInfoRaw> GetTheaterGroupList()
        {
            return this._cmManager.GetTheaterGroupList();
        }

        #endregion

        #region GetAdTime
        [GET("Common/AdTime?{theater}")]
        public AdTime GetAdTime(string theater)
        {
            return this._cmManager.GetAdTimeInfo(theater);
        }

        [GET("Common/AdTime")]
        public AdTime GetAdTime()
        {
            return this._cmManager.GetAdTimeInfo("9999");
        }
        #endregion

        #region GetSiteInfo

        [GET("Common/SiteInfo?{startChar}")]
        public List<SiteInfo> GetSiteInfo(string startChar)
        {
            return this._cmManager.GetSiteInfo(startChar);
        }
        #endregion

        #region GetUserInfo

        [GET("Common/UserInfo?{id}&{pw}")]
        public UserInfoRaw GetUserInfo(string id, string pw)
        {
            return this._cmManager.GetUserInfo(id, pw);
        }
        #endregion

        #region GetTheaterName
        [GET("Common/TheaterName?{cinemaCode}")]
        public string GetTheaterName(int cinemaCode)
        {
            return this._cmManager.GetTheaterName(cinemaCode);
        }
        #endregion

        #region GetCurrentTime ( 현재 시간 )
        [GET("Common/CurrentTime")]
        public DateTime GetCurrentTime()
        {
            return this._cmManager.GetCurrentTime();
        }
        #endregion

        #region ADStatusInfo
        [GET("Common/ADStatusInfo?{adcode}&{advertiser}&{startdate}&{enddate}&{statusCode}&{theter}&{itemcode}")]
        public List<AdStatusInfo> GetADStatusInfo(string adcode, string advertiser, string startdate, string enddate, int statusCode,string theater,string itemcode)
        {
            return this._cmManager.GetADStatusInfo(adcode, advertiser, startdate, enddate, statusCode,theater,itemcode);
        }
        #endregion

        #region GetMovieInfoList ( 영화정보 )
        [GET("Common/MovieInfoList?{theater1}&{theater2}")]
        public List<MovieInfo> GetMovieInfoList(string theater1, string theater2)   
        {
            var movieList = this._cmManager.GetMovieInfoList(theater1, theater2);

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

                movieList = (from m in movieList
                             orderby m.WatchCount descending, m.MovieName, m.ScreenNumber ascending
                             select m).ToList();
            }

            return movieList;
        }
        #endregion

        #region GetMovieTimeCellInfoList ( 영화상영정보 )
        [GET("Common/MovieTimeCellInfoList?{theater1}&{theater2}")]
        public List<MovieTimeCellInfo> GetMovieTimeCellInfoList(string theater1, string theater2)
        {
            var movieCellList = this._cmManager.GetMovieTimeCellInfoList(theater1, theater2);

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

        #region GetPatternInfoList ( 패턴 )
        [GET("Common/PatternInfoList?{theater}&{itemID}")]
        public List<PatternInfo> GetPatternInfoList(string theater, string itemID)
        {
            List<PatternInfo> list = new List<PatternInfo>();

            var result = this._cmManager.GetScheduleList(theater, itemID, ContentsType.Pattern);

            foreach (var schedule in result)
            {
                PatternInfo patternInfo = new PatternInfo
                {
                    OrderNo = schedule.OrderNo,
                    Type = (ContentsType)Enum.Parse(typeof(ContentsType), schedule.ID)
                };

                list.Add(patternInfo);
            }

            return list;
        }
        #endregion

        #region GetSchedule

        [GET("Common/ScheduleInfo?{itemID}&{contentsType}")]
        public List<ScheduleInfoRaw> GetScheduleInfo(string itemID, string contentsType,string theater)
        {
            List<ScheduleInfoRaw> list = new List<ScheduleInfoRaw>();
            var result = this._cmManager.GetScheduleInfo(itemID, contentsType,theater);
            return result;
        }

        #endregion

        #region GetMovieInfo

        [GET("Common/MovieInfo")]
        public List<PlannedMovieInfoRaw> GetMovieInfo()
        {
            var result = this._cmManager.GetMovieInfo();
            return result;
        }

        #endregion

        #region GetMovieShowingContent

        [GET("Common/MovieShowingContent?{theater}&{playdate}&{itemID}")]
        public List<PlannedMovieInfoRaw> GetMovieShowingContent(string theater, string playdate,string itemID)
        {
            var result = this._cmManager.GetMovieShowingContent(theater, playdate, itemID);
            return result;
        }

        #endregion

        #region GetCheckMovieTimeList
        [GET("Common/MovieTime?{theater}")]
        public List<CheckMovieTimeInfoRaw> GetCheckMovieTimeList(string theater)
        {
            var result = this._cmManager.GetCheckMovieTimeList(theater);
            return result;
        }
        #endregion

        #region GetMovieShowing

        [GET("Common/MovieShowing?{theater}&{itemID}")]
        public List<PlannedMovieInfoRaw> GetMovieShowing(string theater, string itemID)
        {
            var result = this._cmManager.GetMovieShowing(theater, itemID);
            return result;
        }

        #endregion

        #region GetAdInfo

        [GET("Common/AdInfo?{itemID}&{isSpecial}&{theater}")]
        public List<AdInfoRaw> GetAdInfo(string itemID, string contentsType,string theater)
        {
            List<AdInfoRaw> list = new List<AdInfoRaw>();
            return this._cmManager.GetAdInfo(itemID,contentsType,theater);
        }

        #endregion

        #region GetAdInfoItem

        [GET("Common/AdInfoItem?{id}")]
        public AdInfoRaw GetAdInfoItem(string id)
        {
            return this._cmManager.GetAdInfoItem(id);
        }

        #endregion 

        #region GetTransparentAdInfoItem

        [GET("Common/TransparentAdInfoItem?{id}")]
        public AdInfoRaw GetTransparentAdInfoItem(string id)
        {
            return this._cmManager.GetTransparentAdInfoItem(id);
        }

        #endregion 

        

        #region GetMediaInfo

        [GET("Common/MediaInfo?{itemID}&{theater}")]
        public List<MediaInfoRaw> GetMediaInfo(string itemID, string theater)
        {
            List<MediaInfoRaw> list = new List<MediaInfoRaw>();
            return this._cmManager.GetMediaInfo(itemID, theater);
        }

        #endregion

        #region GetMovieShowingContents

        [GET("Common/MovieContentsSetting?{theater}&{itemid}&{id}")]
        public bool GetMovieContentsSetting(string theater, string itemid,string id)
        {
            return this._cmManager.GetMovieContentsSetting(theater, itemid,id);
        }

        #endregion

        #region GetESEventLog_List

        [GET("Common/ESEventLog_List?{seq}")]
        public List<ESEventLogInfo> GetESEventLog_List(int seq)
        {
            return this._cmManager.GetESEventLog_List(seq);
        }


        #endregion

        #region GetTESTAdInfoList ( 광고 )
        [GET("Common/TESTAdInfoList?{theater}&{itemID}&{isSpecial}")]
        public List<AdInfo> GetTESTAdInfoList(string theater, string itemID, bool isSpecial)
        {
            List<AdInfo> list = new List<AdInfo>();
            var id = (ItemID)Enum.Parse(typeof(ItemID), itemID);

            if (isSpecial)
            {
                var result = this._cmManager.GetTESTAdInfoList(theater, itemID, ContentsType.SpecialAdver);

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

                        var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.SpecialAdver);

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
                if (id == ItemID.TBA || id == ItemID.NS || id == ItemID.CS || id == ItemID.ES || id == ItemID.SeatInfo)
                {
                    var result = this._cmManager.GetTESTAdInfoList(theater, itemID, ContentsType.Adver);

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

                            var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.Adver);

                            contentsInfo.FtpFilePath = configFile.FtpFilePath;
                            contentsInfo.LocalFilePath = configFile.LocalFilePath;

                            adverInfo.ContentsList.Add(contentsInfo);
                        }

                        list.Add(adverInfo);
                    }
                }
                else
                {
                    var result = this._cmManager.GetTESTScheduleList(theater, itemID, ContentsType.Adver);

                    var groups = (from s in result
                                  group s by new { s.OrderNo, s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

                    foreach (var group in groups)
                    {
                        AdInfo adverInfo = new AdInfo
                        {
                            OrderNo = group.Key.OrderNo,
                            ID = group.Key.ID,
                            Title = group.Key.Title,
                            LayoutType = (LayoutType)group.Key.LayoutType,
                            SoundPosition = group.Key.SoundPosition,
                            ContentsList = new List<ContentsInfo>(),
                            BeginDate = group.Key.BeginDate,
                            EndDate = group.Key.EndDate
                        };

                        foreach (var item in group)
                        {
                            ContentsInfo contentsInfo = new ContentsInfo
                            {
                                ContentsID = item.ContentsID,
                                GroupID = group.Key.ID,
                                FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                                FileType = item.FileType,
                                ItemPositionID = item.ItemPositionID,
                                ContentsType = item.ContentsType
                            };

                            var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.Adver);

                            contentsInfo.FtpFilePath = configFile.FtpFilePath;
                            contentsInfo.LocalFilePath = configFile.LocalFilePath;

                            adverInfo.ContentsList.Add(contentsInfo);
                        }

                        list.Add(adverInfo);
                    }

                    result.Clear();
                }
            }

            return list;
        }
        #endregion



        #region GetAdInfoList ( 광고 )
        [GET("Common/AdInfoList?{theater}&{itemID}&{isSpecial}")]
        public List<AdInfo> GetAdInfoList(string theater, string itemID, bool isSpecial)
        {
            List<AdInfo> list = new List<AdInfo>();
            var id = (ItemID)Enum.Parse(typeof(ItemID), itemID);

            if (isSpecial)
            {
                var result = this._cmManager.GetAdInfoList(theater, itemID, ContentsType.SpecialAdver);      

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

                        var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.SpecialAdver);

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
                if (id == ItemID.TBA || id == ItemID.NS || id == ItemID.CS || id == ItemID.ES || id == ItemID.SeatInfo)
                {
                    var result = this._cmManager.GetAdInfoList(theater, itemID, ContentsType.Adver);

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

                            var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.Adver);

                            contentsInfo.FtpFilePath = configFile.FtpFilePath;
                            contentsInfo.LocalFilePath = configFile.LocalFilePath;

                            adverInfo.ContentsList.Add(contentsInfo);
                        }

                        list.Add(adverInfo);
                    }
                }
                else
                {
                    var result = this._cmManager.GetScheduleList(theater, itemID, ContentsType.Adver);

                    var groups = (from s in result
                                  group s by new { s.OrderNo, s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

                    foreach (var group in groups)
                    {
                        AdInfo adverInfo = new AdInfo
                        {
                            OrderNo = group.Key.OrderNo,
                            ID = group.Key.ID,
                            Title = group.Key.Title,
                            LayoutType = (LayoutType)group.Key.LayoutType,
                            SoundPosition = group.Key.SoundPosition,
                            ContentsList = new List<ContentsInfo>(),
                            BeginDate = group.Key.BeginDate,
                            EndDate = group.Key.EndDate
                        };

                        foreach (var item in group)
                        {
                            ContentsInfo contentsInfo = new ContentsInfo
                            {
                                ContentsID = item.ContentsID,
                                GroupID = group.Key.ID,
                                FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                                FileType = item.FileType,
                                ItemPositionID = item.ItemPositionID,
                                ContentsType = item.ContentsType
                            };

                            var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.Adver);

                            contentsInfo.FtpFilePath = configFile.FtpFilePath;
                            contentsInfo.LocalFilePath = configFile.LocalFilePath;

                            adverInfo.ContentsList.Add(contentsInfo);
                        }

                        list.Add(adverInfo);
                    }

                    result.Clear();
                }
            }

            return list;
        }
        #endregion

        #region GetSubAdInfoList ( 서브 광고 )

        [GET("Common/SubAdInfoList?{theater}&{itemID}")]
        public List<AdInfo> GetSubAdInfoList(string theater, string itemID)
        {
            List<AdInfo> list = new List<AdInfo>();
            var id = (ItemID)Enum.Parse(typeof(ItemID), itemID);

            var result = this._cmManager.GetAdInfoList(theater, itemID, ContentsType.SubAdver);

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

                    var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.SpecialAdver);

                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    adverInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(adverInfo);
            }

            result.Clear();

            return list;
        }

        #endregion

        #region GetMediaInfoList ( 미디어 )
        [GET("Common/MediaInfoList?{theater}&{itemID}")]
        public List<MediaInfo> GetMediaInfoList(string theater, string itemID)
        {
            List<MediaInfo> list = new List<MediaInfo>();

            var result = this._cmManager.GetScheduleList(theater, itemID, ContentsType.Media);

            var groups = (from s in result
                          group s by new { s.OrderNo, s.ID, s.Title, s.LayoutType, s.SoundPosition }).ToList();

            foreach (var group in groups)
            {
                MediaInfo mediaInfo = new MediaInfo
                {
                    OrderNo = group.Key.OrderNo,
                    ID = group.Key.ID,
                    Title = group.Key.Title,
                    LayoutType = (LayoutType)group.Key.LayoutType,
                    SoundPosition = group.Key.SoundPosition,
                    ContentsList = new List<ContentsInfo>(),
                };

                foreach (var item in group)
                {
                    if (item.ContentsType != ContentsType.None)
                    {
                        ContentsInfo contentsInfo = new ContentsInfo
                        {
                            ContentsID = item.ContentsID,
                            ItemPositionID = item.ItemPositionID,
                            GroupID = group.Key.ID,
                            FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                            FileType = item.FileType,
                            ContentsType = item.ContentsType
                        };

                        mediaInfo.ContentsList.Add(contentsInfo);
                    }
                }

                list.Add(mediaInfo);
            }

            result.Clear();

            return list;
        }
        #endregion

        #region GetPlannedMovieInfoList ( 상영예정영화 )
        [GET("Common/PlannedMovieInfoList?{theater}&{itemID}")]
        public List<PlannedMovieInfoRaw> GetPlannedMovieInfoList(string theater, string itemID)
        {
            return this._cmManager.GetPlannedMovieInfoList(theater, itemID);
        }
        #endregion

        #region GetRecommandMovieInfoList ( 상영추천영화 )
        [GET("Common/RecommandMovieInfoList?{theater}&{itemID}")]
        public List<RecommandedMovieInfoRaw> GetRecommandMovieInfoList(string theater, string itemID)
        {
            return this._cmManager.GetRecommandMovieInfoList(theater, itemID);
        }
        #endregion

        #region GetTheaterFloorInfoList ( 층 정보 )
        [GET("Common/TheaterFloorInfoList?{theater1}&{theater2}")]
        public List<TheaterFloorInfoRaw> GetTheaterFloorInfoList(string theater1, string theater2)
        {
            return this._cmManager.GetTheaterFloorInfoList(theater1, theater2);
        }
        #endregion

        #region GetRecommandedMovieInfoList ( 추천영화 )
        [GET("Common/RecommandedMovieInfoList?{theater}&{itemID}")]
        public List<RecommandedMovieInfo> GetRecommandedMovieInfoList(string theater, string itemID)
        {
            List<RecommandedMovieInfo> list = new List<RecommandedMovieInfo>();

            var result = this._cmManager.GetRecommandedMovieInfoList(theater, itemID);

            foreach (var item in result)
            {
                RecommandedMovieInfo rmInfo = new RecommandedMovieInfo
                {
                    OrderNo = item.OrderNo,
                    Title = item.Title,
                    MovieCode = item.MovieCode,
                    RunningTime = item.RunningTime,
                    Country = item.Country,
                    Genre = item.Genre,
                    Direction = item.Direction,
                    Casts = item.Casts,
                    Grade = item.WatchClass,
                    OpenDate = item.OpenDate
                };

                var configFile = ConfigHelper.GetFilePath(rmInfo.MovieCode, "", ItemID.TMB, ContentsType.RecommendMovie);
                rmInfo.LocalFilePath = configFile.LocalFilePath;
                rmInfo.FtpFilePath = configFile.FtpFilePath;

                list.Add(rmInfo);
            }

            return list;
        }
        #endregion

        #region GetPopupNoticeInfoList
        [GET("Common/PopupNoticeInfoList?{theater}&{itemID}")]
        public List<PopupNoticeInfo> GetPopupNoticeInfoList(string theater, string itemID)
        {
            List<PopupNoticeInfo> list = new List<PopupNoticeInfo>();

            var result = this._cmManager.GetPopupNoticeInfoList(theater, itemID);

            var groups = (from s in result
                          group s by new
                          {
                              s.SkinID, s.FontFamily, s.Header, s.Title, s.Body,
                              s.HeaderCharacterColor, s.HeaderCharacterBold,
                              s.TitleCharacterColor, s.TitleCharacterBold,
                              s.BodyCharacterColor, s.BodyCharacterBold
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

                    var id = (ItemID)Enum.Parse(typeof(ItemID), itemID);
                    var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.Skin);

                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    adverInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(adverInfo);
            }

            return list;
        }
        #endregion

        #region GetPopupNoticeInfo
        [GET("Common/PopupNoticeInfo?{theater}&{itemID}")]
        public List<PopupNoticeInfoRaw> GetPopupNotice(string theater, string itemID)
        {
            List<PopupNoticeInfoRaw> list = new List<PopupNoticeInfoRaw>();
            return  this._cmManager.GetPopupNoticeInfo(theater, itemID);
        }
        #endregion   
 
        #region GetContentsInfo
        [GET("Common/ContentsInfo?{category}&{itemID}")]
        public List<ContentsInfoRaw> GetContentsInfo(int category,string itemID)
        {
            List<ContentsInfoRaw> list = new List<ContentsInfoRaw>();
            return this._cmManager.GetContentsInfo(category,itemID);
        }
        #endregion   

        #region GetContentsInfo
        [GET("Common/ContentsInfo?{groupid}")]
        public List<ContentsInfoRaw> GetContentsInfo(string groupid)
        {
            List<ContentsInfoRaw> list = new List<ContentsInfoRaw>();
            return this._cmManager.GetContentsInfo(groupid);
        }
        #endregion   

        #region GetNoticeInfoList
        [GET("Common/NoticeInfoList?{theater}&{itemID}")]
        public List<NoticeInfoRaw> GetNoticeInfoList(string theater, string itemID)
        {
            return this._cmManager.GetNoticeInfoList(theater, itemID);
        }
        #endregion

        #region GetDigitalSignNoticeInfoList
        [GET("Common/DigitalSignInfoList?{theater}&{contentsType}&{isVisible}")]
        public List<DigitalSignInfoRaw> GetDigitalSignNoticeInfoList(string theater, string contentsType,string isVisible)
        { 
            return this._cmManager.GetDigitalSignNoticeInfoList(theater, contentsType, isVisible);
        }
        #endregion

        #region GetCurtainContentsTime

          [GET("Common/CurtainContentsTime")]
        public List<string> GetCurtainContentsTime()
        {
            return this._cmManager.GetCurtainContentsTime();
        }
        #endregion

        #region GetSkinInfoList
        [GET("Common/SkinInfoList?{ItemCode}&{ContentsType}")]
        public List<SkinInfoRaw> GetSkinInfoList(string itemCode,string ContentsType)
        {
            return this._cmManager.GetSkinInfoList(itemCode,ContentsType);
        }
        #endregion

        #region GetSkinMapInfoList
        [GET("Common/SkinMapInfoList?{theater}&{itemID}&{contentType}")]
        public List<SkinMapInfo> GetSkinMapInfoList(string theater, string itemID, int contentType)
        {
            List<SkinMapInfo> list = new List<SkinMapInfo>();

            var result = this._cmManager.GetSkinMapInfoList(theater, itemID, (ContentsType)contentType);

            var groups = (from s in result
                          group s by new { s.GroupID, s.SkinID }).ToList();

            foreach (var group in groups)
            {
                SkinMapInfo skinMapInfo = new SkinMapInfo
                {
                    GroupID = group.Key.GroupID,
                    SkinID = group.Key.SkinID,
                    ContentsType = ContentsType.Skin,
                    ContentsList = new List<ContentsInfo>(),
                };

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        GroupID = group.Key.SkinID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        ItemPositionID = item.ItemPositionID,
                        ContentsType = item.ContentsType
                    };

                    skinMapInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(skinMapInfo);
            }

            result.Clear();

            return list;
        }
        #endregion 

        #region GetTESTSkinMapInfoList
        [GET("Common/TESTSkinMapInfoList?{theater}&{itemID}&{contentType}")]
        public List<SkinMapInfo> GetTESTSkinMapInfoList(string theater, string itemID, int contentType)
        {
            List<SkinMapInfo> list = new List<SkinMapInfo>();

            var result = this._cmManager.GetTESTSkinMapInfoList(theater, itemID, (ContentsType)contentType);

            var groups = (from s in result
                          group s by new { s.GroupID, s.SkinID }).ToList();

            foreach (var group in groups)
            {
                SkinMapInfo skinMapInfo = new SkinMapInfo
                {
                    GroupID = group.Key.GroupID,
                    SkinID = group.Key.SkinID,
                    ContentsType = ContentsType.Skin,
                    ContentsList = new List<ContentsInfo>(),
                };

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        GroupID = group.Key.SkinID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        ItemPositionID = item.ItemPositionID,
                        ContentsType = item.ContentsType
                    };

                    skinMapInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(skinMapInfo);
            }

            result.Clear();

            return list;
        }
        #endregion 

        #region GetEventInfoList
        [GET("Common/EventInfoList?{theater}&{itemID}")]
        public List<EventInfo> GetEventInfoList(string theater, string itemID)
        {
            List<EventInfo> list = new List<EventInfo>();

            var result = this._cmManager.GetEventInfoList(theater, itemID);

            var groups = (from s in result
                          group s by new { s.EventID, s.Title, s.LayoutType, s.SoundPosition }).ToList();

            foreach (var group in groups)
             {
                EventInfo eventContentsInfo = new EventInfo
                {
                    EventID = group.Key.EventID,
                    Title = group.Key.Title,
                    LayoutType = group.Key.LayoutType,
                    SoundPosition = group.Key.SoundPosition,
                    ContentsList = new List<ContentsInfo>()
                };

                var id = (ItemID)Enum.Parse(typeof(ItemID), itemID);

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        GroupID = group.Key.EventID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        ItemPositionID = item.ItemPositionID,
                        ContentsType = item.ContentsType
                    };

                    var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, id, ContentsType.Event);

                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    eventContentsInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(eventContentsInfo);
            }

            result.Clear();

            return list;
        }
        #endregion

        #region GetEventList
        [GET("Common/EventList?{theater}&{itemID}")]
        public List<EventInfoRaw> GetEventList(string theater, string itemID)
        {
            return this._cmManager.GetEventList(theater, itemID);
        }
        #endregion

        #region GetContractInfoList
        [GET("Common/ContractInfoList?{itemCode}&{AdvertiserName}&{ContractID}&{ContractName}")]
        public List<ContractInfoProcedure> GetContractInfoList(string itemCode, string AdvertiserName, string ContractID, string ContractName)
        {
            return this._cmManager.GetContractList(itemCode, AdvertiserName, ContractID, ContractName);
        }
        #endregion 

        #region GetAccountList
        [GET("Common/AccountList?{theater}&{itemID}&{isSpecial}&{BeginDate}&{EndDate}")]
        public List<AccountInfoProcedure> GetAccountList(string theater, string itemID, bool isSpecial, string BeginDate, string EndDate)
        {
            return this._cmManager.GetAccountList(theater, itemID, isSpecial, BeginDate, EndDate);
        }
        #endregion 

        #region GetTransparencyADAccountList
        [GET("Common/TransparencyADAccountList?{theater}&{BeginDate}&{EndDate}")]
        public List<AccountInfoProcedure> GetTransparencyADAccountList(string theater, string BeginDate, string EndDate)
        {
            return this._cmManager.GetTransparencyADAccountList(theater, BeginDate, EndDate);
        }
        #endregion 

        #region GetUserPaidAmount
        [GET("Common/UserPaidAmount?{id}")]
        public int GetUserPaidAmount(string id)
        {
            return this._cmManager.GetUserPaidAmount(id);
        }
        #endregion 

        #region GetExistsMember
        [GET("Common/ExistsMember?{id}")]
        public bool GetExistsMember(string id)
        {
            return this._cmManager.ExistsMember(id);
        }
        #endregion

        #region GetNoticeInfoItem
        [GET("Common/PopupNoticeInfoItem?{noticeID}")]
        public PopupNoticeInfoRaw GetNoticeInfoItem(string noticeID)
        {
            var result = this._cmManager.GetPopupNoticeInfoItem(noticeID);
            return result;
        }
        #endregion

        #region GetSkinMapInfo
        [HttpGet]
        [GET("Common/SkinMapInfo?{id}")]
        public string GetSkinMapInfo(string id)
        {
            return this._cmManager.GetSkinMapInfoItem(id);
        }
        #endregion

        #region GetMovieContentsInfoList
        [HttpGet]
        [GET("Common/MovieContentsInfoList")]
        public List<MovieContentsInfoRaw> GetMovieContentsInfoList()
        {
            return this._cmManager.GetMovieContentsInfoList();
        }
        #endregion

        #region GetMovieContentsUploadAvailableInfoList
        [HttpGet]
        [GET("Common/MovieContentsUploadAvailableInfoList?{startDate}&{endDate}")]
        public List<MovieContentsUploadAvailableInfo> GetMovieContentsUploadAvailableInfoList(DateTime startDate, DateTime endDate)
        {
            return this._cmManager.GetMovieContentsUploadAvailableInfoList(startDate, endDate);
        }
        #endregion

        #region GetESEventInfomationList
        [HttpGet]
        [GET("Common/ESEventInfomationList?{cinemaCode01}&{cinemaCode02}&{beginDate}&{endDate}")]
        public List<ESEventInfomationRawInfo> GetESEventInfomationList(string cinemaCode01, string cinemaCode02, string beginDate, string endDate)
        {
            return this._cmManager.GetESEventInfomationList(cinemaCode01,cinemaCode02,beginDate,endDate);
        }
        #endregion

        #endregion

        #region Insert Method

        #region SetLimitedAdverInfo
        [HttpPut]
        [PUT("Common/SetLimitedAdverInfo?{ad}")]
        public void SetLimitedAdverInfo(AdInfoRaw ad)
        {
            if (!this._cmManager.InsertLimitedADInfo(ad))
            {
                ResponseException();
            }
        }
        #endregion

        #region SetTransparentAdverInfo
        [HttpPut]
        [PUT("Common/SetTransparentAdverInfo?{ad}")]
        public void SetTransparentAdverInfo(AdInfoRaw ad)
        {
            if (!this._cmManager.InsertTransparentAdverInfo(ad))
            {
                ResponseException();
            }
        }
        #endregion

        #region SetCheckMovieTimeInfo
        [HttpPut]
        [PUT("Common/CheckMovieTimeInfo?{movietimecell}")]
        public void SetCheckMovieTimeInfo(MovieTimeCellInfo movietimecell)
        {
            if (!this._cmManager.InsertCheckMovieTimeInfo(movietimecell))
            {
                ResponseException();
            }
        }
        #endregion     

        #region SetAdverInfo
        [HttpPut]
        [PUT("Common/SetAdverInfo?{ad}")]
        public void SetAdverInfo(AdInfoRaw ad)
        {
            if (!this._cmManager.InsertADInfo(ad))
            {
                ResponseException();
            }
        }
        #endregion 

        #region SetMediaInfo
        [HttpPut]
        [PUT("Common/MediaInfo?{media}")]
        public void SetMediaInfo(MediaInfoRaw media)
        {
            if (!this._cmManager.InsertMediaInfo(media))
            {
                ResponseException();
            }
        }
        #endregion 

        #region SetContentInfo
        [HttpPut]
        [PUT("Common/SetContentsInfo?{content}")]
        public bool SetContentInfo(ContentsInfoRaw content) 
        {
            var result = false;

            result = this._cmManager.InsertContentsInfo(content);

            if(result)
            {
                if(this._cmManager.InsertOriginFileInfo(content))
                {
                    return result;
                }
                else
                {
                    ResponseException();
                }
            }
            else
            {
                ResponseException();
            }

            return result;
        }
        
        #endregion 

        #region SetPlannedMovieInfoList
        [HttpPut]
        [PUT("Common/PlannedMovieInfoList?{movie}")]
        public bool SetPlannedMovieInfoList(PlannedMovieInfoRaw movie)
        {
            return this._cmManager.SetPlannedMovieInfoList(movie);
        }
        #endregion

        #region SetMovieShowing
        [HttpPut]
        [PUT("Common/MovieShowing?{movie}")]
        public bool SetMovieShowing(PlannedMovieInfoRaw movie)
        {
            return this._cmManager.SetMovieShowing(movie);
        }
        #endregion

        #region SetPlannedMovieInfoList
        [HttpPut]
        [PUT("Common/RecommandMovieInfoList?{movie}")]
        public bool SetRecommandMovieInfoList(RecommandedMovieInfoRaw movie)
        {
            return this._cmManager.SetRecommandMovieInfoList(movie);
        }
        #endregion

        #region SetSkinMapInfo
        [HttpPut]
        [PUT("Common/SetSkinMapInfo?{skinmap}")]
        public void SetSkinMapInfo(SkinMapInfoProcedure skinmap)
        {
            if (!this._cmManager.InsertSkinMapInfo(skinmap))
            {
                ResponseException();
            }
        }
        #endregion

        #region SetSkinInfo
        [HttpPut]
        [PUT("Common/SetSkinInfo?{skin}")]
        public void SetSkinInfo(SkinInfoRaw skin)
        {
            if (!this._cmManager.InsertSkinInfo(skin))
            {
                ResponseException();
            }
        }
        #endregion 

        #region SetEventInfo
        [HttpPut]
        [PUT("Common/SetEventInfo?{eventItem}")]
        public void SetEventInfo(EventInfoRaw eventItem)
        {
            if (!this._cmManager.InsertEventInfo(eventItem))
            {
                ResponseException();
            }
        }
        #endregion 

        #region SetNoticeInfo
        [HttpPut]
        [PUT("Common/SetNoticeInfo?{notice}")]
        public void SetNoticeInfo(NoticeInfoRaw notice)
        {
            if (!this._cmManager.InsertNoticeInfo(notice))
            {
                ResponseException();
            }
        }

        #region UpdateNoticeInfo
        [HttpPut]
        [PUT("Common/UpdateNoticeInfo?{notice}")]
        public void UpdateNoticeInfo(NoticeInfoRaw notice)
        {
            if (!this._cmManager.UpdateNoticeInfo(notice))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateMovieContentsSetting
        [HttpGet]
        [GET("Common/UpdateMovieContentsSetting?{code}&{isAuto}&{id}")]
        public bool UpdateMovieContentsSetting(string code,string isAuto,string id)
        {
            return this._cmManager.UpdateMovieContentsSetting(code, isAuto, id);
        }
        #endregion
        #endregion

        #region SetDisitalSignInfo
        [HttpPut]
        [PUT("Common/SetDigitalSignInfo?{DSItem}")]
        public void SetDigitalSignInfo(DigitalSignInfoRaw DSItem)
        {
            if (!this._cmManager.InsertDigitalSignInfo(DSItem))
            {
                ResponseException();
            }
        }
        
        #endregion 

       

        #region SetPopupNoticeInfo
        [HttpPut]
        [PUT("Common/SetPopupNoticeInfo?{item}")]
        public void SetPopupNoticeInfo(PopupNoticeInfoRaw item)
        {
            if (!this._cmManager.InsertPopupNoticeInfo(item))
            {
                ResponseException();
            }
        }
        #endregion 

        #region SetScheduleInfo
        [HttpPut]
        [PUT("Common/SetScheduleInfo?{schedule}")]
        public void SetScheduleInfo(ScheduleInfoRaw schedule)
        {
            if (!this._cmManager.SetScheduleInfo(schedule))
            {
                ResponseException(); 
            }
        }
        #endregion 

        #region SetItemStatus ( 아이템 상태 )
        [HttpPost]
        [POST("Common/SetItemStatus?{status}")]
        public void SetItemStatus(ItemStatusInfo status)
        {
            this._cmManager.SetItemStatusInfo(status);
        }
        #endregion 

        #region SetAdLog ( 로그 )
        [HttpPost]
        [POST("Common/SetAdLog?{log}")]
        public bool SetAdLog(AdLogInfo log)
        {
            return this._cmManager.SetAdLogInfo(log);
        }
        #endregion 

        #region SetMovieContentsInfo ( 현재 상영작/상영 예정작 영화 컨텐츠 정보 )
        [HttpPost]
        [POST("Common/SetMovieContentsInfo?{info}")]
        public bool SetMovieContentsInfo(MovieContentsInfoRaw info)
        {
            return this._cmManager.InsertMovieContentsInfo(info);
        }
        #endregion 

        #endregion

        #region Update Method

        #region UpdateSkinMapInfo
        [HttpPut]
        [PUT("Common/UpdateSkinMapInfo?{skinmap}")]
        public void UpdateSkinMapInfo(SkinMapInfoProcedure skinmap)
        {
            if (!this._cmManager.UpdatgeSkinMapInfo(skinmap))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateContentsInfo
        [HttpPut]
        [PUT("Common/UpdateContentsInfo?{content}")]
        public void UpdateContentsInfo(ContentsInfoRaw content)
        {
            if (!this._cmManager.UpdateContentsInfo(content))
            {
                ResponseException();
            }
        }

        #endregion

        #region UpdateEventContentsInfo
        [HttpPut]
        [PUT("Common/UpdateEventContentsInfo?{content}")]
        public void UpdateEventContentsInfo(ContentsInfoRaw content)
        {
            if (!this._cmManager.UpdateEventContentsInfo(content))
            {
                ResponseException();
            }
        }

        #endregion 

        #region UpdateMediaInfo
        [HttpPut]
        [PUT("Common/UpdateMediaInfo?{media}")]
        public void UpdateMediaInfo(MediaInfoRaw media)
        {
            if (!this._cmManager.UpdateMediaInfo(media))
            {
                ResponseException();
            }
        }
        #endregion 

        #region UpdateAdverInfo
        [HttpPut]
        [PUT("Common/UpdateAdverInfo?{ad}")]
        public void UpdateAdverInfo(AdInfoRaw ad)
        {
            if (!this._cmManager.UpdateAdverInfo(ad))
            {
                ResponseException();
            }
        }
        #endregion 

        #region UpdateLimitedAdverInfo
        [HttpPut]
        [PUT("Common/UpdateLimitedAdverInfo?{ad}")]
        public void UpdateLimitedAdverInfo(AdInfoRaw ad)
        {
            if (!this._cmManager.UpdateLimitedAdverInfo(ad))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateTransparentAdverInfo
        [HttpPut]
        [PUT("Common/UpdateTransparentAdverInfo?{ad}")]
        public void UpdateTransparentAdverInfo(AdInfoRaw ad)
        {
            if (!this._cmManager.UpdateTransparentAdverInfo(ad))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateCheckMovieTime
        [HttpPut]
        [PUT("Common/UpdateCheckMovieTime?{checkStatus}&{id}")]
        public void UpdateCheckMovieTime(bool checkStatus, int id)
        {
            if (!this._cmManager.UpdateCheckMovieTime(checkStatus, id))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateSkinInfo
        [HttpPut]
        [PUT("Common/UpdateSkinInfo?{skin}")]
        public void UpdateSkinInfo(SkinInfoRaw skin)
        {
            if (!this._cmManager.UpdateSkinInfo(skin))
            {
                ResponseException();
            }
        }
        #endregion 

        #region UpdateEventInfo
        [HttpPut]
        [PUT("Common/UpdateEventInfo?{eventItem}")]
        public void UpdateEventInfo(EventInfoRaw eventItem)
        {
            if (!this._cmManager.UpdateEventInfo(eventItem))
            {
                ResponseException();
            }
        }
        #endregion 

        #region UpdateNoticeUSEYN
        [HttpPut]
        [PUT("Common/UpdateNoticeUSEYN?{noticeID}")]
        public void SetUpdateNoticeInfo(string noticeID)
        {
            if (!this._cmManager.UpdateNoticeUSEYN(noticeID))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdatePopupNoticeUSEYN

        [HttpPut]
        [PUT("Common/UpdatePopupNoticeUSEYN?{noticeID}")]
        public void UpdateDB_PopupNoticeUSEYN(string noticeID)
        {
            if (!this._cmManager.UpdateDB_PopupNoticeUSEYN(noticeID))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateDisitalSignInfo
        [HttpPut]
        [PUT("Common/UpdateDigitalSignInfo?{DSItem}")]
        public void UpdateDigitalSignInfo(DigitalSignInfoRaw DSItem)
        {
            if (!this._cmManager.UpdateDigitalSignInfo(DSItem))
            {
                ResponseException();
            }
        }
        #endregion 

        #region UpdatePopupNoticeInfo
        [HttpPut]
        [PUT("Common/UpdatePopupNoticeInfo?{item}")]
        public void UpdatePopupNoticeInfo(PopupNoticeInfoRaw item)
        {
            if (!this._cmManager.UpdatePopupNoticeInfo(item))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateDigitalSignSkin
        [HttpPut]
        [PUT("Common/UpdateDigitalSignSkin?{skinID}")]
        public void SetUpdateDigitalSignSkin(string skinID)
        {
            if (!this._cmManager.UpdateDigitalSignSkin(skinID))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateDSNoticeUSEYN
        [HttpPut]
        [PUT("Common/UpdateDSNoticeUSEYN?{noticeID}")]
        public void SetUpdateDSNoticeUSEYN(string noticeID)
        {
            if (!this._cmManager.UpdateDSNoticeUSEYN(noticeID))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdatePopupNoticeUSEYN
        [HttpPut]
        [PUT("Common/UpdatePopupNoticeUSEYN?{noticeID}")]
        public void SetUpdatePopupNoticeUSEYN(string noticeID)
        {
            if (!this._cmManager.UpdateDSNoticeUSEYN(noticeID))
            {
                ResponseException();
            }
        }
        #endregion

        #region UpdateESEventInformation
        [HttpPut]
        [PUT("Common/UpdateESEventInformation?{eventInfomationRawInfo}")]
        public void UpdateESEventInformation(ESEventInfomationRawInfo eventInfomationRawInfo)
        { 
            var seq = eventInfomationRawInfo.Seq;
            var startTime = eventInfomationRawInfo.EventStartTime;
            var isDisplayedNull = eventInfomationRawInfo.isDisplayedNull;
            var isDisplayed = eventInfomationRawInfo.isDisplayed ? 'Y' : 'N';
            var isOpenNull = eventInfomationRawInfo.isOpenNull;
            var isOpen = eventInfomationRawInfo.isOpen ? 'Y' : 'N';

            if (!this._cmManager.UpdateESEventInformation(seq, startTime, isDisplayedNull, isDisplayed, isOpenNull, isOpen))
            {
                ResponseException();
            }
        }
        #endregion

        #endregion

        #region Delete Method

        #region DeletePlannedMovieInfoList ( 상영예정영화 )
        [DELETE("Common/PlannedMovieInfoList?{theater}&{itemID}")]
        public bool DeletePlannedMovieInfoList(string theater, string itemID)
        {
            return this._cmManager.DeletePlannedMovieInfoList(theater, itemID);
        }
        #endregion

        #region DeleteMovieShowingInfoList ( 상영예정영화 )
        [DELETE("Common/MovieShowing?{theater}&{itemID}")]
        public bool DeleteMovieShowingInfoList(string theater, string itemID)
        {
            return this._cmManager.DeleteMovieShowingInfo(theater, itemID);
        }
        #endregion

        #region DeleteRecommandMovieInfoList ( 상영예정영화 )
        [DELETE("Common/RecommandMovieInfoList?{theater}&{itemID}")]
        public bool DeleteRecommandMovieInfoList(string theater, string itemID)
        {
            return this._cmManager.DeleteRecommandMovieInfoList(theater, itemID);
        }
        #endregion

        #region DeleteScheduleInfo(스케쥴 삭제)
        [DELETE("Common/ScheduleInfo?{theater}&{itemID}&{contentsType}")]
        public bool DeleteScheduleInfo(string theater, string itemID, string contentsType)
        {
            return this._cmManager.DeleteScheduleInfo(theater, itemID, contentsType);
        }
        #endregion

        #region DeleteADInfo(광고 삭제)
        [DELETE("Common/ADInfo?{id}")]
        public bool DeleteADInfo(string id)
        {
            return this._cmManager.DeleteADInfo(id);
        }
        #endregion

        #region DeleteTransparentAdInfo(투명광고 삭제)
        [DELETE("Common/TransparentAdInfo?{id}")]
        public bool DeleteTransparentAdInfo(string id)
        {
            return this._cmManager.DeleteTransparentAdInfo(id);
        }
        #endregion

        #region DeleteCheckMovieTime(광고 삭제)
        [DELETE("Common/CheckMovieTime?{id}")]
        public bool DeleteCheckMovieTime(string id)
        {
            return this._cmManager.DeleteCheckMovieTime(id);
        }
        #endregion

        #region DeleteADInfo(미디어 삭제)
        [DELETE("Common/MediaInfo?{id}")]
        public bool DeleteMediaInfo(string id)
        {
            return this._cmManager.DeleteMediaInfo(id);
        }
        #endregion

        #region DeleteNoticeInfo
        [HttpDelete]
        [DELETE("Common/NoticeInfo?{notice}")]
        public void DeleteNoticeInfo(string noticeID)
        {
            if (!this._cmManager.DeleteNoticeInfo(noticeID))
            {
                ResponseException();
            }
        }
        #endregion

        #region DeletePopupNoticeInfo
        [HttpDelete]
        [DELETE("Common/PopupNoticeInfo?{notice}")]
        public void DeletePopupNoticeInfo(string noticeID)
        {
            if (!this._cmManager.DeletePopupNoticeInfo(noticeID))
            {
                ResponseException();
            }
        }
        #endregion

        #region DeleteEventInfo
        [HttpDelete]
        [DELETE("Common/EventInfo?{eventid}")]
        public void DeleteEventInfo(string eventid)
        {
            if (!this._cmManager.DeleteEventInfo(eventid))
            {
                ResponseException();
            }
        }
        #endregion

        #region DeleteSkinInfo
        [HttpDelete]
        [DELETE("Common/SkinInfo?{skinID}")]
        public void DeleteSkinInfo(string skinID)
        {
            if (!this._cmManager.DeleteSkinInfo(skinID))
            {
                ResponseException();
            }
        }
        #endregion

        #region DeleteSkinMap
        [HttpDelete]
        [DELETE("Common/SkinMapInfo?{Code}")]
        public void DeleteSkinMapInfo(string Code)
        {
            if (!this._cmManager.DeleteSkinMapInfo(Code))
            {
                ResponseException();
            }
        }
        #endregion

        #region DeleteMovieContentsInfo
        [HttpDelete]
        [DELETE("Common/MovieContentsInfo?{seq}")]
        public void DeleteSkinMapInfo(int seq)
        {
            if (!this._cmManager.DeleteMovieContentsInfo(seq))
            {
                ResponseException();
            }
        }
        #endregion

        #endregion

        #region Helper Method

        #region ResponseException
        [NonAction]
        private void ResponseException()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
            throw new HttpResponseException(response);
        }
        #endregion

        #endregion

        #endregion
    }
}